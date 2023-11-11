using System;
using DS;
using DS.Core;
using DS.Runtime;
using Quest;
using UnityEngine;

public class PlayerInteractionState : IState
{
    private PlayerController player;
    private StateMachine stateMachine;
    
    private DialogueFlow dialogueFlow;
    private DialogueHandler lastestDialogueHandler;

    private float currentXAxis;
    private Action exitAction;

    public PlayerInteractionState(PlayerController player, StateMachine stateMachine, DialogueContainer dialogueContainer, Action exitAction)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        dialogueFlow = new DialogueFlow(dialogueContainer);
        this.exitAction = exitAction;

        player.Rb.velocity = Vector3.zero;
    }

    public void Enter()
    {
        player.Interaction.Enabled = false;
        UIManager.Instance.PlayerInput.enabled = false;
        currentXAxis = VirtualCameraController.Instance.GetXAxis();
        PlayCurrentNode();
    }

    public void Execute()
    {
        if (!player.InteractionInput)
            return;
        
        player.InteractionInput = false;

        CheckDialoguePlaying(PlayCurrentNode);
    }
    
    public void SelectChoice(string guid)
    {
        player.StopInteractionInput = false;
        
        var nodeLinks = dialogueFlow.GetCurrentNodeLinks();
        
        if (nodeLinks.Count == 0)
        {
            stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
            return;
        }
        
        dialogueFlow.ChangeCurrentNodeData(guid);
        
        PlayCurrentNode();
    }

    private void CheckDialoguePlaying(Action<bool> action)
    {
        if (!DialogueManager.Instance.CheckDialogueEnd())
        {
            var nodeData = dialogueFlow.GetCurrentNodeData();
            if (nodeData.NodeType == NodeTypes.NodeType.NoChoice
                && (nodeData as NoChoiceNodeData).SkipDelay < 0f)
                return;
            
            action?.Invoke(true);
            return;
        }
        
        var nodeLinks = dialogueFlow.GetCurrentNodeLinks();
            
        if (nodeLinks.Count == 0)
        {
            stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
            return;
        }

        dialogueFlow.ChangeCurrentNodeData(nodeLinks[0].TargetNodeGuid);
        
        action?.Invoke(false);
    }
    
    private void PlayCurrentNode(bool skipTyping = false)
    {
        var nodeData = dialogueFlow.GetCurrentNodeData();
        var oldDialogueHandler = lastestDialogueHandler;
        switch (nodeData.NodeType)
        {
            case NodeTypes.NodeType.NoChoice:
                var noChoiceNodeData = nodeData as NoChoiceNodeData;
                lastestDialogueHandler = DialogueManager.Instance.GetHandler(noChoiceNodeData.TargetObjectID);

                if (oldDialogueHandler != null)
                {
                    currentXAxis = oldDialogueHandler.GetXAxis();
                    oldDialogueHandler.DisableLookTarget();
                }
                
                lastestDialogueHandler.LookTarget(currentXAxis);
                lastestDialogueHandler.PlayDialogue(noChoiceNodeData.DialogueText, skipTyping);

                if (skipTyping == false && noChoiceNodeData.SkipDelay >= 0f)
                {
                    player.StopInteractionInputUntil(noChoiceNodeData.SkipDelay);
                }
                break;
            case NodeTypes.NodeType.MultiChoice:
                player.StopInteractionInput = true;
                
                var multiChoiceNodeData = nodeData as MultiChoiceNodeData;
                lastestDialogueHandler = DialogueManager.Instance.GetHandler(multiChoiceNodeData.TargetObjectID);

                if (oldDialogueHandler != null)
                {
                    currentXAxis = oldDialogueHandler.GetXAxis();
                    oldDialogueHandler.DisableLookTarget();
                }
                
                lastestDialogueHandler.LookTarget(currentXAxis);
                lastestDialogueHandler.PlayDialogue(multiChoiceNodeData.DialogueText, skipTyping);
                
                var nodeLinks = dialogueFlow.GetCurrentNodeLinks();
                
                MultiDialogueHandler.Instance.Init(this, nodeLinks);
                break;
            case NodeTypes.NodeType.StartQuest:
                var startQuestNodeData = dialogueFlow.GetCurrentNodeData() as StartQuestNodeData;
                QuestManager.Instance.Accept(startQuestNodeData.QuestType, startQuestNodeData.QuestID);

                CheckDialoguePlaying(PlayCurrentNode);
                break;
        }
    }

    public void Exit()
    {
        if (lastestDialogueHandler != null)
        {
            VirtualCameraController.Instance.SetXAxis(lastestDialogueHandler.GetXAxis());
            lastestDialogueHandler.DisableLookTarget();
        }
        
        player.Interaction.SetInteractionEnableAfterDelay();
        UIManager.Instance.PlayerInput.enabled = true;
        DialogueManager.Instance.StopDialogue();
        
        exitAction?.Invoke();
    }
}

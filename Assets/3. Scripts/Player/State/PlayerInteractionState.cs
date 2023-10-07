using System;
using Cinemachine;
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

    public PlayerInteractionState(PlayerController player, StateMachine stateMachine, DialogueContainer dialogueContainer)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        dialogueFlow = new DialogueFlow(dialogueContainer);
    }

    public void Enter()
    {
        player.Interaction.Enabled = false;
        CameraZoomController.Instance.ZoomIn();
        PlayCurrentNode();
    }

    public void Execute()
    {
        if (!player.InteractionInput)
            return;
        player.InteractionInput = false;

        CheckDialoguePlaying(PlayCurrentNode);
    }

    private void CheckDialoguePlaying(Action<bool> action)
    {
        if (!DialogueManager.Instance.CheckDialogueEnd())
        {
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
        switch (nodeData.NodeType)
        {
            case NodeTypes.NodeType.NoChoice:
                var noChoiceNodeData = nodeData as NoChoiceNodeData;
                DialogueManager.Instance.GetHandler(noChoiceNodeData.TargetObjectID)
                    .PlayDialogue(noChoiceNodeData.DialogueText, skipTyping);
                break;
            case NodeTypes.NodeType.MultiChoice:
                // player.StopInteractionInput = true;
                var multiChoiceNodeData = nodeData as MultiChoiceNodeData;
                DialogueManager.Instance.GetHandler(multiChoiceNodeData.TargetObjectID)
                    .PlayDialogue(multiChoiceNodeData.DialogueText, skipTyping);
                
                var nodeLinks = dialogueFlow.GetCurrentNodeLinks();
                foreach (var nodeLink in nodeLinks)
                {
                    Debug.Log(nodeLink.PortName);
                }
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
        player.Interaction.Enabled = true;
        CameraZoomController.Instance.ZoomOut();
        DialogueManager.Instance.StopDialogue();
    }
}

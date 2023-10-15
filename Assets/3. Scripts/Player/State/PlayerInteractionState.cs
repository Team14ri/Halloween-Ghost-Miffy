using System;
using System.Collections;
using Cinemachine;
using DS;
using DS.Core;
using DS.Runtime;
using Quest;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerInteractionState : IState
{
    private PlayerController player;
    private StateMachine stateMachine;
    
    private DialogueFlow dialogueFlow;

    private DialogueHandler lastestDialogueHandler;

    private float currentXAxis;

    public PlayerInteractionState(PlayerController player, StateMachine stateMachine, DialogueContainer dialogueContainer)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        dialogueFlow = new DialogueFlow(dialogueContainer);
    }

    public void Enter()
    {
        player.Interaction.Enabled = false;
        // CameraZoomController.Instance.ZoomIn();
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
                // player.StopInteractionInput = true;
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
        if (lastestDialogueHandler != null)
        {
            VirtualCameraController.Instance.SetXAxis(lastestDialogueHandler.GetXAxis());
            lastestDialogueHandler.DisableLookTarget();
        }
        
        player.Interaction.SetInteractionEnableAfterDelay();
        // CameraZoomController.Instance.ZoomOut();
        DialogueManager.Instance.StopDialogue();
    }
}

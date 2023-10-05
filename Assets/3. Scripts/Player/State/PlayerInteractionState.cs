using System;
using DS;
using DS.Core;
using DS.Runtime;
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
        PlayCurrentNode();
    }

    public void Execute()
    {
        if (!player.InteractionInput)
            return;
        player.InteractionInput = false;

        CheckDialoguePlaying((skipTyping) =>
        {
            PlayCurrentNode(skipTyping);
        });
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

        switch (dialogueFlow.GetCurrentNodeData().NodeType)
        {
            // Todo: 아래처럼 텍스트 출력이 아닌 것들은 전처리를 거칠 수 있습니다
            // case NodeTypes.NodeType.IsQuestInProgress:
            //     // Do Something
            //     CheckDialoguePlaying(action);
            //     break;
            default:
                dialogueFlow.ChangeCurrentNodeData(nodeLinks[0].TargetNodeGuid);
                action?.Invoke(false);
                break;
        }

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
        }
    }

    public void Exit()
    {
        player.Interaction.Enabled = true;
        DialogueManager.Instance.StopDialogue();
    }
}

using DS;
using DS.Core;
using DS.Runtime;
using UnityEngine;

public class PlayerInteractionState : IState
{
    private PlayerController player;
    private StateMachine stateMachine;

    private DialogueHandler dialogueHandler;
    private DialogueContainer dialogueContainer;
    private DialogueFlow dialogueFlow;

    public PlayerInteractionState(PlayerController player, StateMachine stateMachine, DialogueHandler dialogueHandler, DialogueContainer dialogueContainer)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.dialogueHandler = dialogueHandler;
        this.dialogueContainer = dialogueContainer;
        dialogueFlow = new DialogueFlow(dialogueContainer);
    }

    public void Enter()
    {
        var nodeData = dialogueFlow.GetCurrentNodeData();
        switch (nodeData.NodeType)
        {
            case NodeTypes.NodeType.NoChoice:
                var detailNodeData = nodeData as NoChoiceNodeData;
                DialogueManager.Instance.GetHandler(detailNodeData.TargetObjectID).PlayDialogue(detailNodeData.DialogueText);
                break;
        }
    }

    public void Execute()
    {
        if (!player.InteractionInput)
            return;
        player.InteractionInput = false;

        bool skipTyping = false;

        if (DialogueManager.Instance.CheckDialogueEnd())
        {
            // 다이얼로그 출력 완료 후 다음 텍스트 출력
            var nodeLinks = dialogueFlow.GetCurrentNodeLinks();
            
            if (nodeLinks.Count == 0)
            {
                stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
                return;
            }
            
            dialogueFlow.ChangeCurrentNodeData(nodeLinks[0].TargetNodeGuid);
        }
        else
        {
            // 다이얼로그 빠르게 출력
            skipTyping = true;
        }
        
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
        DialogueManager.Instance.StopDialogue();
    }
}

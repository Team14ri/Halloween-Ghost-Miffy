using System;
using System.Linq;
using DS;
using DS.Core;
using DS.Runtime;
using Quest;
using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    [SerializeField] private DialogueContainer dialogueContainer;
    
    private DialogueHandler dialogueHandler;
    private DialogueFlow dialogueFlow;

    private void Start()
    {
        dialogueHandler = GetComponent<DialogueHandler>();
    }

    public void Enter()
    {
        dialogueFlow = new DialogueFlow(dialogueContainer);
        
        PlayerController.Instance.Interaction.Enabled = false;
        UIManager.Instance.PlayerInput.enabled = false;
        
        PlayCurrentNode();
    }

    private void Update()
    {
        if (!PlayerController.Instance.InteractionInput)
            return;
        
        PlayerController.Instance.InteractionInput = false;

        CheckDialoguePlaying(PlayCurrentNode);
    }
    
    public void SelectChoice(string guid)
    {
        PlayerController.Instance.StopInteractionInput = false;
        
        var nodeLinks = dialogueFlow.GetCurrentNodeLinks();
        
        if (nodeLinks.Count == 0)
        {
            Exit();
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
            Exit();
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
                
                dialogueHandler.SetName(noChoiceNodeData.TargetObjectID);
                dialogueHandler.PlayDialogue(noChoiceNodeData.DialogueText, skipTyping);

                if (skipTyping == false && noChoiceNodeData.SkipDelay >= 0f)
                {
                    PlayerController.Instance.StopInteractionInputUntil(noChoiceNodeData.SkipDelay);
                }
                break;
            case NodeTypes.NodeType.MultiChoice:
                PlayerController.Instance.StopInteractionInput = true;
                
                var multiChoiceNodeData = nodeData as MultiChoiceNodeData;
                dialogueHandler.SetName(multiChoiceNodeData.TargetObjectID);
                dialogueHandler.PlayDialogue(multiChoiceNodeData.DialogueText, skipTyping);
                
                var nodeLinks = dialogueFlow.GetCurrentNodeLinks();
                
                MultiDialogueHandler.Instance.Init(nodeLinks, SelectChoice);
                break;
            case NodeTypes.NodeType.StartQuest:
                var startQuestNodeData = dialogueFlow.GetCurrentNodeData() as StartQuestNodeData;
                QuestManager.Instance.Accept(startQuestNodeData.QuestType, startQuestNodeData.QuestID);

                CheckDialoguePlaying(PlayCurrentNode);
                break;
            case NodeTypes.NodeType.AddItem:
                var addItemNodeData = dialogueFlow.GetCurrentNodeData() as AddItemNodeData;
                VariableManager.Instance.AddItems(addItemNodeData.ItemID, addItemNodeData.ItemCount);
                
                CheckDialoguePlaying(PlayCurrentNode);
                break;
            case NodeTypes.NodeType.Condition:
                var conditionNodeData = dialogueFlow.GetCurrentNodeData() as ConditionNodeData;
                var conditionLinks = dialogueFlow.GetCurrentNodeLinks();

                var conditionResult = VariableManager.Instance.GetVariableValue(conditionNodeData.ItemID) >= conditionNodeData.EqualOrMany;

                SelectChoice(conditionLinks.FirstOrDefault(link => link.PortName == (conditionResult ? "True" : "False")).TargetNodeGuid);
                
                break;
        }
    }

    private void Exit()
    {
        PlayerController.Instance.Interaction.SetInteractionEnable();
        UIManager.Instance.PlayerInput.enabled = true;
        DialogueManager.Instance.StopDialogue();
    }
}

using System.Collections;
using DS;
using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Custom")]
public class DynamicChoiceNode : Unit
{
    [DoNotSerialize]
    public ControlInput FlowInput { get; private set; }

    [DoNotSerialize]
    public ControlOutput FlowOutput { get; private set; }
    
    [DoNotSerialize]
    public ValueInput TargetInput { get; private set; }
    
    [DoNotSerialize]
    public ValueInput MessageInput { get; private set; }

    protected override void Definition()
    {
        FlowInput = ControlInput(nameof(FlowInput), ExecuteNode);
        FlowOutput = ControlOutput(nameof(FlowOutput));
        
        TargetInput = ValueInput<GameObject>(nameof(TargetInput));
        MessageInput = ValueInput<string>(nameof(MessageInput));

        Succession(FlowInput, FlowOutput);
        Requirement(TargetInput, FlowInput);
        Requirement(MessageInput, FlowInput);
    }

    private ControlOutput ExecuteNode(Flow flow)
    {
        GameObject target = flow.GetValue<GameObject>(TargetInput);
        string message = flow.GetValue<string>(MessageInput);
        
        HandleMessage(target, message);

        return FlowOutput;
    }
    
    private void HandleMessage(GameObject target, string message)
    {
        target.GetComponent<DialogueHandler>().PlayDialogue(message.Replace("\\n", "\n"));
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Custom")]
public class DynamicChoiceNode : Unit 
{
    [DoNotSerialize]
    public ValueInput stringListInput { get; private set; }

    [DoNotSerialize]
    public ControlInput flowInput { get; private set; }

    [DoNotSerialize]
    public ControlOutput flowOutput { get; private set; }

    [DoNotSerialize]
    public ValueOutput selectOutput { get; private set; }

    protected override void Definition()
    {
        stringListInput = ValueInput(nameof(stringListInput), new List<string>());
        flowInput = ControlInput(nameof(flowInput), ExecuteNode);
        flowOutput = ControlOutput(nameof(flowOutput));
        selectOutput = ValueOutput<int>(nameof(selectOutput)); // int형 출력 포트 정의

        Succession(flowInput, flowOutput);
        Requirement(stringListInput, flowInput);
    }

    ControlOutput ExecuteNode(Flow flow)
    {
        List<string> inputList = flow.GetValue<List<string>>(stringListInput);
        HandleList(inputList);

        flow.SetValue(selectOutput, ComputeIntValue()); // selectOutput에 값을 설정
        flow.Invoke(flowOutput);
        
        return null; 
    }

    void HandleList(List<string> inputList)
    {
        foreach (var str in inputList)
        {
            Debug.Log(str);
        }
    }

    int ComputeIntValue()
    {
        // 예제: int 값을 계산하는 로직. 실제 로직에 따라 변경하십시오.
        return 42;
    }
}
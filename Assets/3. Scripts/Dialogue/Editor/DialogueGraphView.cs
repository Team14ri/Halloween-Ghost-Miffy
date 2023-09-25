using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DS.Editor
{
    public class DialogueGraphView : GraphView
    {
        private const string StyleSheetPath = "UI/DialogueGraph";
        private const string NodeStyleSheetPath = "UI/DialogueNode";
        
        public readonly Vector2 DefaultNodeSize = new Vector2(150, 200);

        public DialogueGraphView()
        {
            LoadStyleSheet(StyleSheetPath);
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            AddManipulators();
            SetupBackground();
            AddElement(GenerateEntryPointNode());
        }

        private void LoadStyleSheet(string path)
        {
            styleSheets.Add(Resources.Load<StyleSheet>(path));
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(CreateNodeContextualMenu());
        }

        private void SetupBackground()
        {
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
        }

        private IManipulator CreateNodeContextualMenu()
        {
            return new ContextualMenuManipulator(menuEvent =>
            {
                // 현재 GraphView의 변형(줌 레벨과 스크롤 오프셋)을 가져옵니다.
                Matrix4x4 transformationMatrix = contentViewContainer.worldTransform;
                Vector2 mousePosition = menuEvent.mousePosition;
                Vector2 localMousePosition = transformationMatrix.inverse.MultiplyPoint3x4(mousePosition);
                menuEvent.menu.AppendAction("Add Node", _ => CreateNode("Dialogue Node", localMousePosition));
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(port => startPort != port && startPort.node != port.node).ToList();
        }

        private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }

        private DialogueNode GenerateEntryPointNode()
        {
            var node = new DialogueNode
            {
                title = "START",
                DialogueText = "ENTRYPOINT",
                GUID = Guid.NewGuid().ToString(),
                EntryPoint = true
            };

            var generatedPort = GeneratePort(node, Direction.Output);
            generatedPort.portName = "Next";
            node.outputContainer.Add(generatedPort);

            node.capabilities &= ~Capabilities.Movable;
            node.capabilities &= ~Capabilities.Deletable;

            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(100, 200, 100, 150));
            return node;
        }

        public void CreateNode(string nodeName, Vector2 position)
        {
            AddElement(CreateDialogueNode(nodeName, position));
        }

        public DialogueNode CreateDialogueNode(string nodeName, Vector2 position)
        {
            var dialogueNode = new DialogueNode
            {
                title = nodeName,
                DialogueText = nodeName,
                GUID = Guid.NewGuid().ToString()
            };

            var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            dialogueNode.inputContainer.Add(inputPort);

            LoadStyleSheet(NodeStyleSheetPath);
            AddNewChoiceButton(dialogueNode);
            AddMainTextField(dialogueNode);

            dialogueNode.RefreshExpandedState();
            dialogueNode.RefreshPorts();
            dialogueNode.SetPosition(new Rect(position, DefaultNodeSize));

            return dialogueNode;
        }

        private void AddNewChoiceButton(DialogueNode dialogueNode)
        {
            var button = new Button(() => { AddChoicePort(dialogueNode); }) { text = "New Choice" };
            dialogueNode.titleContainer.Add(button);
        }

        private void AddMainTextField(DialogueNode dialogueNode)
        {
            var textField = new TextField(string.Empty);
            textField.RegisterValueChangedCallback(evt =>
            {
                dialogueNode.DialogueText = evt.newValue;
                dialogueNode.title = evt.newValue;
            });
            textField.SetValueWithoutNotify(dialogueNode.title);
            dialogueNode.mainContainer.Add(textField);
        }

        public void AddChoicePort(DialogueNode dialogueNode, string overriddenPortName = "")
        {
            var generatedPort = GeneratePort(dialogueNode, Direction.Output);
            RemoveOldLabel(generatedPort);

            var choicePortName = GenerateChoicePortName(dialogueNode, overriddenPortName);
            ConfigureTextFieldForPort(generatedPort, choicePortName);
            AddDeleteButtonToPort(dialogueNode, generatedPort);

            generatedPort.portName = choicePortName;
            dialogueNode.outputContainer.Add(generatedPort);
            dialogueNode.RefreshExpandedState();
            dialogueNode.RefreshPorts();
        }

        private static void RemoveOldLabel(Port generatedPort)
        {
            var oldLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(oldLabel);
        }

        private string GenerateChoicePortName(DialogueNode dialogueNode, string overriddenPortName)
        {
            var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
            return string.IsNullOrEmpty(overriddenPortName)
                ? $"Choice {outputPortCount + 1}"
                : overriddenPortName;
        }

        private static void ConfigureTextFieldForPort(Port generatedPort, string portName)
        {
            var textField = new TextField { name = string.Empty, value = portName };
            textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
            generatedPort.contentContainer.Add(new Label("  "));
            generatedPort.contentContainer.Add(textField);
        }

        private void AddDeleteButtonToPort(DialogueNode dialogueNode, Port generatedPort)
        {
            var deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort)) { text = "X" };
            generatedPort.contentContainer.Add(deleteButton);
        }

        private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
        {
            var targetEdge = edges.ToList().FirstOrDefault(x =>
                x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

            if (targetEdge != null)
            {
                targetEdge.input.Disconnect(targetEdge);
                RemoveElement(targetEdge);
            }

            dialogueNode.outputContainer.Remove(generatedPort);
            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
        }
    }
}

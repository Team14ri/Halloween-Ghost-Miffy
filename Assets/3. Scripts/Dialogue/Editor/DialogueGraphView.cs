using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using System.Linq;

namespace DS.Editor
{
    public class DialogueGraphView : GraphView
    {
        private const string StyleSheetPath = "UI/DialogueGraph";

        public readonly Vector2 DefaultNodeSize = new Vector2(150, 200);
        
        public string SaveDirectory;

        public DialogueGraphView()
        {
            LoadStyleSheet(StyleSheetPath);
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            SetupBackground();
            AddManipulators();
            AddElement(CreateEntryPointNode("Start"));
        }

        private void LoadStyleSheet(string path)
        {
            styleSheets.Add(Resources.Load<StyleSheet>(path));
        }
        
        private void SetupBackground()
        {
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(CreateNoChoiceNodeContextualMenu());
            this.AddManipulator(CreateMultiChoiceNodeContextualMenu());
            this.AddManipulator(CreateStartQuestNodeContextualMenu());
        }

        private IManipulator CreateNoChoiceNodeContextualMenu()
        {
            return new ContextualMenuManipulator(menuEvent =>
            {
                // 현재 GraphView의 변형(줌 레벨과 스크롤 오프셋)을 가져옵니다.
                Matrix4x4 transformationMatrix = contentViewContainer.worldTransform;
                Vector2 mousePosition = menuEvent.mousePosition;
                Vector2 localMousePosition = transformationMatrix.inverse.MultiplyPoint3x4(mousePosition);
                menuEvent.menu.AppendAction("Add NoChoice Node", _ => CreateNode(NodeTypes.NodeType.NoChoice, "NoChoice Node", localMousePosition));
            });
        }

        private IManipulator CreateMultiChoiceNodeContextualMenu()
        {
            return new ContextualMenuManipulator(menuEvent =>
            {
                // 현재 GraphView의 변형(줌 레벨과 스크롤 오프셋)을 가져옵니다.
                Matrix4x4 transformationMatrix = contentViewContainer.worldTransform;
                Vector2 mousePosition = menuEvent.mousePosition;
                Vector2 localMousePosition = transformationMatrix.inverse.MultiplyPoint3x4(mousePosition);
                menuEvent.menu.AppendAction("Add MultiChoice Node", _ => CreateNode(NodeTypes.NodeType.MultiChoice, "MultiChoice Node", localMousePosition));
            });
        }

        private IManipulator CreateStartQuestNodeContextualMenu()
        {
            return new ContextualMenuManipulator(menuEvent =>
            {
                // 현재 GraphView의 변형(줌 레벨과 스크롤 오프셋)을 가져옵니다.
                Matrix4x4 transformationMatrix = contentViewContainer.worldTransform;
                Vector2 mousePosition = menuEvent.mousePosition;
                Vector2 localMousePosition = transformationMatrix.inverse.MultiplyPoint3x4(mousePosition);
                menuEvent.menu.AppendAction("Add StartQuest Node", _ => CreateNode(NodeTypes.NodeType.StartQuest, "StartQuest Node", localMousePosition));
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(port => startPort != port && startPort.node != port.node).ToList();
        }

        private void CreateNode(NodeTypes.NodeType type, string nodeName, Vector2 position)
        {
            switch (type)
            {
                case NodeTypes.NodeType.NoChoice:
                    AddElement(CreateNoChoiceNode(nodeName, position));
                    break;
                case NodeTypes.NodeType.MultiChoice:
                    AddElement(CreateMultiChoiceNode(nodeName, position));
                    break;
                case NodeTypes.NodeType.StartQuest:
                    AddElement(CreateStartQuestNode(nodeName, position));
                    break;
            }
        }
        
        private DialogueNode CreateEntryPointNode(string nodeName)
        {
            var node = new EntryPointNode(nodeName);
            node.Build();

            return node;
        }
        
        public DialogueNode CreateNoChoiceNode(string nodeName, Vector2 position)
        {
            var node = new NoChoiceNode(this, nodeName);
            node.Build(position);

            return node;
        }

        public DialogueNode CreateMultiChoiceNode(string nodeName, Vector2 position)
        {
            var node = new MultiChoiceNode(this, nodeName);
            node.Build(position);

            return node;
        }
        
        public DialogueNode CreateStartQuestNode(string nodeName, Vector2 position)
        {
            var node = new StartQuestNode(this, nodeName);
            node.Build(position);

            return node;
        }
    }
}

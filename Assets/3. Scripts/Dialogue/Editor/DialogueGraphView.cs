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

        public DialogueGraphView()
        {
            LoadStyleSheet(StyleSheetPath);
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            AddManipulators();
            SetupBackground();
            AddElement(CreateEntryPointNode("Start"));
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
                menuEvent.menu.AppendAction("Add MultiChoice Node", _ => CreateNode("MultiChoice Node", localMousePosition));
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(port => startPort != port && startPort.node != port.node).ToList();
        }

        public void CreateNode(string nodeName, Vector2 position)
        {
            AddElement(CreateMultiChoiceNode(nodeName, position));
        }
        
        private DialogueNode CreateEntryPointNode(string nodeName)
        {
            var entryPointNode = new EntryPointNode(nodeName);
            entryPointNode.Build();

            return entryPointNode;
        }

        public DialogueNode CreateMultiChoiceNode(string nodeName, Vector2 position)
        {
            var multiChoiceNode = new MultiChoiceNode(this, nodeName);
            multiChoiceNode.Build(position);

            return multiChoiceNode;
        }
    }
}

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Editor
{
    public class DialogueNode : Node
    {
        public string GUID;
        public string NodeTitle;
        public bool EntryPoint = false;
        
        private const string NodeStyleSheetPath = "UI/DialogueNode";

        protected void LoadStyleSheet()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(NodeStyleSheetPath));
        }

        protected static void RemoveLabel(VisualElement content, string labelName)
        {
            var oldLabel = content.Q<Label>(labelName);
            if (oldLabel != null)
            {
                content.Remove(oldLabel);
            }
        }

        protected Port GeneratePort(Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }
    }
}
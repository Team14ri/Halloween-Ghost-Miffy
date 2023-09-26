using System;
using UnityEngine;

namespace DS.Runtime
{
    [Serializable]
    public class DialogueNodeData
    {
        public string GUID;
        public string NodeTitle;
        public NodeTypes NodeType;
        public Vector2 Position;
        
        public enum NodeTypes
        {
            NoChoice,
            MultiChoice
        }
    }
}

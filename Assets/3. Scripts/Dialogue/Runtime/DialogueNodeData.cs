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
        
        public bool IsEqual(DialogueNodeData other)
        {
            if (GUID != other.GUID)
                return false;
            
            if (NodeTitle != other.NodeTitle)
                return false;
            
            if (NodeType != other.NodeType)
                return false;
            
            if (!(Mathf.Approximately(Position.x, other.Position.x) 
                  && Mathf.Approximately(Position.y, other.Position.y))) 
                return false;
            
            return true;
        }
    }
}

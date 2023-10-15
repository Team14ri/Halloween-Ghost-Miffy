using System;
using UnityEngine;

namespace DS.Runtime
{
    [Serializable]
    public class DialogueNodeData
    {
        public string GUID;
        public string NodeTitle;
        public NodeTypes.NodeType NodeType;
        public Vector2 Position;

        public virtual bool IsEqual(DialogueNodeData other)
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
    
    [Serializable]
    public class NoChoiceNodeData : DialogueNodeData
    {
        public string TargetObjectID;
        public string DialogueText;
        public float SkipDelay;

        public override bool IsEqual(DialogueNodeData other)
        {
            if (!base.IsEqual(other))
                return false;

            var otherNode = other as NoChoiceNodeData;
            
            if (TargetObjectID != otherNode.TargetObjectID)
                return false;
            
            if (DialogueText != otherNode.DialogueText)
                return false;
            
            if (!Mathf.Approximately(SkipDelay, otherNode.SkipDelay)) 
                return false;
            
            return true;
        }
    }
    
    [Serializable]
    public class MultiChoiceNodeData : DialogueNodeData
    {
        public string TargetObjectID;
        public string DialogueText;
        
        public override bool IsEqual(DialogueNodeData other)
        {
            if (!base.IsEqual(other))
                return false;

            var otherNode = other as MultiChoiceNodeData;
            
            if (TargetObjectID != otherNode.TargetObjectID)
                return false;
            
            if (DialogueText != otherNode.DialogueText)
                return false;
            
            return true;
        }
    }
    
    [Serializable]
    public class StartQuestNodeData : DialogueNodeData
    {
        public string QuestType;
        public string QuestID;
        
        public override bool IsEqual(DialogueNodeData other)
        {
            if (!base.IsEqual(other))
                return false;

            var otherNode = other as StartQuestNodeData;
            
            if (QuestType != otherNode.QuestType)
                return false;
            
            if (QuestID != otherNode.QuestID)
                return false;

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Runtime
{
    [Serializable]
    public class DialogueContainer : ScriptableObject
    {
        public List<NodeLinkData> NodeLinks = new();
        public List<DialogueNodeData> DialogueNodeData = new();
        
        public bool IsEqual(DialogueContainer other)
        {
            // NodeLinks 리스트 비교
            if (NodeLinks.Count != other.NodeLinks.Count)
                return false;

            for (int i = 0; i < NodeLinks.Count; i++)
            {
                if (!NodeLinks[i].IsEqual(other.NodeLinks[i])) 
                    return false;
            }

            // DialogueNodeData 리스트 비교
            if (DialogueNodeData.Count != other.DialogueNodeData.Count)
                return false;

            for (int i = 0; i < DialogueNodeData.Count; i++)
            {
                if (!DialogueNodeData[i].IsEqual(other.DialogueNodeData[i]))
                    return false;
            }

            return true;
        }
    }
}

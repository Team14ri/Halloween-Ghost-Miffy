using System.Collections.Generic;
using System.Linq;
using DS.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Editor
{
    public class GraphSaveUtility
    {
        private DialogueGraphView _targetGraphView;
        private DialogueContainer _containerCache;
        
        private IEnumerable<Edge> Edges => _targetGraphView.edges.ToList();
        private IEnumerable<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>();

        private GraphSaveUtility(DialogueGraphView targetGraphView)
        {
            _targetGraphView = targetGraphView;
        }

        public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView) 
            => new GraphSaveUtility(targetGraphView);

        public void SaveGraph(string fileName)
        {
            if (!Edges.Any()) return;

            var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
            SaveNodes(dialogueContainer);
            SaveLinks(dialogueContainer);

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{fileName}.asset");
            AssetDatabase.SaveAssets();
        }

        private void SaveLinks(DialogueContainer container)
        {
            var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

            foreach (var port in connectedPorts)
            {
                var outputNode = port.output.node as DialogueNode;
                var inputNode = port.input.node as DialogueNode;

                container.NodeLinks.Add(new NodeLinkData
                {
                    BaseNodeGuid = outputNode.GUID,
                    PortName = port.output.portName,
                    TargetNodeGuid = inputNode.GUID
                });
            }
        }

        private void SaveNodes(DialogueContainer container)
        {
            foreach (var dialogueNode in Nodes.Where(node => !node.EntryPoint))
            {
                container.DialogueNodeData.Add(new DialogueNodeData
                {
                    GUID = dialogueNode.GUID,
                    DialogueText = dialogueNode.DialogueText,
                    Position = dialogueNode.GetPosition().position
                });
            }
        }

        public void LoadGraph(string fileName)
        {
            _containerCache = Resources.Load<DialogueContainer>(fileName);
            if (_containerCache == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exist!", "OK");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
        }

        private void ClearGraph()
        {
            var entryNode = Nodes.FirstOrDefault(x => x.EntryPoint);
            entryNode.GUID = _containerCache.NodeLinks[0].BaseNodeGuid;

            foreach (var node in Nodes)
            {
                if (node.EntryPoint) continue;

                var connectedEdges = Edges.Where(x => x.input.node == node).ToList();
                foreach (var edge in connectedEdges)
                    _targetGraphView.RemoveElement(edge);

                _targetGraphView.RemoveElement(node);
            }
        }

        private void CreateNodes()
        {
            foreach (var nodeData in _containerCache.DialogueNodeData)
            {
                var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText, nodeData.Position);
                tempNode.GUID = nodeData.GUID;
                _targetGraphView.AddElement(tempNode);

                var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.GUID).ToList();
                nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
            }
        }

        private void ConnectNodes()
        {
            foreach (var node in Nodes)
            {
                var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == node.GUID).ToList();
                foreach (var connection in connections)
                {
                    var targetNodeGuid = connection.TargetNodeGuid;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                    
                    var output = node.outputContainer.Q<Port>();
                    var input = (Port)targetNode.inputContainer[0];

                    LinkNodes(output, input);

                    var position = _containerCache.DialogueNodeData.First(x => x.GUID == targetNodeGuid).Position;
                    targetNode.SetPosition(new Rect(position, _targetGraphView.DefaultNodeSize));
                }
            }
        }

        private void LinkNodes(Port output, Port input)
        {
            var edge = new Edge
            {
                output = output,
                input = input
            };
            edge.input.Connect(edge);
            edge.output.Connect(edge);

            _targetGraphView.Add(edge);
        }
    }
}

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

        private List<Edge> Edges => _targetGraphView.edges.ToList();
        private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

        public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
        {
            return new GraphSaveUtility
            {
                _targetGraphView = targetGraphView
            };
        }

        public void SaveGraph(string fileName)
        {
            if (!Edges.Any())   // Edges(연결)이 없다면 저장하지 않음
                return;

            var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

            SaveNodes(dialogueContainer);
            SaveLinks(dialogueContainer);

            // 리소스 폴더가 없다면 자동으로 폴더 생성
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
                    NodeTitle = dialogueNode.NodeTitle,
                    NodeType = (DialogueNodeData.NodeTypes)dialogueNode.NodeType,
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
            Nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;

            foreach (var node in Nodes)
            {
                if (node.EntryPoint)
                    continue;
                
                Edges.Where(x => x.input.node == node).ToList()
                    .ForEach(edge => _targetGraphView.RemoveElement(edge));
                
                _targetGraphView.RemoveElement(node);
            }
        }

        private void CreateNodes()
        {
            foreach (var nodeData in _containerCache.DialogueNodeData)
            {
                switch (nodeData.NodeType)
                {
                    case DialogueNodeData.NodeTypes.NoChoice:
                        CreateNoChoiceNode(nodeData);
                        break;
                    case DialogueNodeData.NodeTypes.MultiChoice:
                        CreateMultiChoiceNode(nodeData);
                        break;
                }
            }
        }

        private void CreateNoChoiceNode(DialogueNodeData nodeData)
        {
            var tempNode = _targetGraphView.CreateNoChoiceNode(nodeData.NodeTitle, nodeData.Position) as NoChoiceNode;
            tempNode.GUID = nodeData.GUID;
            _targetGraphView.AddElement(tempNode);
        }

        private void CreateMultiChoiceNode(DialogueNodeData nodeData)
        {
            var tempNode = _targetGraphView.CreateMultiChoiceNode(nodeData.NodeTitle, nodeData.Position) as MultiChoiceNode;
            tempNode.GUID = nodeData.GUID;
            _targetGraphView.AddElement(tempNode);

            var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.GUID).ToList();
            nodePorts.ForEach(x => tempNode.AddChoicePort(x.PortName));
        }

        private void ConnectNodes()
        {
            foreach (var node in Nodes)
            {
                var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == node.GUID).ToList();
                for (int j = 0; j < connections.Count; j++)
                {
                    var targetNodeGuid = connections[j].TargetNodeGuid;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                    LinkNodes(node.outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                    
                    targetNode.SetPosition(new Rect(
                        _containerCache.DialogueNodeData.First(x => x.GUID == targetNodeGuid).Position,
                        _targetGraphView.DefaultNodeSize
                    ));
                }
            }
        }

        private void LinkNodes(Port output, Port input)
        {
            var tempEdge = new Edge
            {
                output = output,
                input = input
            };
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            
            _targetGraphView.Add(tempEdge);
        }
    }
}

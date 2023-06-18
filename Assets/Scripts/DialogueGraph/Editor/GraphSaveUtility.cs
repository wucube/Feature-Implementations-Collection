using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private DialogueGraphView targetGraphView;
    private DialogueContainer containerCache;

    private List<Edge> edges => targetGraphView.edges.ToList();
    private List<DialogueNode> nodes => targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();


    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility { targetGraphView = targetGraphView };
    }
    

    public void SaveGraph(string fileName)
    {
        if (!edges.Any()) return; // if there are no edges(no connections) then return

        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        var connectedPorts = edges.Where(x => x.input.node != null).ToArray();
        for(int i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as DialogueNode;
            var inputNode = connectedPorts[i].input.node as DialogueNode;

            dialogueContainer.nodeLinks.Add(new NodeLinkData
            {
                baseNodeGuid = outputNode.guid,
                portName = connectedPorts[i].output.portName,
                targetNodeGuid = inputNode.guid
            }); ;
        }

        foreach(var dialogueNode in nodes.Where(node => !node.entryPoint))
        {
            dialogueContainer.dialogueNodeData.Add(new DialogueNodeData
            {
                guid = dialogueNode.guid,
                dialogueText = dialogueNode.dialogueText,
                position = dialogueNode.GetPosition().position
            });
        }

        // Auto creates resources folder if it doesn't exist
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {
        containerCache = Resources.Load<DialogueContainer>(fileName);
        if(containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exists!", "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ClearGraph()
    {
        // Set entry points guid back from the save. Discard existing guid.
        nodes.Find(x => x.entryPoint).guid = containerCache.nodeLinks[0].baseNodeGuid;
        foreach(var node in nodes)
        {
            if (node.entryPoint) continue;

            //Remove edges that connected to this node
            edges.Where(x => x.input.node == node).ToList()//List<Edge>
                .ForEach(edge => targetGraphView.RemoveElement(edge));

            // Then remove the nodes
            targetGraphView.RemoveElement(node);
        }
    }

    private void CreateNodes()
    {
        foreach(var nodeData in containerCache.dialogueNodeData)
        {
            var tempNode = targetGraphView.CreateDialogueNode(nodeData.dialogueText);
            tempNode.guid = nodeData.guid;
            targetGraphView.AddElement(tempNode);

            var nodePorts = containerCache.nodeLinks.Where(x => x.baseNodeGuid == nodeData.guid).ToList();
            nodePorts.ForEach(x => targetGraphView.AddChoicePort(tempNode, x.portName));
        }
    }

    private void ConnectNodes()
    {
        for(int i = 0; i < nodes.Count; i++)
        {
            var connections = containerCache.nodeLinks.Where(x => x.baseNodeGuid == nodes[i].guid).ToList();
            for (int j =0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].targetNodeGuid;
                var targetNode = nodes.First(x => x.guid == targetNodeGuid);

                LinkNodes(nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                targetNode.SetPosition(new Rect(
                    containerCache.dialogueNodeData.First(x => x.guid == targetNodeGuid).position,
                    targetGraphView.defaultNodeSize
                 ));
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge { output = output, input = input };

        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        targetGraphView.Add(tempEdge);
    }
}

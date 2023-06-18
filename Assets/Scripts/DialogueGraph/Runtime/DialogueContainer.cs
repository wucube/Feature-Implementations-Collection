using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using UnityEngine;

[Serializable]
public class DialogueContainer:ScriptableObject
{
    public List<NodeLinkData> nodeLinks = new List<NodeLinkData>();
    public List<DialogueNodeData> dialogueNodeData = new List<DialogueNodeData>();
}

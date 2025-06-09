using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogoNode
{
    public string id;
    public string text;
    public string[] options;
    public string[] nextNodeIds;
}

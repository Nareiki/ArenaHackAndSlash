using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "BehaviourDataEnemy", menuName = "CreateNodeSystem")]
public class TestScript : ScriptableObject
{
    [SerializeReference]
    private INode RootNode;
}

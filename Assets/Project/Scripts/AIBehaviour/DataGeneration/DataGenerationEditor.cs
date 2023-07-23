using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DataGenerationEditor : EditorWindow
{
    private BasicTree _root;
    private ScriptableObject _nodeDataContainer;

    private Dictionary<string, object> dataFromNodes;

    [MenuItem("Window/NodeDataExtractor")]
    public static void ShowWindow()
    {
        GetWindow<DataGenerationEditor>("Node data Extractor");
    }

    private void OnGUI()
    {
        dataFromNodes = new Dictionary<string, object>();


        EditorGUILayout.LabelField("Node date Extractor", EditorStyles.boldLabel);

        _root = (BasicTree)EditorGUILayout.ObjectField("Root Node", _root, typeof(BasicTree), true);
        _nodeDataContainer = (ScriptableObject)EditorGUILayout.ObjectField("Node Data Container", _nodeDataContainer, typeof(ScriptableObject), false);

        if (GUILayout.Button("Extract Data"))
        {
            if (_root != null && _nodeDataContainer != null)
            {
                ExtractNodeData(_root, _nodeDataContainer);
                Debug.Log(dataFromNodes);
                Debug.Log("Node data extracted and saved into the ScriptableObject");
            }
            else
            {
                Debug.LogError("Please assign both the root and the ScriptableObject before extracting");
            }
        }
    }

    private void ExtractNodeData(BasicTree root, ScriptableObject container)
    {
        // Here, you can extract the data from the 'node' and save it into the 'container'
        // For example, you can create public fields in the ScriptableObject to store the extracted data

        // Example: Let's assume your ScriptableObject has fields like 'nodeNames', 'nodeValues', 'nodePositions'.
        // You can modify this based on the actual data you want to extract.
        Type type = root.GetType();

        MethodInfo info = type.GetMethod("SetupTree", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        BasicNode tree = new BasicNode();
        if (info != null)
        {
            tree = (BasicNode)info.Invoke(root, null);
        }
        Debug.Log(tree);

        // Get Children of Child

        BindingFlags nonPubFields = BindingFlags.Instance | BindingFlags.NonPublic;

        GetAllNodesFromNodeList(tree, nonPubFields, "Children");

        //List<BasicNode> test2 = GetProtectedFieldValueOfTree<List<BasicNode>>(tree, nonPubFields, "Children");

        //foreach (var node in test2)
        //{
        //    List<BasicNode> childrenNodes = GetProtectedFieldValueOfTree<List<BasicNode>>(node, nonPubFields, "Children");
        //    if (childrenNodes.Count > 0)
        //    {
        //        foreach (var childNode in childrenNodes)
        //        {

        //        }
        //    }
        //    Debug.Log(node);
        //}



        //foreach (var node in tree.Children)
        //{

        //}


        //root.Setup

        //ScriptableObject dataContainer = container as ScriptableObject;
        //dataContainer.nodeNames.Add(root.name);
        //dataContainer.nodeValues.Add(node.GetValue());
        //dataContainer.nodePositions.Add(node.transform.position);

        //if (node.children != null)
        //{
        //    foreach (BasicNode childNode in node.children)
        //    {
        //        ExtractNodeData(childNode, container);
        //    }
        //}
    }

    private void GetAllNodesFromNodeList<T>(T node, BindingFlags flags, string field)
    {
        List<T> list = GetProtectedFieldValueOfTree<List<T>>(node, flags, field);

        if (list != null && list.Count > 0)
        {
            foreach (var item in list)
            {
                Debug.Log(item);
                GetAllFieldsOfClass(item);
                GetAllNodesFromNodeList(item, flags, field);
            }

        }
    }

    private T GetProtectedFieldValueOfTree<T>(System.Object baseNode, BindingFlags flags, string fieldName)
    {
        Type typeTree = baseNode.GetType();

        FieldInfo treeNodes = typeTree.GetField(fieldName, flags);
        T test2 = (T)treeNodes.GetValue(baseNode);

        return test2;
    }

    private void GetAllFieldsOfClass<T>(T extractionClass)
    {
        BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        Type typeOfClass = extractionClass.GetType();

        FieldInfo[] allFields = typeOfClass.GetFields(flags);

        foreach (var field in allFields)
        {
            Debug.Log(field.GetValue(extractionClass));
            if (!dataFromNodes.ContainsKey(field.Name))
            {
                dataFromNodes.Add(field.Name, field.GetValue(extractionClass));
            }
        }
    }
}

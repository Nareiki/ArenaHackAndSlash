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

    private Dictionary<string, TypeAndValuePair> dataFromNodes;

    [MenuItem("Window/NodeDataExtractor")]
    public static void ShowWindow()
    {
        GetWindow<DataGenerationEditor>("Node data Extractor");
    }

    private void OnGUI()
    {
        dataFromNodes = new Dictionary<string, TypeAndValuePair>();


        EditorGUILayout.LabelField("Node date Extractor", EditorStyles.boldLabel);

        _root = (BasicTree)EditorGUILayout.ObjectField("Root Node", _root, typeof(BasicTree), true);

        if (GUILayout.Button("Extract Data"))
        {
            if (_root != null)
            {
                ExtractNodeData(_root);
                //CreateScriptableObjectInstance(dataFromNodes);
                ScriptGenerator generator = new ScriptGenerator();
                generator.CreateNewScript(dataFromNodes);
                Debug.Log(dataFromNodes);
                Debug.Log("Node data extracted and saved into the ScriptableObject");
            }
            else
            {
                Debug.LogError("Please assign both the root and the ScriptableObject before extracting");
            }
        }
    }

    private void ExtractNodeData(BasicTree root)
    {
        // Here, you can extract the data from the 'node' and save it into the 'container'
        // For example, you can create public fields in the ScriptableObject to store the extracted data

        // Example: Let's assume your ScriptableObject has fields like 'nodeNames', 'nodeValues', 'nodePositions'.
        // You can modify this based on the actual data you want to extract.
        Type type = root.GetType();

        MethodInfo info = type.GetMethod("SetupTree", BindingFlags.NonPublic | BindingFlags.Instance);
        BasicNode tree = new BasicNode();
        if (info != null)
        {
            tree = (BasicNode)info.Invoke(root, null);
        }
        Debug.Log(tree);

        // Get Children of Child

        BindingFlags nonPubFields = BindingFlags.Instance | BindingFlags.NonPublic;

        GetAllNodesFromParentNode(tree, nonPubFields, "Children");

    }

    private void GetAllNodesFromParentNode<T>(T node, BindingFlags flags, string field)
    {
        List<T> list = GetProtectedFieldValueOfTree<List<T>>(node, flags, field);

        if (list != null && list.Count > 0)
        {
            foreach (var item in list)
            {
                Debug.Log(item);
                GetAllFieldsOfClass(item, flags);
                GetAllNodesFromParentNode(item, flags, field);
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

    private void GetAllFieldsOfClass<T>(T extractionClass, BindingFlags flags)
    {
        Type typeOfClass = extractionClass.GetType();

        FieldInfo[] allFields = typeOfClass.GetFields(flags);

        foreach (var field in allFields)
        {
            Debug.Log(field.GetValue(extractionClass));
            string keyToAdd = field.Name.Substring(1);
            if (!dataFromNodes.ContainsKey(keyToAdd))
            {
                if (field.GetCustomAttribute<NonReflectionValue>() == null)
                {
                    dataFromNodes.Add(keyToAdd, new TypeAndValuePair(field.FieldType, field.GetValue(extractionClass)));
                }
            }
        }
    }
}

public class TypeAndValuePair
{
    public Type type { get; set; }
    public object value { get; set; }

    public TypeAndValuePair(Type type, object value)
    {
        this.type = type;
        this.value = value;
    }
}

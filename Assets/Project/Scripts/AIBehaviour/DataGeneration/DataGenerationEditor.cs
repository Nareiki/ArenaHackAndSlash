using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DataGenerationEditor : EditorWindow
{
    private BasicTree _root;
    private string nameOfFile = "";
    string _scriptFolder;

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

        nameOfFile = EditorGUILayout.TextField("Object Name: ", nameOfFile);

        if (GUILayout.Button("Extract Data"))
        {
            if (_root != null)
            {
                ReadSetupData();
                ExtractNodeData(_root);
                //CreateScriptableObjectInstance(dataFromNodes);
                ScriptGenerator generator = new ScriptGenerator();
                generator.CreateNewScript(dataFromNodes, _scriptFolder, nameOfFile);
                Debug.Log(dataFromNodes);
                Debug.Log("Node data extracted and saved into the ScriptableObject");
            }
            else
            {
                Debug.LogError("Please assign both the root and the ScriptableObject before extracting");
            }
        }
    }

    void ReadSetupData()
    {
        MonoScript ms = MonoScript.FromScriptableObject(this);
        string scriptFilePath = AssetDatabase.GetAssetPath(ms);

        FileInfo fi = new FileInfo(scriptFilePath);

        _scriptFolder = fi.Directory.ToString();
        _scriptFolder = _scriptFolder.Replace(Application.dataPath, "");


        Debug.Log(_scriptFolder);
    }

    private void ExtractNodeData(BasicTree root)
    {

        Type type = root.GetType();

        SetDataBehaviourField(type, root);

        MethodInfo info = type.GetMethod("SetupTree", BindingFlags.NonPublic | BindingFlags.Instance);
        BasicNode tree = new BasicNode();
        if (info != null)
        {
            tree = (BasicNode)info.Invoke(root, null);
        }

        // Get Children of Child

        BindingFlags nonPubFields = BindingFlags.Instance | BindingFlags.NonPublic;

        GetAllNodesFromParentNode(tree, nonPubFields, "Children");

    }

    private void SetDataBehaviourField(Type type, BasicTree root)
    {
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (FieldInfo field in fields)
        {

            Type typetemp = field?.FieldType;
            if (typetemp != null && typetemp == typeof(DataBehaviourBase))
            {
                var test = field?.GetValue(root);
                if (test == null)
                {
                    field.SetValue(root, (DataBehaviourBase)CreateInstance("DataBehaviourBase"));
                }
                return;
            }
        }

        //FieldInfo temp = type.GetField("_guardData", BindingFlags.Instance | BindingFlags.NonPublic);
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

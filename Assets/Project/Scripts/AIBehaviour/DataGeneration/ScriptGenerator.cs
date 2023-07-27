using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Text;
using System;

public class ScriptGenerator
{
    public void CreateNewScript(Dictionary<string, TypeAndValuePair> dict)
    {
        string scriptContent = @"using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = ""New MyData"", menuName = ""BehaviourTreeData/MyData"", order = 1)]
public class MyData : DataBehaviourBase
{ " +
    GenerateDataFromDictionary(dict) +
    @"
}";
        // Setzen Sie hier den Pfad, wo Sie das Skript speichern möchten.
        string scriptPath = "Assets/MyData.cs";

        File.WriteAllText(scriptPath, scriptContent);

        AssetDatabase.Refresh();
    }

    string GenerateDataFromDictionary(Dictionary<string, TypeAndValuePair> dict)
    {
        StringBuilder buildString = new StringBuilder();

        foreach (var item in dict)
        {
            buildString.Append("public ");
            buildString.Append(item.Value.type);
            buildString.Append(" ");
            buildString.Append(item.Key);
            buildString.Append(" = ");
            buildString.Append(item.Value.value);
            buildString.AppendLine(";");
        }


        return buildString.ToString();
    }
}

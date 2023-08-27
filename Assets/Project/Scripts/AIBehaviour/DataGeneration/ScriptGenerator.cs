using System.Collections.Generic;
using UnityEditor;
using System.Text;
using System.IO;
using UnityEngine;

public class ScriptGenerator
{
    public void CreateNewScript(Dictionary<string, TypeAndValuePair> dict, string folderStructure, string nameOfFile)
    {
        nameOfFile = nameOfFile.Replace(" ", "");//.Remove(nameOfFile.IndexOf(" "));

        string scriptContent = @"using UnityEngine;

[CreateAssetMenu(fileName = ""New " + nameOfFile + @""", menuName = ""BehaviourTreeData/" + nameOfFile + @""", order = 1)]
public class " + nameOfFile + @" : DataBehaviourBase
{
" + GenerateDataFromDictionary(dict) +
@"}";

        string scriptPath = "Assets/" + folderStructure + "/" + nameOfFile + ".cs";


        File.WriteAllText(scriptPath, scriptContent);

        AssetDatabase.Refresh();
    }

    string GenerateDataFromDictionary(Dictionary<string, TypeAndValuePair> dict)
    {
        StringBuilder buildString = new StringBuilder();

        foreach (var item in dict)
        {
            string valueType = item.Value.type.FullName;

            buildString.Append("\tpublic ");

            if (valueType.Contains("Single"))
            {
                buildString.Append("float");
            }
            else if (valueType.Contains("UnityEngine"))
            {
                buildString.Append(valueType.Substring(valueType.IndexOf('.') + 1));
            }
            //buildString.Append(valueType);
            buildString.Append(" ");
            buildString.Append(item.Key);

            string namespaceStr = item.Value.type.Namespace;

            if (namespaceStr != null &&namespaceStr.Contains("System"))
            {
                buildString.Append(" = ");

                buildString.Append(item.Value.value);
                if (valueType.Contains("Single"))
                {
                    buildString.Append("f");
                }
            }
            buildString.AppendLine(";");
        }


        return buildString.ToString();
    }
}

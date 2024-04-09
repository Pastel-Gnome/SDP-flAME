using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New  single character Dialogue Data", menuName = "Dialogue System/Dialogue Data for a single character")]
public class SingleDialogueData : ScriptableObject
{
    public int sceneID;
    public string characterName;
    [TextArea(3, 10)] public string[] text;
    //public Sprite characterSprite;

    [SerializeField]
   [HideInInspector] public TextAsset csvFile;

    public void PopulateSentencesFromCSV()
    {
        text = ReadCSV(csvFile);
    }

    public string[] ReadCSV(TextAsset csv)
    {
        List<string> dialogueEntries = new List<string>();

        if (csv != null)
        {
            string[] lines = csv.text.Split('\n');
            foreach (string line in lines)
            {
                string[] fields = line.Split(';');
                if (fields.Length >= 2)
                {
                    string tempText = fields[2];
                    dialogueEntries.Add(tempText);
                }
            }
        }

        return dialogueEntries.ToArray();
    }
}






[CustomEditor(typeof(SingleDialogueData))]
public class SingleDialogueDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SingleDialogueData dialogueData = (SingleDialogueData)target;

        // Display the default fields
        base.OnInspectorGUI();

        GUILayout.Space(10);

        GUILayout.Label("CSV Data");
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        dialogueData.csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", dialogueData.csvFile, typeof(TextAsset), false);

        if (GUILayout.Button("Populate Sentences"))
        {
            dialogueData.text = dialogueData.ReadCSV(dialogueData.csvFile);
        }

        GUILayout.EndHorizontal();
    }
}

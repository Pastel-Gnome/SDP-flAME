using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.ProBuilder.Shapes;
using Unity.VisualScripting;

//[CreateAssetMenu(fileName = "New Multi-Character Dialogue Data", menuName = "Dialogue System/Dialogue Data for Multiple Characters")]
public class MultiDialogueData : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string characterName;
        [TextArea(3, 10)] public string text;
        //public Sprite characterSprite;
    }

    public List<List<DialogueEntry>> dialogueEntries = new List<List<DialogueEntry>>();

    [SerializeField]
    int NumOfScenes;
    public TextAsset csvFile;

    public void Awake()
    {
        dialogueEntries = ReadCSV(csvFile);
    }

    public List<List<DialogueEntry>> ReadCSV(TextAsset csv)
    {
        List<List<DialogueEntry>> entries = new List<List<DialogueEntry>>();
        for (int i = 0; i < NumOfScenes; i++)
        {
            entries.Add(new List<DialogueEntry>());
        }

		if (csv != null)
        {
            string[] lines = csv.text.Split('\n');
            for (int i = 1; i < lines.Length; i++) // Start from index 1 to skip the header
            {
                string line = lines[i];
                string[] fields = line.Split(';');
                int sceneID = int.Parse(fields[0]);

				if (fields.Length >= 2)
                {
                    DialogueEntry entry = new DialogueEntry
                    {
                        characterName = fields[1],
                        text = fields[2],
                    };
					entries[sceneID-1].Add(entry);
				}
            }
        }
		Debug.Log("Dialogue Loaded From " + entries.Count + " Scenes.");
		return entries;
    }
}
/*
[CustomEditor(typeof(MultiDialogueData))]
public class MultiDialogueDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MultiDialogueData dialogueData = (MultiDialogueData)target;

        // Display the default fields
        base.OnInspectorGUI();

        GUILayout.Space(10);

        GUILayout.Label("CSV Data");
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        dialogueData.csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", dialogueData.csvFile, typeof(TextAsset), false);

		if (GUILayout.Button("Populate Sentences"))
        {
			//Undo.RecordObject(target, "Updated Dialogue Data");
            dialogueData.PopulateSentencesFromCSV();
			//AssetDatabase.CreateAsset(dialogueData, "Assets/Scripts/World/Cutscenes/GameDialogue.asset");
			EditorUtility.SetDirty(target);
			//AssetDatabase.SaveAssets();
		}
		GUILayout.EndHorizontal();
    }
} */

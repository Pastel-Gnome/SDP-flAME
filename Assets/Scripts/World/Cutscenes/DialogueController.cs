using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public GameObject DialoguePanel;

    public TMP_Text characterName;
    public TMP_Text dialogueText;

    private Queue<string> sentences;
    private Queue<string> names;
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
    }

    public void StartMultiDialogue(MultiDialogueData dialogueData, int sceneID)
    {
        if (dialogueData == null)
        {
            Debug.LogWarning("Dialogue Data is null. Cannot start dialogue.");
            return;
        }

        if (dialogueData.dialogueEntries.Count == 0)
        {
            Debug.LogWarning("Dialogue Data does not contain any dialogue entries.");
            return;
        }

        sentences.Clear();

        foreach (MultiDialogueData.DialogueEntry entry in dialogueData.dialogueEntries[sceneID-1])
        {
            characterName.text = entry.characterName;
            sentences.Enqueue(entry.text);
            names.Enqueue(entry.characterName);

        }

        DisplayNextSentenceMultiCharacters();
    }

    public void DisplayNextSentenceSingleCharacter()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }
    public void DisplayNextSentenceMultiCharacters()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        string name = names.Dequeue();
        characterName.text = name;
    }

    void EndDialogue()
    {
        DialoguePanel.SetActive(false);
    }
}

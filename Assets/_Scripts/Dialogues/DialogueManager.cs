using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public GameObject Dialogue;
    public Image characterIcon;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;
    public float lineDisplayTime = 5f;  // Time between dialogue lines
    public List<AudioClip> DialogueSfx;
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        Dialogue.SetActive(true);
        

        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            PlayRandomSound();
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait for line display time before showing the next line
        yield return new WaitForSeconds(lineDisplayTime);
        DisplayNextDialogueLine();
    }

    public void PlayRandomSound()
    {
        if (DialogueSfx.Count > 0 && !audioSource.isPlaying)
        {
            AudioClip randomSound = DialogueSfx[Random.Range(0, DialogueSfx.Count)];
            audioSource.PlayOneShot(randomSound);
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        Dialogue.SetActive(false);
        
    }
}

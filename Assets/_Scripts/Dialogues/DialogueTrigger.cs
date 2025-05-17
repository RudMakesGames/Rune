using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

    public class DialogueCharacter
    {
        public string name;
        public Sprite icon;
    }

    [System.Serializable]
    public class DialogueLine
    {
        public DialogueCharacter character;
        [TextArea(3, 10)]
        public string line;
    }

    [System.Serializable]
    public class Dialogue
    {
        public List<DialogueLine> dialogueLines = new List<DialogueLine>();
    }

    public class DialogueTrigger : MonoBehaviour
    {
        public int index;
        public Dialogue dialogue;

        public void TriggerDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogue);

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<Player>(out var player))
            {


                if (player.currentDialogueIndex < index)
                {
                    player.currentDialogueIndex++;
                    StartCoroutine(WakeUp());
                }
            }
        }



        IEnumerator WakeUp()
        {
            yield return new WaitForSeconds(0.75f);
            TriggerDialogue();
        }
    }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeSlowMoDialogueTrigger : MonoBehaviour
{
    
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            StartKnifeKillDialogue();
        }
    }


    public void StartKnifeKillDialogue()
    {
        StartCoroutine(WakeUp());
    }
    IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(0.25f);
        TriggerDialogue();
        ThrowableKnifeManager.Instance.EnemyKilled = false;
    }
}

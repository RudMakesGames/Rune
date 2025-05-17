using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowableKnifeManager : MonoBehaviour
{
    public static ThrowableKnifeManager Instance;
    public Slider slider;
    public int AbilityThreshold = 6;
    public int CurrentAbilityThreshold;
    public bool canThrowKnife;
    public bool EnemyKilled;
    public TimeSlowMoDialogueTrigger timeSlowMoDialogue;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Update()
    {
        slider.value = CurrentAbilityThreshold;

        if (CurrentAbilityThreshold == AbilityThreshold)
        {
            
            canThrowKnife = true;
            Debug.Log("Can Use throwableKnife");

        }
        if(EnemyKilled)
        {
            timeSlowMoDialogue.StartKnifeKillDialogue();
        }
    }
    public void RefreshAbility()
    {
        CurrentAbilityThreshold = 0;
        AbilityThreshold = 3;
        canThrowKnife = false;
        Debug.Log("Ability Reset!");
    }
    public void AddOnePoint()
    {
        if(CurrentAbilityThreshold < AbilityThreshold)
        {
            CurrentAbilityThreshold++;
            Debug.Log("Added 1 point!");
        }
    }

}

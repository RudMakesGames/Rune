using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSkullOnKillableEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject SkullIcon;

    public void ActivateIcon()
    {
        SkullIcon.SetActive(true);
    }
    public void DeactivateIcon() {
        SkullIcon.SetActive(false); }
}

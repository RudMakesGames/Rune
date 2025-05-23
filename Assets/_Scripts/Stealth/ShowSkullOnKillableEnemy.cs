using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSkullOnKillableEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject SkullIcon;

    public Transform AirAssasinationTransform;
    public Transform StealthKillPosition;

    public void ActivateIcon()
    {
        SkullIcon.SetActive(true);
    }
    public void DeactivateIcon() {
        SkullIcon.SetActive(false); }
}

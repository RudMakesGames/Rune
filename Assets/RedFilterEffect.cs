using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class RedFilterEffect : MonoBehaviour
{
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    private void Start()
    {
        if (volume.profile.TryGet(out colorAdjustments))
        {
           
        }
        DisableRedFilter();
    }

    public void EnableRedFilter()
    {
        colorAdjustments.saturation.Override(0f);
        colorAdjustments.colorFilter.Override(Color.red);
    }

    public void DisableRedFilter()
    {
        colorAdjustments.saturation.Override(0f);
        colorAdjustments.colorFilter.Override(Color.white);
    }
}

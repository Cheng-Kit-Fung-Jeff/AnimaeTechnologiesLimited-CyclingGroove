using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CKF_SetGlobalVolume : MonoBehaviour
{
    public Volume globalVolume;

    public void SetBloomIntensity(float value)
    {
        if (globalVolume.profile.TryGet(out Bloom b))
        {
            b.intensity.overrideState = true;
            b.intensity.value = value;
        }
    }

    public void SetBloomTint(Color value)
    {
        if (globalVolume.profile.TryGet(out Bloom b))
        {
            b.tint.overrideState = true;
            b.tint.value = value;
        }
    }

    public void SetVignetteIntensity(float value)
    {
        if (globalVolume.profile.TryGet(out Vignette b))
        {
            b.intensity.overrideState = true;
            b.intensity.value = value;
        }
    }
}

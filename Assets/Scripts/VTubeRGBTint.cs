using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VTubeRGBTint : MonoBehaviour
{
    public Gradient gradient;
    public float RainbowTime;
    public List<string> Tags;

    public VTubeStudio vtube;

    private void FixedUpdate()
    {
        vtube.TintArtMeshTag(gradient.Evaluate((Time.time % RainbowTime) / RainbowTime), Tags);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class ScreenI
{
    static public float Width = 0f;
    static public float Height = 0f;

    static public Vector3 RandomWidth()
    {
        return new Vector3(Width * Random.Range(-1f, 1f), 0, 0);
    }
    static public Vector3 RandomHeight()
    {
        return new Vector3(0, Height * Random.Range(-1f, 1f), 0);
    }
}

public class ScreenInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 corner = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0));
        ScreenI.Width = corner.x;
        ScreenI.Height = corner.y;
    }
}

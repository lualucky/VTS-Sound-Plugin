using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempScreenMessage : MonoBehaviour
{
    public float Time;

    public void Enable(string text)
    {
        GetComponent<Text>().text = text;
        gameObject.SetActive(true);
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(Time);
        gameObject.SetActive(false);
    }
}

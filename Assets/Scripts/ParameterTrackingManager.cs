using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using WebGL;

public class ParameterTrackingManager : MonoBehaviour
{
    public static ParameterTrackingManager Instance;

    public Transform ParameterParent;
    public GameObject ParameterUI;

    List<AudioTrigger> parameters;

    private void Start()
    {
        parameters = new List<AudioTrigger>();
        parameters.Add(ParameterParent.GetComponentInChildren<AudioTrigger>());
    }

    public void AddParameterItem()
    {
        GameObject ui = Instantiate(ParameterUI, ParameterParent);
        parameters.Add(ui.GetComponent<AudioTrigger>());
    }

    public void PlaySound(AudioClip sound)
    {
        GetComponent<AudioSource>().PlayOneShot(sound);
    }

    public void DeleteParameter(AudioTrigger a)
    {
        parameters.Remove(a);
        Destroy(a.gameObject);
    }
}

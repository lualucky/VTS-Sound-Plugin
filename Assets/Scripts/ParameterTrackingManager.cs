using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ParameterTrackingManager : MonoBehaviour
{
    public static ParameterTrackingManager Instance;

    public Transform ParameterParent;
    public GameObject ParameterUI;

    List<AudioTrigger> parameters;

    AudioSource audio;

    private void Start()
    {
        
    }

    public void AddParameterItem()
    {
        GameObject ui = Instantiate(ParameterUI, ParameterParent);
        parameters.Add(ui.GetComponent<AudioTrigger>());
    }

    public void Save()
    {

    }

    public void PlaySound(AudioClip sound)
    {
        audio.PlayOneShot(sound);
    }

    public void DeleteParameter(AudioTrigger a)
    {
        parameters.Remove(a);
        Destroy(a.gameObject);
    }
}

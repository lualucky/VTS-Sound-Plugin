using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class ParameterData
{
    public enum TrackType
    {
        Movement,
        Value
    }

    public enum ParamType
    {
        Input,
        Output
    }

    public string Parameter;
    public ParamType Type;
    public TrackType TrackingType;
    public float Sensitivity;
    public float Cooldown;
    public List<AudioClip> Sounds;
    public bool OnCooldown;
    public float Value;

    ParameterData()
    {
        Sounds = new List<AudioClip>();
    }
}

public class AudioTrigger : MonoBehaviour
{
    public InputField ParameterInput;
    public Dropdown TypeDropdown;
    public Slider SensitivitySlider;
    public Slider CooldownSlider;
    public Transform SoundListParent;
    public List<Dropdown> SoundList;

    public GameObject SoundUI;

    public ParameterData data;

    VTubeStudio vtube;
    ParameterTrackingManager manager;

    void Start()
    {
        vtube = VTubeStudio.Instance;

        vtube.onVTubeStudioConnected += VTubeConnected;
    }

    void VTubeConnected()
    {
        
    }

    void Update()
    {
        if (vtube.isConnected())
        {
            VTubeParameterValueData v = vtube.TrackingParameterValue(data.Parameter);

            // trigger sound if necessary
            if (data.Sounds.Count > 0 && !data.OnCooldown)
            {
                bool trigger = false;
                switch (data.TrackingType) {
                    case ParameterData.TrackType.Movement:
                        if (Mathf.Abs(v.value - data.Value) > data.Sensitivity)
                        {
                            trigger = true;
                        }
                        break;
                    case ParameterData.TrackType.Value:
                        if(Mathf.Abs(v.value - data.Value) <= data.Sensitivity)
                        {
                            trigger = true;
                        }
                        break;
                }
                if(trigger)
                {
                    manager.PlaySound(data.Sounds[UnityEngine.Random.Range(0, data.Sounds.Count)]);
                    StartCoroutine(CooldownTimer());
                }
            }

            data.Value = v.value;
        }
    }

    IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(data.Cooldown);
        data.OnCooldown = false;
    }

    public void SetParameterName(string parameterName)
    {
        data.Parameter = parameterName;
    }

    public void AddSound()
    {
        GameObject SoundLine = Instantiate(SoundUI, SoundListParent);
        SoundList.Add(SoundLine.GetComponent<Dropdown>());
    }

    public void RemoveSound()
    {
        Destroy(SoundList[SoundList.Count - 1].gameObject);
        SoundList.RemoveAt(SoundList.Count - 1);
    }

    public void DeleteParameter()
    {
        manager.DeleteParameter(this);
    }
}

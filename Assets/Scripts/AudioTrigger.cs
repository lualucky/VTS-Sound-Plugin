using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using WebGL;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

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
    [DllImport("__Internal")]
    private static extern void JS_FileSystem_Sync();

    public InputField ParameterInput;
    public Dropdown TypeDropdown;
    public Slider SensitivitySlider;
    public Slider CooldownSlider;
    public Transform SoundListParent;
    public List<Transform> SoundList;
    public List<AudioClip> Sound;

    public GameObject SoundUI;

    public ParameterData data;

    VTubeStudio vtube;
    ParameterTrackingManager manager;

    Transform soundPanel;

    void Start()
    {
        vtube = VTubeStudio.Instance;

        if(vtube)
            vtube.onVTubeStudioConnected += VTubeConnected;
    }

    void VTubeConnected()
    {
        
    }

    void Update()
    {
        if (vtube && vtube.isConnected())
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
        SoundList.Add(SoundLine.transform);
        SoundLine.GetComponentInChildren<Button>().onClick.AddListener(() => SelectFile(SoundLine));
    }

    public void SelectFile(GameObject panel)
    {
        soundPanel = panel.transform;

        NativeFileInteractionforWebGLManager.instance.callActionCallBack = SaveAudio;
        NativeFileInteractionforWebGLManager.instance._OpenSelectFilePopup(".mp3");
    }

    public void SaveAudio(string value, byte[] data)
    {
        string ext = Path.GetExtension(value);
        ext = ext.ToLower();

        if (ext.EndsWith(".mp3"))
        {
            string savePath = Path.Combine(Application.persistentDataPath, Path.GetFileName(value));

            System.IO.File.WriteAllBytes(savePath, data);

            JS_FileSystem_Sync();

            StartCoroutine(LoadAudioClip(savePath));
            soundPanel.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(value);
        }

        NativeFileInteractionforWebGLManager.instance.callActionCallBack = null;
    }

    IEnumerator LoadAudioClip(string path)
    {
        UnityWebRequest loader = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);
        yield return loader.SendWebRequest();
        if (loader.isHttpError)
            Debug.LogError(loader.error);
        else
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(loader);

            Sound.Add(clip);
        }
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

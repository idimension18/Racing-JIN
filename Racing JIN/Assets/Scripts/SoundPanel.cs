using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // Menu
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;
    
    // ----------- OPTIONS ---------------------
    public void OnMasterVolumeChange(float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Max(20f * Mathf.Log10(value), -80f));
    }

    public void OnSoundtrackVolumeChange(float value)
    {
        mixer.SetFloat("SoundtrackVolume", Mathf.Max(20f * Mathf.Log10(value), -80f));
    }

    public void OnSFXVolumeChange(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Max(20f * Mathf.Log10(value), -80f));
    }
}

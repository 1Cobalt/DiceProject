using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public Slider musicSlider;

    public TextMeshProUGUI musicSliderTextMesh;

    public AudioSource audioSource;
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    

    void Start()
    {
        musicSlider.value = LoadVolume();
        OnMusicSliderChange();
        LoadMusic();
      
    }

    public void LoadMusic()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            audioSource.clip = menuMusic;
        else
            audioSource.clip = gameMusic;

        audioSource.Play();
    }

    public void OnMusicSliderChange()
    {
        SaveVolume(musicSlider.value);
        audioSource.volume = musicSlider.value;
        musicSliderTextMesh.text = "" + (int)(musicSlider.value * 100) + "%";
    }

    public void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public float LoadVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 1f); // 1f — значение по умолчанию
    }
}

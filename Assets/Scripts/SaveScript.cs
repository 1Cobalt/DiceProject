using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScript : MonoBehaviour
{
    public static SaveScript Instance;
    public static bool isPlayingAgainstAI;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            isPlayingAgainstAI = false;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }

        

    }


    public void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public float LoadVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 1f); 
    }

}

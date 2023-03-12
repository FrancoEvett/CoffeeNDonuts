using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// simple script that stop obecjt frobeing deleted from scene to scene, and then removes itself from the component
/// </summary>


public class DontDestroy : MonoBehaviour
{
    public void Start()
    {
        if (MusicSettings.musicLoaded)
        {
            Destroy(gameObject);
        }
        else
        {
            MusicSettings.musicLoaded = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}
public static class MusicSettings
{
    //class to hold persistant music seetings
    public static bool musicLoaded;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// class to controll sound managment and playing
/// </summary>




public class SoundController : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private List<AudioClip> audioClipsRaw = new List<AudioClip>();    //load all audio clips into here

    //reason i have 2 scources is to be able to change volume of different effects as well as multiple effect at a time through the channels
    [Header("Sources")]
    [SerializeField] private AudioSource soundEffects;
    [SerializeField] private AudioSource menuEffects;




    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();//hash map of clips






    public void Start()
    {
        //compile ausio clips into a hash map
        foreach(AudioClip audioClip in audioClipsRaw)
        {
            audioClips.Add(audioClip.name, audioClip);
        }
        audioClipsRaw = null;
    }



    public void PlaySoundEffect(string name, bool interrupt)
    {
        if (audioClips.ContainsKey(name))
        {
            if (soundEffects.isPlaying)
            {
                if (interrupt)
                {
                    //interup the sound, stoping it and playign desired sound
                    soundEffects.Stop();
                    soundEffects.PlayOneShot(audioClips[name]);
                }
                else
                {
                    //overlay this sounds, so they both play at the smae time
                    soundEffects.PlayOneShot(audioClips[name]);
                }
            }
            else
            {
                //no otehr sound is playing, just play sound
                soundEffects.PlayOneShot(audioClips[name]);
            }            
        }
    }
    public void PlayMenuEffect(string name)
    {
        if (audioClips.ContainsKey(name))
        {
            menuEffects.PlayOneShot(audioClips[name]);
        }
    }


}

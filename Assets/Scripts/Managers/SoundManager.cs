using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager manager;

    public AudioSource splatSound;
    public AudioSource explosionSound;
    public AudioSource punchSound;
    public AudioSource[] shardSounds;
    public AudioSource[] eatSounds;

    public void PlayEatSound()
    {
        int index = Random.Range(0, eatSounds.Length - 1);
        eatSounds[index].Play();
    }

    private void Awake()
    {
        if (manager != null)
            GameObject.Destroy(manager);
        else
            manager = this;
        // DontDestroyOnLoad(this);
    }

    private void Update()
    {
        
    }
}

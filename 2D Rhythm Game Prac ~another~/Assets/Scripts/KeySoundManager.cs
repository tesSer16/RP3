using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySoundManager : MonoBehaviour
{
    public static KeySoundManager instance { get; set; }
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public AudioSource audioSource;
    public AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.2f;
        audioSource.clip = audioClip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

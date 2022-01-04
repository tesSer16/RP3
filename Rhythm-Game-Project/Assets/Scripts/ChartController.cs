using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartController : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip music;
    private List<List<float>> Notes;
    public Slider progressBar;
    public Button pButton;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        string temp = "[Arcaea] Axium Crisis - ak+q";
        // music.clip = Resources.Load<AudioClip>("Music/" + Info.musicTitle);
        music = Resources.Load<AudioClip>("Music/" + temp);
        audioSource.clip = music;      
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying) 
            progressBar.value = audioSource.time / music.length;
    }

    public void Pause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            pButton.GetComponentInChildren<Text>().text = "¢º";
        }
        else
        {
            audioSource.Play();
            pButton.GetComponentInChildren<Text>().text = "II";
        }
    }

    public void OnPointerUp()
    {
        audioSource.time = progressBar.value * music.length;
        audioSource.Play();
    }

    public void OnPointerDown()
    {
        audioSource.Pause();
    }
}

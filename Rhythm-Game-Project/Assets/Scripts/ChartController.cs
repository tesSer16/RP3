using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartController : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip music;
    private List<List<int>> Notes;
    public Slider progressBar;
    public Button pButton;
    public Text timeText;
    private string totalTime;
    private float noteSpeed = 10.0f;
    private int maxOrder;

    void Start()
    {
        // 음악 불러오기
        audioSource = GetComponent<AudioSource>();
        string temp = "[Arcaea] Axium Crisis - ak+q";
        // music.clip = Resources.Load<AudioClip>("Music/" + Info.musicTitle);
        music = Resources.Load<AudioClip>("Music/" + temp);
        audioSource.clip = music;

        // 음악 진행 상황 텍스트 
        totalTime = $"{(int) music.length / 60}:{(int) music.length % 60:D2}";
        timeText.text = CalculatedTime();

        // 차트 초기화(끝값 오류 방지 +1)
        maxOrder = Convert.ToInt32(music.frequency * music.length) / 1000 + 1;
        Notes = new List<List<int>>();
        for (int i = 0; i < 4; i++)
        {
            Notes.Add(new List<int>());
            for (int j = 0; j < maxOrder; j++)
            {
                Notes[i].Add(0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // progressbar update
        if (audioSource.isPlaying)
            progressBar.value = audioSource.time / music.length;

        //time update
        timeText.text = CalculatedTime();
    }

    public string CalculatedTime()
    {
        int currentTime = Convert.ToInt32(progressBar.value * music.length);
        string current = $"{currentTime / 60}:{currentTime % 60:D2}";
        return $"{current} / {totalTime}";
    }

    public void MakeNote(int trail)
    {
        Notes[trail][audioSource.timeSamples / 1000] = 1;
    }

    public void Save()
    {
        if (!audioSource.isPlaying)
        {
            Debug.Log("Hi");
        }
    }

    public void Pause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            pButton.GetComponentInChildren<Text>().text = "▶";
        }
        else
        {
            audioSource.Play();
            pButton.GetComponentInChildren<Text>().text = "II";
        }
    }

    public void OnPointerUp()
    {
        audioSource.time = Mathf.Clamp(progressBar.value * music.length, 0, music.length - 0.00001f);
        audioSource.Play();
    }

    public void OnPointerDown()
    {
        audioSource.Pause();
    }
}

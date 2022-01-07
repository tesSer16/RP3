using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class NoteController : MonoBehaviour
{
    class Note
    {
        public int trail { get; set; }
        public int order { get; set; }
        public float startY { get; set; }
        public Note(int trail, int order, float startY)
        {
            this.order = order;
            this.trail = trail;
            this.startY = startY;
        }
    }

    private ObjectPooler noteObjectPooler;
    private List<Note> notes = new List<Note>();
    private float x, z, startY;

    private float time = 0;
    private bool finished = false;

    public static AudioSource audioSource;
    private AudioClip music;
    int maxOrder;
    string _path;
    int chartInterval;

    void MakeNote(Note note, int trail)
    {
        GameObject obj = noteObjectPooler.getObject(note.trail);
        x = -15.0f + 10.0f * trail;
        z = obj.transform.position.z;
        startY = note.startY;
        obj.transform.position = new Vector3(x, startY, z);
        obj.GetComponent<NoteBehavior>().Initialize();
        obj.SetActive(true);
    }

    IEnumerator AwaitMakeNote(Note note)
    {
        int trail = note.trail;
        int order = note.order;
        while (audioSource.timeSamples < order * 1024) yield return 0;
        MakeNote(note, trail);
    }

    IEnumerator CountDown(int num)
    {
        for (int n = num; n > 0; n--)
        {
            GameManager.instance.count.text = n.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        GameManager.instance.count.text = "";
        audioSource.Play();
        yield return new WaitForSeconds(music.length + 2.0f);
        Debug.Log($"Perfect: {Info.perfect}");
        Debug.Log($"Good: {Info.good}");
        Debug.Log($"Bad: {Info.bad}");
        Debug.Log($"miss: {Info.miss}");
    }

    void Start()
    {
        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();

        // 음악 불러오기
        audioSource = GetComponent<AudioSource>();
        music = Resources.Load<AudioClip>("Music/" + Info.musicTitle);
        audioSource.clip = music;

        // 차트 불러오기
        maxOrder = Convert.ToInt32(music.frequency * music.length) / 1024 + 1;
        _path = "Assets/Resources/Charts/" + Info.chartTitle;

        StreamReader reader = new StreamReader(_path);
        reader.ReadLine();
        string[] lines = reader.ReadToEnd().Split('\n');
        chartInterval = (int)(music.frequency * 110.0f / GameManager.instance.noteSpeed * 0.1) / 1024;

        if (lines.Length != maxOrder + 1)
        {
            Debug.LogError("차트와 노래 길이가 일치하지 않습니다.");
        }
        else
        {
            for (int i = 0; i < maxOrder; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (lines[i][j] == '1')
                    {
                        float y = 173.7f;
                        int ni = i - chartInterval;
                        if (ni < 0)
                        {
                            y += 110.0f * ni / chartInterval;
                        }
                        notes.Add(new Note(j, ni, y));
                    }                        
                }
            }
        }
        

        for (int i = 0; i < notes.Count; i++)
        {
            StartCoroutine(AwaitMakeNote(notes[i]));
        }
        StartCoroutine(CountDown(3));
    }
}

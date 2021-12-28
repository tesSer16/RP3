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
        public Note(int trail, int order)
        {
            this.order = order;
            this.trail = trail;
        }
    }

    public GameObject[] Notes;
    private ObjectPooler noteObjectPooler;
    private List<Note> notes = new List<Note>();
    private float x, z, startY;

    private float time = 0;
    private bool finished = false;

    // song variables
    private string musicTitle;
    private string musicArtist;
    private int BPM;
    private int divider;
    private float startingPoint;
    private float beatCount;
    private float beatInterval;

    void MakeNote(Note note, int trail)
    {
        GameObject obj = noteObjectPooler.getObject(note.trail);
        x = obj.transform.position.x;
        z = obj.transform.position.z;
        startY = 180.0f;
        obj.transform.position = new Vector3(x, startY, z);
        obj.GetComponent<NoteBehavior>().Initialize();
        obj.SetActive(true);
    }

    IEnumerator AwaitMakeNote(Note note)
    {
        int trail = note.trail;
        int order = note.order;

        yield return new WaitUntil(() => Info.order + 13 == order);
        MakeNote(note, trail);
    }
    void Start()
    {
        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();
        BPM = 160;
        divider = 15;
        startingPoint = 3.5f;

        beatCount = (float)BPM / divider;
        beatInterval = 1 / beatCount;
        Info.beatInterval = beatInterval;

        Info.Chart = new List<Queue<int>>();
        for (int i = 0; i < 4; i++)
            Info.Chart.Add(new Queue<int>());

        Info.order = -13;
        notes.Add(new Note(0, 2));
        notes.Add(new Note(1, 6));
        notes.Add(new Note(2, 10));
        notes.Add(new Note(3, 14));

        Info.Chart[0].Enqueue(2);
        Info.Chart[1].Enqueue(6);
        Info.Chart[2].Enqueue(10);
        Info.Chart[3].Enqueue(14);

        for (int i = 0; i < notes.Count; i++)
        {
            StartCoroutine(AwaitMakeNote(notes[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            if (time > 0) time -= Time.deltaTime;
            else
            {
                time = Info.beatInterval;
                Info.order++;
            }
        }
    }
}

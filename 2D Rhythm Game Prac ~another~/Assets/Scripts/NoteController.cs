using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteController : MonoBehaviour
{
    class Note
    {
        public int noteType { get; set; }
        public int order { get; set; }
        public Note(int noteType, int order)
        {
            this.noteType = noteType;
            this.order = order;
        }
    }

    class LongNote
    {
        public int noteType { get; set; }
        public int startOrder { get; set; }
        public int endOrder { get; set; }
        public LongNote(int noteType, int order)
        {
            this.noteType = noteType;
            this.startOrder = startOrder;
            this.endOrder = endOrder;
        }
    }

    public GameObject[] Notes;
    private ObjectPooler noteObjectPooler;
    private bool finished = false;

    private List<Note> notes = new List<Note>();
    private List<LongNote> longNotes = new List<LongNote>();

    private float sync = - 0.43f;
    private float x, z, startY;

    void MakeNote(Note note)
    {
        GameObject obj = noteObjectPooler.getObject(note.noteType);

        x = obj.transform.position.x;
        startY = 8.0f + sync;
        z = obj.transform.position.z;

        obj.transform.position = new Vector3(x, startY, z);
        obj.GetComponent<NoteBehavior>().Initialize();
        obj.SetActive(true);
    }

    private string musicTitle;
    private string musicArtist;
    private int BPM;
    private int divider;
    private float startingPoint;
    private float beatCount;
    private float beatInterval;
    private float longNoteInterval;

    public AudioClip tickSound;
    public AudioSource audioSource;

    private bool temp = false;
    private float time;
    private float value;

    IEnumerator AwaitMakeNote(Note note)
    {
        int noteType = note.noteType;
        int order = note.order;

        yield return new WaitUntil(() => PlayerInformation.order + (int) value - 1 == order);
        MakeNote(note);
    }

    IEnumerator Metronome()
    {
        yield return new WaitForSeconds(startingPoint);
        temp = true;
    }

    void Start()
    {
        Debug.Log("Play mode");
        finished = false;
        PlayerInformation.test = new List<float>();

        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();

        //TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + PlayerInformation.selectedMusic);
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/4");
        StringReader reader = new StringReader(textAsset.text);
        
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = tickSound;

        //First line - music name
        musicTitle = reader.ReadLine();
        //Second - artist
        musicArtist = reader.ReadLine();
        //Third - information
        string[] beatInformation = reader.ReadLine().Split(' ');
        BPM = Convert.ToInt32(beatInformation[0]);
        divider = Convert.ToInt32(beatInformation[1]);
        startingPoint = (float) Convert.ToDouble(beatInformation[2]);

        beatCount = (float) BPM / divider;
        beatInterval = 1 / beatCount;
        longNoteInterval = beatInterval / 2;

        time = beatInterval;
        PlayerInformation.beatInterval = beatInterval;

        value = 12.1f / (GameManager.instance.noteSpeed) / beatInterval + 1;
        PlayerInformation.order = (int) -value;

        string line;
        int order;
        string[] isNote;

        // Note ordering
        PlayerInformation.noteOrders = new List<Queue<int>>();
        for (int i = 0; i < 4; i++) 
            PlayerInformation.noteOrders.Add(new Queue<int>());

        while ((line = reader.ReadLine()) != null)
        {
            order = Convert.ToInt32(line.Split(',')[0]);
            isNote = line.Split(',')[1].Split(' ');
            
            for (int i = 0; i < isNote.Length; i++)
            {
                if (isNote[i] == "1")
                {
                    Note note = new Note(i + 1, order);
                    notes.Add(note);
                    PlayerInformation.noteOrders[i].Enqueue(order);
                }
            }
        }

        StartCoroutine(Metronome());
        for (int i = 0; i < notes.Count; i++)
        {
            StartCoroutine(AwaitMakeNote(notes[i]));
        }
        PlayerInformation.beats = notes.Count + longNotes.Count;

        //Game result
        StartCoroutine(AwaitGameResult(notes[notes.Count - 1].order));
    }

    IEnumerator AwaitGameResult(int order)
    {
        yield return new WaitForSeconds(startingPoint + order * beatInterval + 4.0f);
        finished = true;
        StreamWriter sw = new StreamWriter("test11.txt");
        foreach (float var in PlayerInformation.test)
            sw.WriteLine(var);
        sw.Flush();
        sw.Close();

        GameResult();
    }

    void GameResult()
    {
        PlayerInformation.maxCombo = GameManager.instance.maxCombo;
        PlayerInformation.score = GameManager.instance.score;
        PlayerInformation.musicTitle = musicTitle;
        PlayerInformation.musicArtist = musicArtist;
        SceneManager.LoadScene("GameResultScene");
    }

    private void Update()
    {
        if (temp && !finished)
        {
            if (time > 0) time -= Time.deltaTime;
            else
            {
                time = PlayerInformation.beatInterval;
                PlayerInformation.order++;
            }
        }
    }
}

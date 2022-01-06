using System;
using System.IO;
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
    private ChartPooler chartPooler;
    private List<Queue<GameObject>> activeObject;
    private int chartInterval;

    StreamWriter sw;
    string chartName;
    string _path;

    private bool[] recordCooldowns;
    private float recordCool;

    public GameObject background;
    void Start()
    {
        // ������Ʈ Ǯ��
        chartPooler = GetComponent<ChartPooler>();

        // ���� �ҷ�����
        audioSource = GetComponent<AudioSource>();
        string temp = "[Arcaea] Axium Crisis - ak+q";
        // music.clip = Resources.Load<AudioClip>("Music/" + Info.musicTitle);
        music = Resources.Load<AudioClip>("Music/" + temp);
        audioSource.clip = music;

        // ���� ���� ��Ȳ �ؽ�Ʈ 
        totalTime = $"{(int) music.length / 60}:{(int) music.length % 60:D2}";
        timeText.text = CalculatedTime();

        // �ؽ�Ʈ ���� ���� (���� ���� ���� +1)
        maxOrder = Convert.ToInt32(music.frequency * music.length) / 1024 + 1;
        chartName = "test_chart";
        _path = "Assets/Resources/Charts/" + chartName + ".txt";
        if (!File.Exists(_path))
        {
            sw = new StreamWriter(_path);
            for (int i = 0; i < maxOrder; i++)
            {
                sw.WriteLine("0000");
            }
            sw.Flush();
            sw.Close();
        }

        StreamReader reader = new StreamReader(_path);
        string[] lines = reader.ReadToEnd().Split('\n');

        if (lines.Length != maxOrder + 1)
        {
            Debug.LogError("��Ʈ�� �뷡 ���̰� ��ġ���� �ʽ��ϴ�.");
        }
        else
        {
            Notes = new List<List<int>>();
            for (int i = 0; i < maxOrder; i++)
            {
                Notes.Add(new List<int>());
                for (int j = 0; j < 4; j++)
                {
                    if (lines[i][j] == '1')
                        Notes[i].Add(1);
                    else
                        Notes[i].Add(0);
                }
            }
        }

        // ��Ÿ�� �ʱ�ȭ
        recordCooldowns = new bool[4];
        for (int i = 0; i < 4; i++) recordCooldowns[i] = true;

        // �����
        Debug.Log(background.GetComponent<Renderer>().bounds.size);
        Debug.Log((int)(music.frequency * 110.0f / noteSpeed / 10) / 1024);
    }

    void Update()
    {
        // progressbar update
        if (audioSource.isPlaying)
            progressBar.value = audioSource.time / music.length;

        //time update
        timeText.text = CalculatedTime();

        // chart update
        chartInterval = (int)(music.frequency * 110.0f / noteSpeed / 10) / 1024;
        int currentOrder = (int) (progressBar.value * music.length * music.frequency) / 1024;
        
    }

    public string CalculatedTime()
    {
        int currentTime = Convert.ToInt32(progressBar.value * music.length);
        string current = $"{currentTime / 60}:{currentTime % 60:D2}";
        return $"{current} / {totalTime}";
    }

    public void MakeNote(int trail)
    {
        if (audioSource.isPlaying && recordCooldowns[trail])
        {
            Notes[audioSource.timeSamples / 1024][trail] = 1;
            StartCoroutine(RecordCoolTime(trail));
        }
    }

    IEnumerator RecordCoolTime(int trail)
    {
        recordCooldowns[trail] = false;
        yield return new WaitForSeconds(0.125f);
        recordCooldowns[trail] = true;
    }

    private void ChartUpdate(int order)
    {
        activeObject = new List<Queue<GameObject>>();
        for (int i = 0; i < 4; i++)
        {
            activeObject.Add(new Queue<GameObject>());
            for (int k = 0; k < chartInterval; k++)
            {
                if (Notes[order + k][i] == 1)
                {
                    Debug.Log(1);
                }
            }
        }
    }

    public void Save()
    {
        if (!audioSource.isPlaying)
        {
            sw = new StreamWriter(_path);
            for (int i = 0; i < maxOrder; i++)
            {
                string data = "";
                for (int j = 0; j < 4; j++)
                {
                    data += Convert.ToString(Notes[i][j]);
                }
                sw.WriteLine(data);
            }
            sw.Flush();
            sw.Close();

            Debug.Log("������ �Ϸ�Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log("���� ���¿����� ������ �����մϴ�.");
        }
    }

    public void Pause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            pButton.GetComponentInChildren<Text>().text = "��";
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

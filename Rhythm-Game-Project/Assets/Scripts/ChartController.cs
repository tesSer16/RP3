using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartController : MonoBehaviour
{
    public static AudioSource audioSource;
    private AudioClip music;
    private static List<List<int>> Notes;
    public Slider progressBar;
    public Button pButton;
    public Text timeText;
    public Text speedText;

    private string totalTime;
    private float noteSpeed = 9.0f;
    private int maxOrder;
    private ChartPooler chartPooler;
    public static int chartInterval;
    private static int currentOrder;
    private bool paused = true;
    private bool finished = false;

    StreamWriter sw;
    string chartName;
    string _path;

    private bool[] recordCooldowns;
    private float recordCool;

    public static bool clicked;

    // 디버깅 변수

    void Start()
    {
        // 오브젝트 풀링
        chartPooler = GetComponent<ChartPooler>();

        // 음악 불러오기
        audioSource = GetComponent<AudioSource>();
        string temp = "[Arcaea] Axium Crisis - ak+q";
        // music.clip = Resources.Load<AudioClip>("Music/" + Info.musicTitle);
        music = Resources.Load<AudioClip>("Music/" + temp);
        audioSource.clip = music;

        // 음악 진행 상황 텍스트 
        totalTime = $"{(int) music.length / 60}:{(int) music.length % 60:D2}";
        timeText.text = CalculatedTime();

        // 텍스트 파일 생성 (끝값 오류 방지 +1)
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
            Debug.LogError("차트와 노래 길이가 일치하지 않습니다.");
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

        // 쿨타임 초기화
        recordCooldowns = new bool[4];
        for (int i = 0; i < 4; i++) recordCooldowns[i] = true;

        // 초기화
        clicked = false;
        speedText.text = "9.0";
        chartPooler = gameObject.GetComponent<ChartPooler>();
        chartInterval = (int)(music.frequency * 110.0f / noteSpeed * 0.1) / 1024;
        ChartUpdate(0);

        // 디버그
        Debug.Log(music.length * music.frequency);
    }

    void Update()
    {
        // progressbar update
        if (audioSource.isPlaying)
            progressBar.value = audioSource.time / music.length;

        //time update
        timeText.text = CalculatedTime();

        // chart update
        currentOrder = (int) (progressBar.value * music.length * music.frequency) / 1024;
        /*
        최적화 코드(슬라이더로 움직일 때는 일부 누락돼서 일단 빼놓음)
        (9.0기준) 최적화 전 - 47 * 4 * 2번 탐색, 최적화 후 - 2 * 4번 탐색
        for (int i = 0; i < 4; i++)
        {
            if (Notes[currentOrder][i] == 1)
                chartPooler.SetObject(i, currentOrder);
            if (maxOrder > currentOrder + chartInterval)
            {
                if (Notes[currentOrder + chartInterval][i] == 1)
                    chartPooler.SetObject(i, currentOrder + chartInterval);
            }
        }
        */

        if (!clicked)
        {
            ChartUpdate(currentOrder);
            chartPooler.PositionUpdate(currentOrder, chartInterval);
        }

        // 노래 종료
        if (music.length * music.frequency - audioSource.timeSamples < 0)
        {
            paused = true;
            finished = true;
            pButton.GetComponentInChildren<Text>().text = "▶";
        }
    }

    private string CalculatedTime()
    {
        int currentTime = Convert.ToInt32(progressBar.value * music.length);
        string current = $"{currentTime / 60}:{currentTime % 60:D2}";
        return $"{current} / {totalTime}";
    }

    public static void EditNote(int a, int b, int c, int d)
    {
        Notes[b][a] = 0;
        Notes[currentOrder + d][c] = 1;
    }

    public void MakeNote(int trail)
    {
        if (recordCooldowns[trail])
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
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < chartPooler.NotePools[i].Count; j++)
            {
                chartPooler.NotePools[i][j].SetActive(false);
                chartPooler.NotePools[i][j].GetComponent<ChartBehavior>().order = -1;
            }

            if (maxOrder > order + chartInterval)
            {
                for (int k = 0; k < chartInterval; k++)
                {
                    if (Notes[order + k][i] == 1)
                    {
                        chartPooler.SetObject(i, order + k);
                    }
                }
            }
        }
    }

    public void SpeedUp1()
    {
        ChangeSpeed(0.1f);
    }
    public void SpeedUp10()
    {
        ChangeSpeed(1.0f);
    }
    public void SpeedDown1()
    {
        ChangeSpeed(-0.1f);
    }
    public void SpeedDown10()
    {
        ChangeSpeed(-1.0f);
    }
    private void ChangeSpeed(float value)
    {
        noteSpeed = Mathf.Clamp(noteSpeed + value, 2.0f, 13.0f);
        chartInterval = (int)(music.frequency * 110.0f / noteSpeed * 0.1) / 1024;
        speedText.text = $"{noteSpeed:0.0}";
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

            Debug.Log("저장이 완료되었습니다.");
        }
        else
        {
            Debug.Log("정지 상태에서만 저장이 가능합니다.");
        }
    }

    public void Pause()
    {
        if (audioSource.isPlaying)
        {
            paused = true;
            audioSource.Pause();
            pButton.GetComponentInChildren<Text>().text = "▶";
        }
        else
        {
            if (finished) 
            {
                audioSource.time = 0;
                finished = false;
            }
            
            paused = false;
            audioSource.Play();
            pButton.GetComponentInChildren<Text>().text = "II";
        }
    }

    public void OnPointerUp()
    {
        audioSource.time = Mathf.Clamp(progressBar.value * music.length, 0, music.length - 0.00001f);
        if (!paused)
            audioSource.UnPause();        
        ChartUpdate(currentOrder);
    }

    public void OnPointerDown()
    {
        audioSource.Pause();
    }
}

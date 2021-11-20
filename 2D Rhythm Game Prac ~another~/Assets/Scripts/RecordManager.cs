using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordManager : MonoBehaviour
{
    private float startingPoint = 3.5f;
    private float beatInterval;
    private float beatCount;
    private int BPM = 160;
    private int divider = 15;
    private bool finished = false;

    public AudioClip tickSound;
    public AudioSource audioSource;

    IEnumerator AwaitGameResult(float time)
    {
        yield return new WaitForSeconds(time);
        finished = true;
        Debug.Log("Record Finish");

        for (int i = 0; i < 4; i++)
        {
            Debug.Log(i.ToString() + ": " + string.Join(" ", PlayerInformation.noteRecord[i]));
        }

        RecordEnd();
    }

    IEnumerator Metronome(float time)
    {
        yield return new WaitForSeconds(startingPoint);
        Debug.Log("Record Start");
        while (!finished)
        {
            PlayerInformation.order++;
            //audioSource.Play();
            yield return new WaitForSeconds(time);
        }
    }

    void RecordEnd()
    {
        StreamWriter sw = new StreamWriter("Assets/Resources/Beats/4.txt");
        sw.WriteLine("Drops of H20");
        sw.WriteLine("J.Lang");
        sw.WriteLine("160 15 3.5");

        for (int i = 0; i < PlayerInformation.order; i++)
        {
            List<int> value = new List<int>() { 0, 0, 0, 0 };
            for (int j = 0; j < 4; j++)
            {
                if (PlayerInformation.noteRecord[j].Count > 0)
                {
                    if (PlayerInformation.noteRecord[j][0] == i)
                    {
                        PlayerInformation.noteRecord[j].Remove(i);
                        value[j] = 1;
                    }
                }
            }
            sw.WriteLine(string.Format("{0},{1}", i, string.Join(" ", value)));
        }

        sw.Flush();
        sw.Close();

        SceneManager.LoadScene("SongSelectScene");
    }


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = tickSound;

        beatCount = (float)BPM / divider;
        beatInterval = 1 / beatCount;
        PlayerInformation.beatInterval = beatInterval;

        float value = 12.1f / (GameManager.instance.noteSpeed) / beatInterval + 1;
        PlayerInformation.order = (int) -value;

        PlayerInformation.noteRecord = new List<List<int>>();
        for (int i = 0; i < 4; i++) 
            PlayerInformation.noteRecord.Add(new List<int>());

        StartCoroutine(AwaitGameResult(10.0f));
        StartCoroutine(Metronome(beatInterval));
    }
}

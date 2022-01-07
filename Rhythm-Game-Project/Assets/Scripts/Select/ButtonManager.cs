using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    private string song = "";
    private string chart = "";
    public void SelectSong()
    {
        Info.musicTitle = song;
        Info.chartTitle = chart;
        SceneManager.LoadScene("GameScene");
    }

    public void SetData(string name1, string name2)
    {
        song = name1;
        chart = name2;
    }

    // Chart Select Scene
    public void MakeChart()
    {
        // ÆË¾÷ ¶ç¿ö¼­ chartTitle ÀÔ·Â (¿¹Á¤)
        SceneManager.LoadScene("SongSelectScene");
    }

    // Song Select Scene
    public void NextFolder()
    {
        string folder = GetComponentInChildren<Text>().text;
        GameObject.Find("Song Select Manager").GetComponent<SongSelectManager>().Next(folder);
    }

    public void PreviousFolder()
    {
        GameObject.Find("Song Select Manager").GetComponent<SongSelectManager>().Back();
    }

    public void StartSong()
    {
        string file = GetComponentInChildren<Text>().text;
        GameObject.Find("Song Select Manager").GetComponent<SongSelectManager>().Play(file);
    }
}

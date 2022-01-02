using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public string songPath = "";
    public void SelectSong()
    {
        //GameObject obj = EventSystem.current.currentSelectedGameObject; -> ±×³É Get
        //Debug.Log(obj.GetComponentInChildren<Text>().text);
        Debug.Log(songPath);
    }

    public void SetPath(string path)
    {
        songPath = path;
    }

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

    public void MakeChart()
    {
        Debug.Log("Add song");
    }
}

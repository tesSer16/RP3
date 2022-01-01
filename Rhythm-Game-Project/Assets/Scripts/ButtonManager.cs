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
        //GameObject obj = EventSystem.current.currentSelectedGameObject;
        //Debug.Log(obj.GetComponentInChildren<Text>().text);
        Debug.Log(songPath);
    }

    public void SetPath(string path)
    {
        songPath = path;
    }

    public void MakeChart()
    {

    }
}

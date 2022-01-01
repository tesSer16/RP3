using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SongSelectManager : MonoBehaviour
{
    private string _path;
    private string[] files;
    private string[] folders;
    public GameObject Content;
    public GameObject BG;
    public GameObject FG;
    public GameObject MG;
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            _path = (Application.persistentDataPath.Replace("Android", "")).Split(new string[] { "//" }, System.StringSplitOptions.None)[0];
        }
        else
        {
            _path = "C:/";
        }

        FileUpdate();
    }

    private void FileUpdate()
    {
        // 기존 오브젝트 삭제
        //GameObject[] child = Content.transform.FindChild
        foreach (GameObject c in child)
        {
            if (c != Content.transform)
            {
                Destroy(c.gameObject);
            }
        }

        folders = Directory.GetDirectories(_path);
        files = Directory.GetFiles(_path);
        foreach (string folder in folders)
        {
            if (folder[0] != '$')
            {
                GameObject f = Instantiate(MG) as GameObject;
                f.transform.SetParent(Content.transform, false);

                FileInfo fi = new FileInfo(folder);
                f.GetComponentInChildren<Text>().text = fi.Name;
            }
        }

        foreach (string file in files)
        {
            FileInfo fi = new FileInfo(file);
            if (fi.Extension == ".mp3")
            {
                GameObject f = Instantiate(FG) as GameObject;
                f.transform.SetParent(Content.transform, false);
                
                f.GetComponentInChildren<Text>().text = fi.Name;
            }
        }
    }

    public void NextFolder()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;

public class SongSelectManager : MonoBehaviour
{
    private string _path;
    private string origin;
    private string[] files;
    private string[] folders;
    public GameObject Content;
    public GameObject BG;
    public GameObject FG;
    public GameObject MG;
    public GameObject scrollView;

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
        origin = string.Copy(_path);

        FileUpdate();
    }

    private void FileUpdate()
    {
        // ���� ������Ʈ ����
        Transform[] child = Content.GetComponentsInChildren<Transform>();
        foreach (Transform c in child)
        {
            if (c != Content.transform)
            {
                Destroy(c.gameObject);
            }
        }
        if (_path != origin)
        {
            GameObject b = Instantiate(BG) as GameObject;
            b.transform.SetParent(Content.transform, false);
        }

        // ���� ó�� �ʿ� UnauthorizedAccessException
        try
        {
            folders = Directory.GetDirectories(_path);
            files = Directory.GetFiles(_path);
        }
        catch (UnauthorizedAccessException)
        {
            Debug.Log("���� ������ �����ϴ�.");
            Back();
        }
            
        // ���丮 ������Ʈ ����
        int n = 0, m = 0;
        foreach (string folder in folders)
        {
            FileInfo fi = new FileInfo(folder);
            if (fi.Name[0] != '$')
            {
                GameObject f = Instantiate(FG) as GameObject;
                f.transform.SetParent(Content.transform, false);

                f.GetComponentInChildren<Text>().text = fi.Name;
                n++;
            }
        }

        // mp3 ���� ������Ʈ ����
        foreach (string file in files)
        {
            FileInfo fi = new FileInfo(file);
            if (fi.Extension == ".mp3")
            {
                GameObject f = Instantiate(MG) as GameObject;
                f.transform.SetParent(Content.transform, false);
                
                f.GetComponentInChildren<Text>().text = fi.Name;
                m++;
            }
        }
    }
    
    public void Next(string name)
    {
        if (_path != "C:/") _path += '/';
        _path += name;
        FileUpdate();
    }

    public void Back()
    {
        FileInfo fi = new FileInfo(_path);
        _path = _path.Replace('/' + fi.Name, "");
        if (_path == "C:") _path += '/';
        FileUpdate();
    }

    public void Play(string name)
    {
        Info.musicPath = _path + '/' + name;
        Info.musicTitle = name.Substring(0, name.Length - 4);
        Debug.Log(Info.musicTitle);
        File.Copy(_path + '/' + name, Application.dataPath + "/Resources/Music/" + name, true);
        // �ε� �߰� ����, ���� ���� Ž��
        SceneManager.LoadScene("MakerScene");
    }
}
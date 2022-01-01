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

        FileUpdate();
    }

    private void FileUpdate()
    {
        // 기존 오브젝트 삭제
        Transform[] child = Content.GetComponentsInChildren<Transform>();
        foreach (Transform c in child)
        {
            if (c != Content.transform)
            {
                Destroy(c.gameObject);
            }
        }

        folders = Directory.GetDirectories(_path);
        files = Directory.GetFiles(_path);
        // 디렉토리 오브젝트 생성
        int n = 0, m = 0;
        foreach (string folder in folders)
        {
            FileInfo fi = new FileInfo(folder);
            Debug.Log(fi.Name);
            if (fi.Name[0] != '$')
            {
                GameObject f = Instantiate(FG) as GameObject;
                f.transform.SetParent(Content.transform, false);

                f.GetComponentInChildren<Text>().text = fi.Name;
                n++;
            }
        }

        // mp3 파일 오브젝트 생성
        foreach (string file in files)
        {
            FileInfo fi = new FileInfo(file);
            if (fi.Extension == ".mp3")
            {
                GameObject f = Instantiate(FG) as GameObject;
                f.transform.SetParent(Content.transform, false);
                
                f.GetComponentInChildren<Text>().text = fi.Name;
                m++;
            }
        }

        // Content size 설정
        ScrollRect scrollRect = scrollView.GetComponent<ScrollRect>();
        float width = 1400.0f;
        float height = 150 * (n + m) + 20;
        scrollRect.content.sizeDelta = new Vector2(width, height);
    }

    public void NextFolder()
    {

    }
}

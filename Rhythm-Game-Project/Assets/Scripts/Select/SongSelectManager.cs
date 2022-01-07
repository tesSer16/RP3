using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;
using UnityEngine.Networking;

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
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
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
        // 기존 오브젝트 삭제
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

        // 예외 처리
        try
        {
            folders = Directory.GetDirectories(_path);
            files = Directory.GetFiles(_path);
        }
        catch (UnauthorizedAccessException)
        {
            Debug.Log("접근 권한이 없습니다.");
            Back();
        }
            
        // 디렉토리 오브젝트 생성
        foreach (string folder in folders)
        {
            FileInfo fi = new FileInfo(folder);
            if (fi.Name[0] != '$')
            {
                GameObject f = Instantiate(FG);
                f.transform.SetParent(Content.transform, false);

                f.GetComponentInChildren<Text>().text = fi.Name;
            }
        }

        // mp3 파일 오브젝트 생성
        foreach (string file in files)
        {
            FileInfo fi = new FileInfo(file);
            if (fi.Extension == ".mp3")
            {
                GameObject f = Instantiate(MG);
                f.transform.SetParent(Content.transform, false);

                f.GetComponentInChildren<Text>().text = fi.Name;
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
        Info.musicTitle = name.Substring(0, name.Length - 4);
        StartCoroutine(DL(_path + '/' + name, Application.dataPath + "/Resources/Music/" + name));

        //File.Copy(_path + '/' + name, Application.dataPath + "/Resources/Music/" + name, true);
        // 로딩 추가 고려, 기존 파일 탐색
    }

    IEnumerator DL(string path, string target)
    {
        if (!File.Exists(target))
        {
            var request = UnityWebRequest.Get(path);
            var download = new DownloadHandlerFile(target);
            request.downloadHandler = download;
            yield return request.SendWebRequest();
        }
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MakerScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using System;

public class SongSelectManager : MonoBehaviour
{
    public GameObject Content;
    public GameObject chartPrefab;
    public GameObject addPrefab;
    public GameObject scrollView;
    void Start()
    {
        // Chart ���� �о���� (���Ŀ� mp3 ������ ����Ǿ� �ִ� �� Ȯ�� �ϴ� ���� �ʿ�)
        string[] files;
        files = Directory.GetFiles(Application.persistentDataPath + "/Charts", "*.txt");
        List<string[]> charts = new List<string[]>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo fileInfo = new FileInfo(files[i]);
            StreamReader reader = new StreamReader(files[i]);
            charts.Add(reader.ReadToEnd().Split('\n'));
            reader.Close();
        }

        // Content�� ���� ������Ʈ ���� 
        for (int i = 0; i < files.Length; i++)
        {
            GameObject chart = Instantiate(chartPrefab) as GameObject;
            chart.transform.SetParent(Content.transform, false);
            chart.GetComponentInChildren<Text>().text = string.Format(" {0}\n   - {1}", charts[i][0], charts[i][1]);
        }
        GameObject add = Instantiate(addPrefab) as GameObject;
        add.transform.SetParent(Content.transform, false);

        // Content size ����
        ScrollRect scrollRect = scrollView.GetComponent<ScrollRect>();
        float width = scrollRect.content.rect.width;
        float height = 160 * files.Length + 120;
        scrollRect.content.sizeDelta = new Vector2(width, height);
    }

    

    void Update()
    {
        
    }
}

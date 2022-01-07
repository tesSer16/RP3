using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using System;

public class ChartSelectManager : MonoBehaviour
{
    public GameObject Content;
    public GameObject chartPrefab;
    public GameObject addPrefab;
    public GameObject scrollView;
    void Start()
    {
        Screen.SetResolution(1920, 1200, true);

        // Chart 파일 읽어오기
        string[] files;
        files = Directory.GetFiles(Application.dataPath + "/Resources/Charts", "*.txt");
        List<List<string>> charts = new List<List<string>>();
        for (int i = 0; i < files.Length; i++)
        {
            StreamReader reader = new StreamReader(files[i]);
            FileInfo fi = new FileInfo(files[i]);
            charts.Add(new List<string>());
            charts[i].Add(reader.ReadLine());
            charts[i].Add(fi.Name);
            reader.Close();
        }
        Info.chartTitle = "Chart " + (files.Length + 1).ToString();

        // Content에 게임 오브젝트 생성 
        for (int i = 0; i < files.Length; i++)
        {
            GameObject chart = Instantiate(chartPrefab);
            chart.transform.SetParent(Content.transform, false);

            // 실제로 파일이 있는지 확인 (현재 illegal character issue)
            Debug.Log(charts[i][0]);
            string filePath = "Assets/Resources/Music/" + charts[i][0] + ".mp3";
            FileInfo fi = new FileInfo(filePath);

            if (fi.Exists)
            {
                chart.GetComponentInChildren<Text>().text = string.Format(" {0}\n   - {1}", charts[i][1], charts[i][0]);
                chart.GetComponent<ButtonManager>().SetData(charts[i][0], charts[i][1]);
            }
            else
            {
                chart.GetComponentInChildren<Text>().text = string.Format(" {0}\n   - 위치 정보가 올바르지 않습니다.", charts[i][1]);
            }

        }
        GameObject add = Instantiate(addPrefab);
        add.transform.SetParent(Content.transform, false);
    }
}

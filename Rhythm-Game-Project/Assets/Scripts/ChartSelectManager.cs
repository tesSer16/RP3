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
        // Chart 파일 읽어오기
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

        // Content에 게임 오브젝트 생성 
        for (int i = 0; i < files.Length; i++)
        {
            GameObject chart = Instantiate(chartPrefab) as GameObject;
            chart.transform.SetParent(Content.transform, false);

            // 실제로 파일이 있는지 확인 (현재 illegal character issue)
            Debug.Log(charts[i][2]);
            FileInfo fi = new FileInfo(charts[i][2]);

            if (fi.Exists)
            {
                chart.GetComponentInChildren<Text>().text = string.Format(" {0}\n   - {1}", charts[i][0], charts[i][1]);
                chart.GetComponent<ButtonManager>().SetPath(charts[i][2]);
            }
            else
            {
                chart.GetComponentInChildren<Text>().text = string.Format(" {0}\n   - 위치 정보가 올바르지 않습니다.", charts[i][0]);
            }

        }
        GameObject add = Instantiate(addPrefab) as GameObject;
        add.transform.SetParent(Content.transform, false);

        //// Content size 설정
        //ScrollRect scrollRect = scrollView.GetComponent<ScrollRect>();
        //float width = scrollRect.content.rect.width;
        //float height = 160 * files.Length + 100;
        //scrollRect.content.sizeDelta = new Vector2(width, height);
    }    

    void Update()
    {
        
    }
}

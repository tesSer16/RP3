using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartPooler : MonoBehaviour
{
    public GameObject Note;
    public List<List<GameObject>> NotePools;
    public int noteCount = 10;
    private bool more = true;

    void Awake()
    {
        NotePools = new List<List<GameObject>>();
        for (int i = 0; i < 4; i++)
        {
            NotePools.Add(new List<GameObject>());
            for (int j = 0; j < noteCount; j++)
            {
                GameObject obj = Instantiate(Note);
                obj.SetActive(false);
                obj.GetComponent<ChartBehavior>()._trail = i;
                obj.GetComponent<ChartBehavior>().order = -1;
                NotePools[i].Add(obj);
            }
        }
    }

    public void SetObject(int trail, int order)
    {
        foreach (GameObject obj in NotePools[trail])
        {
            if (obj.GetComponent<ChartBehavior>().order == order) return;
            if (!obj.activeInHierarchy)
            {
                obj.GetComponent<ChartBehavior>().order = order;
                obj.SetActive(true);
                return;
            }
        }
        if (more)
        {
            GameObject obj = Instantiate(Note);
            NotePools[trail].Add(obj);
            obj.GetComponent<ChartBehavior>().order = order;
            obj.SetActive(true);
        }
    }

    public void PositionUpdate(int value, int interval)
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (GameObject obj in NotePools[i])
            {
                if (obj.activeInHierarchy)
                {
                    float x = -15.0f + 10.0f * i;
                    float dy = (float) (obj.GetComponent<ChartBehavior>().order - value) / interval;
                    if (0 <= dy && dy <= 1)
                    {
                        float y = 63.7f + 110.0f * dy;
                        obj.transform.position = new Vector3(x, y, 49.4f);
                    }
                    else
                    {
                        obj.SetActive(false);
                    }
                }
            }
        }
    }
}

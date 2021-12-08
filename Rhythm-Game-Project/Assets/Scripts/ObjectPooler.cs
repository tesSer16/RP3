using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public List<GameObject> Notes;
    private List<List<GameObject>> NotePools;
    public int noteCount = 10;
    private bool more = true;

    void Start()
    {
        NotePools = new List<List<GameObject>>();
        for (int i = 0; i< Notes.Count; i++)
        {
            NotePools.Add(new List<GameObject>());
            for (int j = 0; j < noteCount; j++)
            {
                GameObject obj = Instantiate(Notes[i]);
                obj.SetActive(false);
                NotePools[i].Add(obj);
            }
        }
    }

    public void Judge(int trail)
    {
        foreach (GameObject obj in NotePools[trail])
        {
            if (obj.activeInHierarchy)
            {
                obj.GetComponent<NoteBehavior>().Judge();
            }
        }
    }
    public GameObject getObject(int trail)
    {
        foreach (GameObject obj in NotePools[trail])
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        if (more)
        {
            GameObject obj = Instantiate(Notes[trail]);
            NotePools[trail].Add(obj);
            return obj;
        }
        return null;
    }
    void Update()
    {
        
    }
}

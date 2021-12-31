using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject Note;
    private List<List<GameObject>> NotePools;
    public int noteCount = 10;
    private bool more = true;

    void Start()
    {
        NotePools = new List<List<GameObject>>();
        for (int i = 0; i< 4; i++)
        {
            NotePools.Add(new List<GameObject>());
            for (int j = 0; j < noteCount; j++)
            {
                GameObject obj = Instantiate(Note);
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
            GameObject obj = Instantiate(Note);
            NotePools[trail].Add(obj);
            return obj;
        }
        return null;
    }
    void Update()
    {
        
    }
}

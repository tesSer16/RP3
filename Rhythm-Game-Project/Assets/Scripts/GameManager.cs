using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    Camera _mainCam = null;
    public GameObject[] trails;
    public float noteSpeed;
    private void Start()
    {
        _mainCam = Camera.main;
    }

    void Update()
    {
        foreach (GameObject trail in trails)
        {
            trail.GetComponent<Renderer>().material.color = Color.black;
        }
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }        
    }
}

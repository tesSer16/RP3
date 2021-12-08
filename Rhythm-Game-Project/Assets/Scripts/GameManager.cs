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
    
    // main options
    Camera _mainCam = null;
    public float noteSpeed;
    public bool automode = true;

    //main variables
    public GameObject[] trails;
    private Renderer[] trailRenderers;
    public float score;
    public int combo;

    // note variables
    public enum judges { NONE = 0, BAD, GOOD, PERFECT, MISS };

    private void Start()
    {
        objectPooler = noteObjectPooler.GetComponent<ObjectPooler>();
        _mainCam = Camera.main;
        trailRenderers = new Renderer[trails.Length];
        for (int i = 0; i < trails.Length; i++)
        {
            trailRenderers[i] = trails[i].GetComponent<Renderer>();
        }
    }

    public GameObject noteObjectPooler;
    private ObjectPooler objectPooler;
    void Update()
    {
        /*foreach (GameObject trail in trails)
        {
            trail.GetComponent<Renderer>().material.color = Color.black;
        }
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.name == "Trail 1")
                {
                    ShineTrail(0);
                    objectPooler.Judge(0);
                }
                if (hit.collider.name == "Trail 2")
                {
                    ShineTrail(1);
                    objectPooler.Judge(1);
                }
                if (hit.collider.name == "Trail 3")
                {
                    ShineTrail(2);
                    objectPooler.Judge(2);
                }
                if (hit.collider.name == "Trail 4")
                {
                    ShineTrail(3);
                    objectPooler.Judge(3);
                }
            }
        }

        for (int i = 0; i < trailRenderers.Length; i++)
        {
            Color color = trailRenderers[i].material.color;
            if (color.a > 0) color.a -= 0.01f;
            trailRenderers[i].material.color = color;
        }
    }

    public void ShineTrail(int idx)
    {
        Color color = trailRenderers[idx].material.color;
        color.a = 0.32f;
        trailRenderers[idx].material.color = color;
    }

    private float perfectScore;
    public void processJudge(judges judge)
    {
        if (judge == judges.NONE) return;
        if (judge == judges.MISS)
        {
            // judgementSpriteRenderer.sprite = judgeSprites[2];
            combo = 0;
            Info.miss++;
        }

        else if (judge == judges.BAD)
        {
            // judgementSpriteRenderer.sprite = judgeSprites[0];
            combo = 0;
            score += perfectScore * 0.25f;
            Info.bad++;
        }

        else
        {
            if (judge == judges.PERFECT)
            {
                // judgementSpriteRenderer.sprite = judgeSprites[3];
                score += perfectScore;
                Info.perfect++;
            }
            else if (judge == judges.GOOD)
            {
                // judgementSpriteRenderer.sprite = judgeSprites[1];
                score += perfectScore * 0.75f;
                Info.good++;
            }
            combo += 1;
        }
        // showJudgement();
        Debug.Log(judge);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Text count;
    public Text judgeText;
    public GameObject judgeObj;

    // note variables
    public enum judges { NONE = 0, BAD, GOOD, PERFECT, MISS };
    public GameObject noteObjectPooler;
    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = noteObjectPooler.GetComponent<ObjectPooler>();
        _mainCam = Camera.main;
        trailRenderers = new Renderer[trails.Length];
        for (int i = 0; i < trails.Length; i++)
        {
            trailRenderers[i] = trails[i].GetComponent<Renderer>();
        }

        // 판정선 맞추기
        GameObject p, g1, g2, b1, b2, m;
        p = GameObject.Find("Perfect Line");
        g1 = GameObject.Find("Good Line (1)");
        g2 = GameObject.Find("Good Line (2)");
        b1 = GameObject.Find("Bad Line (1)");
        b2 = GameObject.Find("Bad Line (2)");
        m = GameObject.Find("Miss Line");
        
        b1.transform.Translate(Vector3.left * (1.2f * noteSpeed));
        g1.transform.Translate(Vector3.left * (0.7f * noteSpeed));
        p.transform.Translate(Vector3.left * (0.2f * noteSpeed));
        g2.transform.Translate(Vector3.right * (0.2f * noteSpeed));
        b2.transform.Translate(Vector3.right * (0.7f * noteSpeed));
        m.transform.Translate(Vector3.right * (1.2f * noteSpeed));

        judgeText.text = "";
    }
    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch tempTouch = Input.GetTouch(i);
                if (tempTouch.phase == TouchPhase.Began)
                {
                    Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.name == "Trail 1")
                        {
                            ShineJudge(0);
                        }
                        if (hit.collider.name == "Trail 2")
                        {
                            ShineJudge(1);
                        }
                        if (hit.collider.name == "Trail 3")
                        {
                            ShineJudge(2);
                        }
                        if (hit.collider.name == "Trail 4")
                        {
                            ShineJudge(3);
                        }
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.S)) ShineJudge(0);
        if (Input.GetKey(KeyCode.D)) ShineJudge(1);
        if (Input.GetKey(KeyCode.L)) ShineJudge(2);
        if (Input.GetKey(KeyCode.Semicolon)) ShineJudge(3);

        for (int i = 0; i < trailRenderers.Length; i++)
        {
            Color color = trailRenderers[i].material.color;
            if (color.a > 0) color.a -= 0.01f;
            trailRenderers[i].material.color = color;
        }
    }

    public void ShineJudge(int trail)
    {
        Color color = trailRenderers[trail].material.color;
        color.a = 0.32f;
        trailRenderers[trail].material.color = color;
        objectPooler.Judge(trail);
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
        judgeText.text = $"{judge}\n{combo}";
        judgeObj.GetComponent<Animation>().Play();
    }
}

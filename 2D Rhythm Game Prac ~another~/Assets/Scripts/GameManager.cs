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

        if (recordmode)
            noteController.gameObject.SetActive(false);
        else
            recordManager.gameObject.SetActive(false);
    }

    public float noteSpeed;

    public GameObject scoreUI;
    public float score;
    private Text scoreText;

    public GameObject comboUI;
    private int combo;
    private Text comboText;
    private Animator comboAnimator;
    public int maxCombo;

    public enum judges { NONE = 0, BAD, GOOD, PERFECT, MISS };

    public GameObject judgeUI;
    private Sprite[] judgeSprites;
    private Image judgementSpriteRenderer;
    private Animator judgementSpriteAnimator;

    public GameObject[] trails;
    private SpriteRenderer[] trailSpriteRenderers;

    // Music variables
    private AudioSource audioSource;
    private float clapCool = 0.1f;
    private bool[] ClapCooldowns;

    // Auto mode
    public bool automode;

    // Record mode
    private bool recordmode = false;
    public GameObject noteController;
    public GameObject recordManager;
    private bool[] BeatCooldowns;


    void MusicStart()
    {
        AudioClip audioClip = Resources.Load<AudioClip>("Beats/" + PlayerInformation.selectedMusic);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = 0.0f;
        audioSource.Play();
    }

    IEnumerator AwaitMusicStart(float time)
    {
        yield return new WaitForSeconds(time);
        MusicStart();
    }

    void Start()
    {
        if (recordmode)
        {
            noteController.gameObject.SetActive(false);

            BeatCooldowns = new bool[4];
            for (int i = 0; i < 4; i++) BeatCooldowns[i] = true;
        }
        else
        {
            recordManager.gameObject.SetActive(false);

            perfectScore = 1000000 / PlayerInformation.beats;

            judgementSpriteRenderer = judgeUI.GetComponent<Image>();
            judgementSpriteAnimator = judgeUI.GetComponent<Animator>();
            scoreText = scoreUI.GetComponent<Text>();
            comboText = comboUI.GetComponent<Text>();
            comboAnimator = comboUI.GetComponent<Animator>();

            // Sprite initializing
            judgeSprites = new Sprite[4];
            judgeSprites[0] = Resources.Load<Sprite>("Sprites/Bad");
            judgeSprites[1] = Resources.Load<Sprite>("Sprites/Good");
            judgeSprites[2] = Resources.Load<Sprite>("Sprites/Miss");
            judgeSprites[3] = Resources.Load<Sprite>("Sprites/Perfect");
        }

        StartCoroutine(AwaitMusicStart(2.0f));

        trailSpriteRenderers = new SpriteRenderer[trails.Length];
        for (int i = 0; i < trails.Length; i++)
        {
            trailSpriteRenderers[i] = trails[i].GetComponent<SpriteRenderer>();
        }

        //ClapSound Cooldown
        ClapCooldowns = new bool[4];
        for (int i = 0; i < 4; i++) ClapCooldowns[i] = true;       
    }

    private float time = PlayerInformation.beatInterval;

    void Update()
    {
        if (Input.GetKey(KeyCode.S)) ShineTrail(0); 
        if (Input.GetKey(KeyCode.D)) ShineTrail(1); 
        if (Input.GetKey(KeyCode.L)) ShineTrail(2);
        if (Input.GetKey(KeyCode.Semicolon)) ShineTrail(3);

        for (int i = 0; i < trailSpriteRenderers.Length; i++)
        {
            Color color = trailSpriteRenderers[i].color;
            color.a -= 0.01f;
            trailSpriteRenderers[i].color = color;
        }
    }

    public void ShineTrail(int idx)
    {
        Color color = trailSpriteRenderers[idx].color;
        color.a = 0.32f;
        trailSpriteRenderers[idx].color = color;

        if (recordmode && BeatCooldowns[idx])
        {
            PlayerInformation.noteRecord[idx].Add(PlayerInformation.order);
            StartCoroutine(beatCoolTime(idx));
        }

        //clap
        if (ClapCooldowns[idx])
        {
            KeySoundManager.instance.audioSource.Play();
            StartCoroutine(clapCoolTime(clapCool, idx));
        }
    }

    IEnumerator clapCoolTime(float time, int idx)
    {
        ClapCooldowns[idx] = false;
        yield return new WaitForSeconds(time);
        ClapCooldowns[idx] = true;
    }

    IEnumerator beatCoolTime(int idx)
    {
        BeatCooldowns[idx] = false;
        yield return new WaitForSeconds(PlayerInformation.beatInterval);
        BeatCooldowns[idx] = true;
    }

    // Judgement visualizing
    void showJudgement()
    {
        string scoreFormat = "000000";
        scoreText.text = score.ToString(scoreFormat);

        judgementSpriteAnimator.SetTrigger("Show");

        if (combo >= 2)
        {
            comboText.text = "COMBO " + combo.ToString();
            comboAnimator.SetTrigger("Show");
        }
        if (maxCombo < combo)
        {
            maxCombo = combo;
        }
    }

    // Note judgement
    private float perfectScore;
    public void processJudge(judges judge)
    {
        if (judge == judges.NONE) return;
        if (judge == judges.MISS)
        {
            judgementSpriteRenderer.sprite = judgeSprites[2];
            combo = 0;
            PlayerInformation.miss++;
        }

        else if (judge == judges.BAD)
        {
            judgementSpriteRenderer.sprite = judgeSprites[0];
            combo = 0;
            score += perfectScore * 0.25f;
            PlayerInformation.bad++;
        }

        else 
        {
            if (judge == judges.PERFECT)
            {
                judgementSpriteRenderer.sprite = judgeSprites[3];
                score += perfectScore;
                PlayerInformation.perfect++;
            }
            else if (judge == judges.GOOD)
            {
                judgementSpriteRenderer.sprite = judgeSprites[1];
                score += perfectScore * 0.75f;
                PlayerInformation.good++;
            }
            combo += 1;
        }
        showJudgement();
    }
}

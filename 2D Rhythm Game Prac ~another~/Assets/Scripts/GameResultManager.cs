using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class GameResultManager : MonoBehaviour
{
    public Text musicTitleUI;
    public Text scoreUI;
    public Text maxComboUI;
    public Image RankUI;

    public Text[] textType;
    public GameObject[] spriteType;
    private SpriteRenderer[] spriteTypeRenderers;
    private float alpha = 0.0f;
    private int i = 0;

    void Start()
    {
        textType[0].text = "" + PlayerInformation.perfect;
        textType[1].text = "" + PlayerInformation.good;
        textType[2].text = "" + PlayerInformation.bad;
        textType[3].text = "" + PlayerInformation.miss;

        spriteTypeRenderers = new SpriteRenderer[spriteType.Length];
        for (int j = 0; j < spriteType.Length; j++)
        {
            spriteTypeRenderers[j] = spriteType[j].GetComponent<SpriteRenderer>();
        }

        musicTitleUI.text = PlayerInformation.musicTitle;
        scoreUI.text = "Score: " + (int) PlayerInformation.score;
        maxComboUI.text = "Max Combo: " + PlayerInformation.maxCombo;        

        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + PlayerInformation.selectedMusic);
        StringReader reader = new StringReader(textAsset.text);

        reader.ReadLine();
        reader.ReadLine();

        string beatInformation = reader.ReadLine();
        int scoreS = 950000;
        int scoreA = 900000;
        int scoreB = 800000;

        if (PlayerInformation.score >= scoreS)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank S");
        }
        else if (PlayerInformation.score >= scoreA)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank A");
        }
        else if (PlayerInformation.score >= scoreB)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank B");
        }
        else 
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank C");
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene("SongSelectScene");
    }

    private void Update()
    {
        if (alpha < 0.32f && i == 0)
        {
            spriteTypeRenderers[0].color = new Color(1, 1, 1, alpha);
        }

        if (i < 4)
        {
            if (alpha < 1.0f)
            {
                textType[i].color = new Color(0.2f, 0.2f, 0.2f, alpha);
                spriteTypeRenderers[i + 1].color = new Color(1, 1, 1, alpha);
                alpha += 0.005f;
            }
            else
            {
                alpha = 0.0f;
                i++;
            } 
        }
        else if (i == 4)
        {
            if (alpha < 1.0f)
            {
                textType[4].color = new Color(1, 1, 1, alpha);
                textType[5].color = new Color(1, 1, 1, alpha);
                RankUI.color = new Color(1, 1, 1, alpha);
                alpha += 0.005f;
            }
            else
            {
                alpha = 0.0f;
                i++;
            }
        }
        else if (i == 5)
        {
            if (alpha < 1.0f)
            {
                textType[6].color = new Color(1, 1, 1, alpha);
                alpha += 0.005f;
            }
            else
            {
                alpha = 0.0f;
                i++;
            }
        }
    }
}

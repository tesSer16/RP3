using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    public int noteType;
    public GameManager.judges judge;
    private KeyCode keyCode;


    void Start()
    {
        if (noteType == 1) keyCode = KeyCode.S;
        else if (noteType == 2) keyCode = KeyCode.D;
        else if (noteType == 3) keyCode = KeyCode.L;
        else if (noteType == 4) keyCode = KeyCode.Semicolon;
    }
    
    public void Initialize()
    {
        judge = GameManager.judges.NONE;
    }

    void Update()
    {
        transform.Translate(Vector3.down * GameManager.instance.noteSpeed * Time.deltaTime);

        judgement();
        // keyboard debug
        if (Input.GetKey(keyCode))
        {
            GameManager.instance.processJudge(judge);
            if (judge != GameManager.judges.NONE)
            {
                pop();
                gameObject.SetActive(false);
            }
        }
    }

    private void pop()
    {
        PlayerInformation.noteOrders[noteType - 1].Dequeue();
    }

    private void judgement()
    {
        int judgeValue = PlayerInformation.order - PlayerInformation.noteOrders[noteType - 1].Peek();
        if (judgeValue == 0)
        {
            judge = GameManager.judges.PERFECT;
            if (GameManager.instance.automode)
            {
                PlayerInformation.test.Add(gameObject.transform.position.y);
                KeySoundManager.instance.audioSource.Play();
                GameManager.instance.processJudge(judge);
                pop();
                gameObject.SetActive(false);
            }
        }
        else if (Mathf.Abs(judgeValue) <= 1)
            judge = GameManager.judges.GOOD;
        else if (Mathf.Abs(judgeValue) <= 2) 
            judge = GameManager.judges.BAD;
        else if (judgeValue >= 3)
        {
            judge = GameManager.judges.MISS;
            GameManager.instance.processJudge(judge);
            pop();
            gameObject.SetActive(false);
        }
    }
}

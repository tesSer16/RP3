using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    public int noteType;
    public int trail;
    public GameManager.judges judge;

    public void Initialize()
    {
        judge = GameManager.judges.NONE;
    }
    public void judgement()
    {
        int judgeValue = Info.order - Info.Chart[trail].Peek();
        if (judgeValue == 0)
        {
            judge = GameManager.judges.PERFECT;
            if (GameManager.instance.automode)
            {
                // KeySoundManager.instance.audioSource.Play();
                GameManager.instance.processJudge(judge);
                Info.Chart[trail].Dequeue(); // record?
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
            Info.Chart[trail].Dequeue(); // record?
            gameObject.SetActive(false);
        }
    }

    public void Judge()
    {
        GameManager.instance.processJudge(judge);
        if (judge != GameManager.judges.NONE) gameObject.SetActive(false);
    }

    void Update()
    {
        transform.Translate(Vector3.down * GameManager.instance.noteSpeed);
        judgement();

    }
}

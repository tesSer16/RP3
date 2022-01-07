using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    public GameManager.judges judge;

    public void Initialize()
    {
        judge = GameManager.judges.NONE;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Superfect")
        {
            if (GameManager.instance.automode)
            {
                judge = GameManager.judges.PERFECT;
                //KeySoundManager.instance.audioSource.Play();
                GameManager.instance.processJudge(judge);
                gameObject.SetActive(false);
            }
        }
        else if (other.tag == "Perfect")
        {
            judge = GameManager.judges.PERFECT;
            //Debug.Log(other.tag);
        }
        else if (other.tag == "Good")
        {
            //Debug.Log(other.tag);
            judge = GameManager.judges.GOOD;
        }            
        else if (other.tag == "Bad")
        {
            //Debug.Log(other.tag);
            judge = GameManager.judges.BAD;
        }            
        else if (other.tag == "Miss")
        {
            judge = GameManager.judges.MISS;
            //Debug.Log(other.tag);
            GameManager.instance.processJudge(judge);
            gameObject.SetActive(false);
        }
    }

    public void Judge()
    {
        GameManager.instance.processJudge(judge);
        if (judge != GameManager.judges.NONE)
        {
            gameObject.SetActive(false);
        } 
            
    }

    void Update()
    {
        if (NoteController.audioSource.isPlaying)
            transform.Translate(Vector3.down * GameManager.instance.noteSpeed * Time.deltaTime * 10);
    }
}

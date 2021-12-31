using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public void SelectSong()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
    }
}

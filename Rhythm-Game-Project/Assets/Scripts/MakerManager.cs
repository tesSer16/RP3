using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakerManager : MonoBehaviour
{
    public Camera cam3d, cam2d;
    public GameObject progressBar;
    public GameObject ccButton;
    public GameObject pButton;

    void Start()
    {
        cam3d.enabled = true;
        cam2d.enabled = false;
    }

    public void ChangeCamera()
    {
        if (cam3d.enabled)
        {
            // 3d --> 2d
            cam3d.enabled = false;
            cam2d.enabled = true;
            progressBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(400, 520, 0);
            ccButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(160, 400, 0);
            pButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 400, 0);
            ccButton.GetComponentInChildren<Text>().text = "3D";
        }
        else
        {
            // 2d --> 3d
            cam3d.enabled = true;
            cam2d.enabled = false;
            progressBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 520, 0);
            ccButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(780, 520, 0);
            pButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(620, 520, 0);
            ccButton.GetComponentInChildren<Text>().text = "2D";
        }
    }
}

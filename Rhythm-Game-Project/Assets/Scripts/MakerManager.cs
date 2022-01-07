using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakerManager : MonoBehaviour
{
    // UI & scene variable
    public static Camera cam3d, cam2d;
    public Slider progressBar;
    public Button ccButton;
    public Button pButton;

    // Maker variable
    public GameObject[] trails;
    private Renderer[] trailRenderers;

    void Start()
    {
        cam2d = GameObject.Find("2D Camera").GetComponent<Camera>();
        cam3d = GameObject.Find("Main Camera").GetComponent<Camera>();

        // 카메라 초기화
        cam3d.enabled = true;
        cam2d.enabled = false;

        // trail 불러오기
        trailRenderers = new Renderer[trails.Length];
        for (int i = 0; i < trails.Length; i++)
        {
            trailRenderers[i] = trails[i].GetComponent<Renderer>();
        }
    }

    private void Update()
    {
        // touch detect
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch tempTouch = Input.GetTouch(i);
                if (tempTouch.phase == TouchPhase.Began)
                {
                    Ray ray;
                    if (cam3d.enabled)
                        ray = cam3d.ScreenPointToRay(Input.mousePosition);
                    else
                        ray = cam2d.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.name == "Trail 1")
                        {
                            ShineMake(0);
                        }
                        if (hit.collider.name == "Trail 2")
                        {
                            ShineMake(1);
                        }
                        if (hit.collider.name == "Trail 3")
                        {
                            ShineMake(2);
                        }
                        if (hit.collider.name == "Trail 4")
                        {
                            ShineMake(3);
                        }
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.S)) ShineMake(0);
        if (Input.GetKey(KeyCode.D)) ShineMake(1);
        if (Input.GetKey(KeyCode.L)) ShineMake(2);
        if (Input.GetKey(KeyCode.Semicolon)) ShineMake(3);

        for (int i = 0; i < trailRenderers.Length; i++)
        {
            Color color = trailRenderers[i].material.color;
            if (color.a > 0) color.a -= 0.01f;
            trailRenderers[i].material.color = color;
        }
    }

    public ChartController chartController;
    public void ShineMake(int trail)
    {
        Color color = trailRenderers[trail].material.color;
        color.a = 0.32f;
        trailRenderers[trail].material.color = color;
        chartController.MakeNote(trail);
    }

    public void ChangeCamera()
    {
        if (cam3d.enabled)
        {
            // 3d --> 2d
            cam3d.enabled = false;
            cam2d.enabled = true;
            progressBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(400, 520, 0);
            ccButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(160, 380, 0);
            pButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 380, 0);
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

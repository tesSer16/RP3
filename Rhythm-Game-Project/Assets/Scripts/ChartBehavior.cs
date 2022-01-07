using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartBehavior : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    public int _trail, order;
    private int interval, trail, level;

    private void OnMouseDown()
    {
        if (MakerManager.cam2d.enabled && !ChartController.audioSource.isPlaying)
        {
            mZCoord = MakerManager.cam2d.WorldToScreenPoint(transform.position).z;
            mOffset = transform.position - GetMouseAsWorldPoint();
            interval = ChartController.chartInterval;

            ChartController.clicked = true;
        }
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return MakerManager.cam2d.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        if (MakerManager.cam2d.enabled && !ChartController.audioSource.isPlaying)
        {
            Vector3 objectPos = GetMouseAsWorldPoint() + mOffset;

            trail = Mathf.Clamp((int)(objectPos.x + 15.0f) / 10, 0, 3);
            level = Mathf.Clamp((int)((objectPos.y - 63.7f) / 110 * interval), 0, (int) (interval * 0.6f));
            
            objectPos.x = -15.0f + 10.0f * trail;
            objectPos.y = 63.7f + 110.0f * level / interval;
            objectPos.z = 49.4f;
            transform.position = objectPos;
        }
    }

    private void OnMouseUp()
    {
        if (MakerManager.cam2d.enabled && !ChartController.audioSource.isPlaying)
        {
            ChartController.EditNote(_trail, order, trail, level);
            ChartController.clicked = false;
        }
            
    }
}

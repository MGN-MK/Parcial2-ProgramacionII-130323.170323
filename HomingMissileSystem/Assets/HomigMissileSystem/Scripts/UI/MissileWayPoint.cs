using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissileWayPoint : MonoBehaviour
{
    public Image icon;
    public Transform missile;
    public TextMeshProUGUI meter;
    public Vector3 offset;

    private void FixedUpdate()
    {
        float minX = icon.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;
        float minY = icon.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position + offset);

        if(Vector3.Dot((missile.position - transform.position), transform.forward) < 0)
        {
            if(pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        icon.transform.position = pos;
        meter.text = ((int)Vector3.Distance(missile.position, transform.position)).ToString() + " m";
    }
}

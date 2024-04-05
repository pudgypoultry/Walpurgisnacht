using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLineRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lr;
    private Transform[] points;
    private bool ready = false;

    public bool Ready { get => ready; set => ready = value; }

    public void SetUpLine(Transform[] linePoints)
    {
        lr.positionCount = linePoints.Length;
        points = linePoints;
        ready = true;

    }

    private void Update()
    {
        if (ready)
        {
            lr.enabled = true;
            // Debug.Log(points[0]);
            // Debug.Log(points[1]);
            // Debug.Log("============");
            for (int i = 0; i < points.Length; i++)
            {
                lr.SetPosition(i, points[i].position);
            }
        }
        else
        {
            lr.enabled = false;
        }
    }

    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}

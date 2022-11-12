using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorCart : MonoBehaviour
{
    TrackPoint nextAim;

    private void Start()
    {
        enabled = false;
    }

    public void StartRunning(TrackPoint startPoint)
    {
        MoveTowards(startPoint.GetNextNode(null));
    }

    private void Update()
    {

    }
    private void MoveTowards(TrackPoint nextAim)
    {
        this.nextAim = nextAim;
        enabled = true;
    }
}

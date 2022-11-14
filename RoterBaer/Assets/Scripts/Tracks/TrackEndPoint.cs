using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackEndPoint : TrackPoint
{
    public override ETrackType TrackType => ETrackType.End;

    protected override bool HasValidNeighborCount => neighbors.Count == 1;

    [SerializeField] bool startPoint;
    public override bool IsNeverTarget => startPoint;
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(TrackEndPoint))]
public class TrackEndPointEditor : TrackPointEditor
{
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    public static TrackController Active { get; private set; }

    [SerializeField] TrackPoint startPoint;
    public TrackPoint StartPoint => startPoint;

    [SerializeField] TrackPoint endPoint;
    public TrackPoint EndPoint => endPoint;

    List<TrackPoint> allTrackPoints = new List<TrackPoint>();

    int trackpointIdentifier = 0;

    private void Awake()
    {
        if (Active != null && Active != this)
            Debug.LogError($"[{GetType().Name}] More than one Controller active!");

        Active = this;
    }

    private void Start()
    {
        DrawLineTrackNetwork();
    }

    public bool TryAddTrackpoint(TrackPoint newPoint, out int identifier)
    {
        if (allTrackPoints.Contains(newPoint))
        {
            identifier = -1;
            return false;
        }

        allTrackPoints.Add(newPoint);

        identifier = ++trackpointIdentifier;

        return true;
    }

    public void DrawLineTrackNetwork()
    {
        foreach (var trackPoint in allTrackPoints)
            trackPoint.DrawLineToNeighbors(10f);
    }
}


/// <summary>
/// provides Button to clear all stored data
/// </summary>
[UnityEditor.CustomEditor(typeof(TrackController))]
public class TrackControllerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        if (Application.isPlaying && GUILayout.Button("DrawLineTrackNetwork"))
            ((TrackController)target).DrawLineTrackNetwork();

        GUILayout.Space(5);

        base.OnInspectorGUI();
    }
}

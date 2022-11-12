using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackPoint : MonoBehaviour
{
    int identifier;

    [SerializeField] protected List<TrackPoint> neighbors = new List<TrackPoint>();

    public Vector3 Position => transform.position;

    public virtual ETrackType TrackType => ETrackType.Point;

    public virtual bool IsNeverTarget => false;

    protected virtual void Awake()
    {
        CheckProperNeighbors();

        TrackController.Active.TryAddTrackpoint(this, out identifier);
    }

    protected virtual bool HasValidNeighborCount => neighbors.Count == 2;


    public virtual TrackPoint GetNextNode(TrackPoint previousPoint)
    {
        if (previousPoint == null || IsValidPreviousPoint(previousPoint))
        {
            foreach (var point in neighbors)
            {
                if (point != previousPoint) return point;
            }
        }

        return previousPoint;
    }

    protected bool IsValidPreviousPoint(TrackPoint previousPoint)
    {
        if (previousPoint != this && !IsNeighbor(previousPoint))
        {
            Debug.LogError($"[{GetType().Name}] Unexpected Next Point request for '{this.gameObject.name}' with '{previousPoint.gameObject.name}' as orgin.", previousPoint);
            return false;
        }
        return true;
    }

    public bool IsNeighbor(TrackPoint point) => neighbors.Contains(point);

    public void DrawLineToNeighbors(float duration)
    {
        foreach (var trackPoint in neighbors)
        {
            if (trackPoint.identifier < identifier) continue;

            Debug.DrawLine(transform.position, trackPoint.transform.position, Color.white, duration);
        }
    }

    #region Development
    public void CheckProperNeighbors()
    {
        List<TrackPoint> validList = new List<TrackPoint>(neighbors.Where((x) => x != null && x != this));

        if (validList.Count != neighbors.Count)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "Removed self reference and/or null value from neighbors.");
#endif
            neighbors = validList;
        }

        if (!HasValidNeighborCount)
        {
            if (TrackType == ETrackType.Gate && neighbors.Count > 1)
                Debug.LogWarning($"[{GetType().Name}] {gameObject.name} has unexpected amount of neighbors: {neighbors.Count}.", this);
            else
                Debug.LogError($"[{GetType().Name}] {gameObject.name} has unexpected amount of neighbors: {neighbors.Count}.", this);
        }

        foreach (var neighbor in neighbors)
        {
            if (!neighbor.IsNeighbor(this))
                Debug.LogError($"[{GetType().Name}] {gameObject.name} is neighbor of '{neighbor.gameObject.name}', but not the other way around.", this);
        }
    }

    public void AddMissingNeighbors()
    {
        foreach (var trackPoint in neighbors)
            trackPoint.AddNeighbor(this);
    }

    public void RemoveMissingNeighbors()
    {
        List<TrackPoint> dangling = new List<TrackPoint>(neighbors.Where((x) => x == null || !x.IsNeighbor(this)));

        foreach (var trackPoint in dangling)
            RemoveNeighbor(trackPoint);
    }

    public void AddNeighbor(TrackPoint point)
    {
        if (IsNeighbor(point)) return;

#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(this, "Added missing neighbor");
#endif
        neighbors.Add(point);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    public void RemoveNeighbor(TrackPoint point)
    {
        if (!IsNeighbor(point)) return;

#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(this, "Removed missing neighbor");
#endif
        neighbors.Remove(point);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
    #endregion Development
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(TrackPoint))]
public class TrackPointEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Draw Track Network"))
        {
            foreach (var trackPoint in FindObjectsOfType<TrackPoint>())
                trackPoint.DrawLineToNeighbors(40f);
        }
        if (GUILayout.Button("Check Neighbors"))
        {
            foreach (var trackPoint in FindObjectsOfType<TrackPoint>())
                trackPoint.CheckProperNeighbors();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Add missing Neighbors"))
        {
            foreach (var trackPoint in FindObjectsOfType<TrackPoint>())
                trackPoint.AddMissingNeighbors();
        }
        if (GUILayout.Button("Remove missing Neighbors"))
        {
            foreach (var trackPoint in FindObjectsOfType<TrackPoint>())
                trackPoint.RemoveMissingNeighbors();
        }

        GUILayout.Space(5);

        base.OnInspectorGUI();
    }
}
#endif
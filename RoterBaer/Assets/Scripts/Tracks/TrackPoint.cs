using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackPoint : MonoBehaviour
{
    int identifier;

    [SerializeField] List<TrackPoint> neighbors = new List<TrackPoint>();

    protected void Awake()
    {
        if (neighbors.Where((x) => x != null).Count() < 2)
            Debug.LogError($"[{GetType().Name}] Track point '{name}' is missing neighbors!", this);

        TrackController.Active.TryAddTrackpoint(this, out identifier);
    }

    public void DrawLineToNeighbors(float duration)
    {
        foreach (var trackPoint in neighbors)
        {
            if (trackPoint.identifier < identifier) continue;

            Debug.DrawLine(transform.position, trackPoint.transform.position, Color.white, duration);
        }
    }

    public void AddMissingNeighbors()
    {
        foreach (var trackPoint in neighbors)
            trackPoint.AddNeighbor(this);
    }

    public void RemoveMissingNeighbors()
    {
        foreach (var trackPoint in neighbors.Where((x) => !x.IsNeighbor(this)))
            trackPoint.RemoveNeighbor(this);
    }

    public bool IsNeighbor(TrackPoint point) => neighbors.Contains(point);

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
                trackPoint.DrawLineToNeighbors(10f);
        }
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
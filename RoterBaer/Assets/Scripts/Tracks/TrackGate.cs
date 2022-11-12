using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackGate : TrackPoint
{
    [SerializeField] ETrackGateType gateType;

    [SerializeField] TrackPoint nextExit;

    protected override void Awake()
    {
        base.Awake();

        if (nextExit == null && gateType == ETrackGateType.Fixed)
        {
            Debug.LogError($"[{GetType().Name}] Fixed Gate '{gameObject.name}' with undefined Exit.", this);

            if (neighbors.Count > 0) nextExit = neighbors.First();
        }
    }

    public override TrackPoint GetNextNode(TrackPoint previousPoint)
    {
        if (previousPoint != null && !IsValidPreviousPoint(previousPoint))
            return previousPoint;

        switch (gateType)
        {
            case ETrackGateType.Iterating:
                TrackPoint oldNextPoint = nextExit;
                SetNextExit(countUp: true);
                return oldNextPoint;

            case ETrackGateType.Fixed:
            case ETrackGateType.Player1:
            case ETrackGateType.Player2:
                break;

            default:
                Debug.Log($"[{GetType().Name}] {nameof(GetNextNode)} is UNDEFINED for {gateType}", this);
                break;
        }
        return nextExit;
    }

    public void SetNextExit(bool countUp)
    {
        int nextID = 0;
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] == nextExit)
            {
                nextID = i;
                break;
            }
        }

        nextID = (nextID + neighbors.Count + (countUp ? 1 : -1)) % neighbors.Count;

        nextExit = neighbors[nextID];
    }
}

/// <summary>
/// provides Button to clear all stored data
/// </summary>
[UnityEditor.CustomEditor(typeof(TrackGate))]
public class TrackGateEditor : TrackPointEditor
{
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackGate : TrackPoint
{
    public System.Action NextExitChangedEvent;

    [SerializeField] List<TrackPoint> blockedDirections = new List<TrackPoint>();

    [SerializeField] ETrackGateType gateType;

    [SerializeField] TrackPoint nextExit;
    public TrackPoint NextExit => nextExit;

    public override ETrackType TrackType => ETrackType.Gate;

    protected override void Awake()
    {
        base.Awake();

        if (nextExit == null)
        {
            if (gateType == ETrackGateType.Fixed)
                Debug.LogError($"[{GetType().Name}] Fixed Gate '{gameObject.name}' with undefined Exit.", this);

            if (neighbors.Count > 0)
            {
                nextExit = neighbors.Where((x) => !x.IsNeverTarget).First();
            }
        }

        NextExitChangedEvent?.Invoke();
    }

    protected override bool HasValidNeighborCount => neighbors.Count >= 3;

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
        int oldID = 0;
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] == nextExit)
            {
                oldID = i;
                break;
            }
        }

        int nextID = oldID;
        for (int i = 1; i < neighbors.Count; i++)
        {
            nextID = (oldID + neighbors.Count + (countUp ? i : -i)) % neighbors.Count;

            if (!neighbors[nextID].IsNeverTarget && !blockedDirections.Contains(neighbors[nextID]))
                break;
        }

        if (nextID != oldID)
        {
            nextExit = neighbors[nextID];
            NextExitChangedEvent?.Invoke();

            if(gateType == ETrackGateType.Iterating)
                AudioManager.instance.SwitchAuto(transform.position);
            else if(gateType == ETrackGateType.Player1 || gateType == ETrackGateType.Player2)
                AudioManager.instance.SwitchPlayer(transform.position);
        }
        else
            Debug.LogWarning($"[{GetType().Name}] Gate couldn't be set different than old value of {neighbors[nextID]}", this);
    }

    public override void CheckProperNeighbors()
    {
        base.CheckProperNeighbors();

        bool atLeastOnePossible = false;

        foreach (var neighbor in neighbors)
        {
            if (!neighbor.IsNeverTarget && !blockedDirections.Contains(neighbor))
            {
                atLeastOnePossible = true;
                break;
            }
        }

        if (!atLeastOnePossible)
            Debug.LogError($"[{GetType().Name}] {gameObject.name} has no exit it might send Carts towards.", this);

        if (!neighbors.Contains(nextExit))
            Debug.LogError($"[{GetType().Name}] {gameObject.name} {nameof(nextExit)} is set to a Node thats not it's neighbor.", this);
    }
}

#if UNITY_EDITOR
/// <summary>
/// provides Button to clear all stored data
/// </summary>
[UnityEditor.CustomEditor(typeof(TrackGate))]
public class TrackGateEditor : TrackPointEditor
{
}
#endif

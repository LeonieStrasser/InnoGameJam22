using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGate : TrackPoint
{
    [SerializeField] ETrackGateType gateType;


}

/// <summary>
/// provides Button to clear all stored data
/// </summary>
[UnityEditor.CustomEditor(typeof(TrackGate))]
public class TrackGateEditor : TrackPointEditor
{
}
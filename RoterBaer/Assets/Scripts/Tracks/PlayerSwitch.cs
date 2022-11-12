using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrackGate))]
public class PlayerSwitch : MonoBehaviour
{
    TrackGate myGate;
    private void Awake()
    {
        myGate = GetComponent<TrackGate>();   
    }

    private void OnMouseDown()
    {
        Debug.Log("Gate klick!");
        myGate.SetNextExit(true);
    }
}

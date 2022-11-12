using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrackGate))]
public class PlayerSwitch : MonoBehaviour
{
    TrackGate myGate;

    bool isHovered;

    private void Awake()
    {
        myGate = GetComponent<TrackGate>();   
    }

    private void OnMouseDown()
    {
        Debug.Log("Gate klick!");
        myGate.SetNextExit(true);
    }

    private void OnMouseEnter()
    {
        SetHovered(true);
    }

    private void OnMouseExit()
    {
        SetHovered(false);
    }

    private void SetHovered(bool isHovered)
    {
        if (this.isHovered == isHovered) return;

        this.isHovered = isHovered;

        if (isHovered)
            transform.localScale *= 1.1f;
        else
            transform.localScale /= 1.1f;
    }
}

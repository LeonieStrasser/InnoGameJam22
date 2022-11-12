using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDirectionFeedback : MonoBehaviour
{
    [SerializeField] GameObject directionSprite;
    [SerializeField] float rotationSpeed;
    
    TrackGate myTrackGate;

    private void Awake()
    {
        myTrackGate = GetComponent<TrackGate>();
        myTrackGate.NextExitChangedEvent += RotateToNextExit;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnDestroy()
    {
        myTrackGate.NextExitChangedEvent -= RotateToNextExit;
    }

    private void Update()
    {
        RotateToNextExit();
    }

    void RotateToNextExit()
    {
        
        directionSprite.transform.LookAt(myTrackGate.NextExit.transform);
    }


}

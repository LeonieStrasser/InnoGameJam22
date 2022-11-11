using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorCartController : MonoBehaviour
{
    public static VisitorCartController Active { get; private set; }

    private  List<VisitorCart> activeCarts = new List<VisitorCart>() ;

    private void Awake()
    {
        if (Active != null && Active != this)
            Debug.LogError($"[{GetType().Name}] More than one Controller active!");

        Active = this;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

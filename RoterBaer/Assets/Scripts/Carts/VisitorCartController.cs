using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorCartController : MonoBehaviour
{
    public static VisitorCartController Active { get; private set; }

    private List<VisitorCart> activeCarts = new List<VisitorCart>();

    [SerializeField] VisitorCart prefabVisitorCart;

    [SerializeField] float timeBetweenCarts = 5f;

    private void Awake()
    {
        if (Active != null && Active != this)
            Debug.LogError($"[{GetType().Name}] More than one Controller active!");

        Active = this;

        InvokeRepeating("SpawnCart", 1f, 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnCart()
    {
        VisitorCart newCart = Instantiate(prefabVisitorCart, TrackController.Active.StartPoint.Position, Quaternion.identity);

        newCart.transform.SetParent(transform);
        newCart.StartRunning(TrackController.Active.StartPoint);
    }
}

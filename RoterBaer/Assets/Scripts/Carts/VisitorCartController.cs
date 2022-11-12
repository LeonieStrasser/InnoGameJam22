using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorCartController : MonoBehaviour
{
    public static VisitorCartController Active { get; private set; }

    private List<VisitorCart> activeCarts = new List<VisitorCart>();

    [SerializeField] float timeBetweenCarts = 5f;

    [SerializeField, Min(1)] int cartLimit = 10;

    [Header("Prefabs")]
    [SerializeField] VisitorCart prefabVisitorCart;
    [SerializeField] Passenger[] prefabPassenger;

    float elapsed = 0;
    private void Awake()
    {
        if (Active != null && Active != this)
            Debug.LogError($"[{GetType().Name}] More than one Controller active!");

        Active = this;
    }

    private void OnDestroy()
    {
        AudioManager.instance.StopAllEnvEmitters();
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= timeBetweenCarts)
        {
            SpawnCart();
            elapsed = 0;
        }
    }

    public void SpawnCart()
    {
        if (activeCarts.Count >= cartLimit) return;

        VisitorCart newCart = Instantiate(prefabVisitorCart, TrackController.Active.StartPoint.Position, Quaternion.identity);

        newCart.transform.SetParent(transform);
        newCart.SetupCart(TrackController.Active.StartPoint, CreatePassengers(newCart.PassengerSeatCount));

        activeCarts.Add(newCart);
        AudioManager.instance.WagonInitialize(newCart.gameObject);
    }

    public void ReachedEnd(VisitorCart arrivingCart)
    {
        if (arrivingCart.TargetTrackPoint != TrackController.Active.EndPoint)
            Debug.LogWarning($"[{GetType().Name}] Cart {arrivingCart.gameObject.name} didn't arrive at expected End: {arrivingCart.TargetTrackPoint}", arrivingCart);

        activeCarts.Remove(arrivingCart);
        AudioManager.instance.WagonRetirement(arrivingCart.gameObject);

        Destroy(arrivingCart.gameObject);
    }

    public Passenger[] CreatePassengers(int passengerCount)
    {
        if (passengerCount < 1)
            Debug.LogError($"[{GetType().Name}] Got Request to create less than one passengers: {passengerCount}");

        Passenger[] result = new Passenger[passengerCount];
        for (int i = 0; i < passengerCount; i++)
        {
            result[i] = Instantiate(prefabPassenger[Random.Range(0, prefabPassenger.Length)], transform);
        }
        return result;
    }
}


/// <summary>
/// provides Button to clear all stored data
/// </summary>
[UnityEditor.CustomEditor(typeof(VisitorCartController))]
public class VisitorCartControllerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Spawn Cart"))
        {
            ((VisitorCartController)target).SpawnCart();
        }

        base.OnInspectorGUI();
    }
}

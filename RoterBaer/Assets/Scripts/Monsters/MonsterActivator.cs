using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MonsterActivator : MonoBehaviour
{
    [SerializeField] KeyCode activateInput;

    List<GameObject> cartsInRange;
    Animator anim;

    private void Awake()
    {
        cartsInRange = new List<GameObject>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(activateInput))
        {
            ActivateScare();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        cartsInRange.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (cartsInRange.Find(i => other.gameObject))
        {
            cartsInRange.Remove(other.gameObject);
        }

    }

    private void ActivateScare()
    {
        anim.SetTrigger("scare");
    }
}

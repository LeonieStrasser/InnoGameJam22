using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MonsterActivator : MonoBehaviour
{
    [SerializeField] EMonsterType myType;
    public EMonsterType MyType
    {
        get
        {
            return myType;
        }
    }

    [SerializeField] List<GameObject> cartsInRange;
    MonsterController monsterHUB;

    public Vector3 Position => transform.position;

    Animator anim;

    private void Awake()
    {
        monsterHUB = FindObjectOfType<MonsterController>();
        cartsInRange = new List<GameObject>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        monsterHUB.AddMonsterToLists(this);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cart"))
            cartsInRange.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cart") && cartsInRange.Find(i => other.gameObject))
        {
            cartsInRange.Remove(other.gameObject);
        }

    }

    public void ActivateScare()
    {
        anim.SetTrigger("scare");

        foreach (var cart in cartsInRange)
            cart.GetComponent<VisitorCart>()?.ScarePassengers(myType);

        AudioManager.instance.MonsterScare(this);
    }

    private void OnMouseDown()
    {
        monsterHUB.SetMonsterListScareActive(myType);
    }
}

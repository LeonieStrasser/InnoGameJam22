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
        cartsInRange.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (cartsInRange.Find(i => other.gameObject))
        {
            cartsInRange.Remove(other.gameObject);
        }

    }

    public void ActivateScare()
    {
        anim.SetTrigger("scare");
    }

    private void OnMouseDown()
    {
        monsterHUB.SetMonsterListScareActive(myType);
    }
}

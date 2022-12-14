using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MonsterActivator : MonoBehaviour
{
    [SerializeField] EMonsterType myType;
    public EMonsterType MyType => myType;

    [SerializeField] List<VisitorCart> cartsInRange;
    MonsterController monsterHUB;

    [Space(20)]
    [SerializeField] GameObject scareRangeVFX;
    [Header("For StartMenu")]
    [SerializeField] bool mutedIdle = false;

    public Vector3 Position => transform.position;

    Animator anim;

    bool isHovered;

    VisitorCart nextCart;

    private void Awake()
    {
        monsterHUB = FindObjectOfType<MonsterController>();
        cartsInRange = new List<VisitorCart>();
        anim = GetComponentInChildren<Animator>();

        if (!mutedIdle)
            AudioManager.instance.MonsterIdleInitialize(this);
    }
    private void Start()
    {
        monsterHUB.AddMonsterToLists(this);
    }

    private void OnDestroy()
    {
        if (!mutedIdle)
            AudioManager.instance.MonsterIdleRetirement(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cart"))
        {
            nextCart = other.GetComponent<VisitorCart>();
            cartsInRange.Add(nextCart);

            nextCart.IsScareEndangered(isHovered);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cart") && cartsInRange.Find(i => other.gameObject))
        {
            nextCart = other.GetComponent<VisitorCart>();
            cartsInRange.Remove(nextCart);

            nextCart.IsScareEndangered(false);
        }
    }

    public void ActivateScare()
    {
        anim.SetTrigger("scare");

        foreach (var cart in cartsInRange)
            cart.GetComponent<VisitorCart>()?.ScarePassengers(myType);
    }

    private void OnMouseDown()
    {
        monsterHUB.SetMonsterListScareActive(myType);
    }

    private void OnMouseEnter()
    {
        foreach (var monster in MonsterController.Active.GetMonsters(myType))
            monster.SetHovered(true);

    }

    private void OnMouseExit()
    {
        foreach (var monster in MonsterController.Active.GetMonsters(myType))
            monster.SetHovered(false);
    }

    private void SetHovered(bool isHovered)
    {
        if (this.isHovered == isHovered) return;

        this.isHovered = isHovered;

        if (isHovered)
        {
            transform.localScale *= 1.05f;
            scareRangeVFX?.SetActive(true);
        }
        else
        {
            transform.localScale /= 1.05f;
            scareRangeVFX?.SetActive(false);
        }

        foreach (var cart in cartsInRange)
            cart.IsScareEndangered(isHovered);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public static MonsterController Active { get; private set; }

    [SerializeField] KeyCode activateInputA;
    [SerializeField] KeyCode activateInputB;
    [SerializeField] KeyCode activateInputC;
    [SerializeField] KeyCode activateInputD;


    List<MonsterActivator> monstersA;
    List<MonsterActivator> monstersB;
    List<MonsterActivator> monstersC;
    List<MonsterActivator> monstersD;

    private void Awake()
    {
        Active = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        monstersA = new List<MonsterActivator>();
        monstersB = new List<MonsterActivator>();
        monstersC = new List<MonsterActivator>();
        monstersD = new List<MonsterActivator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(activateInputA))
        {
            SetMonsterListScareActive(EMonsterType.BONUSMONSTER);
        }
        if (Input.GetKeyDown(activateInputB))
        {
            SetMonsterListScareActive(EMonsterType.Benjee);
        }
        if (Input.GetKeyDown(activateInputC))
        {
            SetMonsterListScareActive(EMonsterType.Fritzi);
        }
        if (Input.GetKeyDown(activateInputD))
        {
            SetMonsterListScareActive(EMonsterType.Axtor);
        }
    }

    public void AddMonsterToLists(MonsterActivator newMonster)
    {
        switch (newMonster.MyType)
        {
            case EMonsterType.BONUSMONSTER:
                monstersA.Add(newMonster);
                break;
            case EMonsterType.Benjee:
                monstersB.Add(newMonster);
                break;
            case EMonsterType.Fritzi:
                monstersC.Add(newMonster);
                break;
            case EMonsterType.Axtor:
                monstersD.Add(newMonster);
                break;
            default:
                break;
        }
    }

    public void SetMonsterListScareActive(EMonsterType monsterType)
    {
        switch (monsterType)
        {
            case EMonsterType.BONUSMONSTER:
                foreach (var item in monstersA)
                {
                    item.ActivateScare();
                }
                break;
            case EMonsterType.Benjee:
                foreach (var item in monstersB)
                {
                    item.ActivateScare();
                }
                break;
            case EMonsterType.Fritzi:
                foreach (var item in monstersC)
                {
                    item.ActivateScare();
                }
                break;
            case EMonsterType.Axtor:
                foreach (var item in monstersD)
                {
                    item.ActivateScare();
                }
                break;
            default:
                break;
        }
    }

    public List<MonsterActivator> GetMonsters(EMonsterType monsterType)
    {
        switch (monsterType)
        {
            case EMonsterType.BONUSMONSTER:
                return monstersA;
            case EMonsterType.Benjee:
                return monstersB;
            case EMonsterType.Fritzi:
                return monstersC;
            case EMonsterType.Axtor:
                return monstersD;

            default:
                Debug.LogError($"[{GetType().Name}] {nameof(GetMonsters)} UNDEFINED for {monsterType}.", this);
                return null;
        }
    }
}

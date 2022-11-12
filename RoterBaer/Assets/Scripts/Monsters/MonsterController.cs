using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] KeyCode activateInputA;
    [SerializeField] KeyCode activateInputB;
    [SerializeField] KeyCode activateInputC;
    [SerializeField] KeyCode activateInputD;


    List<MonsterActivator> monstersA;
    List<MonsterActivator> monstersB;
    List<MonsterActivator> monstersC;
    List<MonsterActivator> monstersD;


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
            SetMonsterListScareActive(EMonsterType.MonsterA);
        }
        if (Input.GetKeyDown(activateInputB))
        {
            SetMonsterListScareActive(EMonsterType.MonsterB);
        }
        if (Input.GetKeyDown(activateInputC))
        {
            SetMonsterListScareActive(EMonsterType.MonsterC);
        }
        if (Input.GetKeyDown(activateInputD))
        {
            SetMonsterListScareActive(EMonsterType.MonsterD);
        }
    }

    public void AddMonsterToLists(MonsterActivator newMonster)
    {
        switch (newMonster.MyType)
        {
            case EMonsterType.MonsterA:
                monstersA.Add(newMonster);
                break;
            case EMonsterType.MonsterB:
                monstersB.Add(newMonster);
                break;
            case EMonsterType.MonsterC:
                monstersC.Add(newMonster);
                break;
            case EMonsterType.MonsterD:
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
            case EMonsterType.MonsterA:
                foreach (var item in monstersA)
                {
                    item.ActivateScare();
                }
                break;
            case EMonsterType.MonsterB:
                foreach (var item in monstersB)
                {
                    item.ActivateScare();
                }
                break;
            case EMonsterType.MonsterC:
                foreach (var item in monstersC)
                {
                    item.ActivateScare();
                }
                break;
            case EMonsterType.MonsterD:
                foreach (var item in monstersD)
                {
                    item.ActivateScare();
                }
                break;
            default:
                break;
        }
    }
}

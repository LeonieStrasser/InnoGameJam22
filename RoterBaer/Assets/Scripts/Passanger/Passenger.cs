using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    [System.Serializable]
    struct ScaredOf
    {
        public EMonsterType monsterType;
        public float scariness;
    }

    [SerializeField] EPassengerType passengerType;

    [Header("Scare Values")]
    [SerializeField] [Range(0.01f, 1f)] float standardScariness;
    [SerializeField] List<ScaredOf> scarinessOfMonsters = new List<ScaredOf>();

    [SerializeField] Animator imageAnim;

    private float stressLevel = 0;
    public float StressLevel
    {
        get => stressLevel;
        set
        {
            stressLevel = value;
            imageAnim.SetFloat("stressLevel", value);
        }
    }

    public void ScarePassenger(EMonsterType monsterType)
    {
        float monsterSpecificScariness = standardScariness;

        foreach (var monster in scarinessOfMonsters)
        {
            if(monsterType == monster.monsterType)
            {
                monsterSpecificScariness = monster.scariness;
                break;
            }
        }

        StressLevel += monsterSpecificScariness;
    }
}

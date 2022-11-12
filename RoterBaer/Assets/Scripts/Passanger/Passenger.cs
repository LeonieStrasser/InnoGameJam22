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

    [Header("Scare States")]
    [SerializeField] [Range(0, 1)] float endOfBoredLevel;
    [SerializeField] [Range(0, 1)] float littleScaredLevel;
    [SerializeField] [Range(0, 1)] float scaredLevel;
    [Space(10)]
    [SerializeField] GameObject zzzVfx;

    private float stressLevel = 0;
    public float StressLevel
    {
        get => stressLevel;
        set
        {
            stressLevel = value;
            SetScareLevel(value);

        }
    }


    enum passangerMode
    {
        bored,
        normal,
        littleScared,
        scared,
        despawn
    }
    bool dead = false;

    passangerMode myScareLevel = passangerMode.bored;

    public void ScarePassenger(EMonsterType monsterType)
    {
        if (!dead)
        {
            float monsterSpecificScariness = standardScariness;

            foreach (var monster in scarinessOfMonsters)
            {
                if (monsterType == monster.monsterType)
                {
                    monsterSpecificScariness = monster.scariness;
                    break;
                }
            }

            StressLevel += monsterSpecificScariness;
        }

    }

    void SetScareLevel(float scareValue)
    {
        if (scareValue > endOfBoredLevel && scareValue < littleScaredLevel)
        {
            myScareLevel = passangerMode.normal;
            imageAnim.SetBool("normal", true);
            imageAnim.SetTrigger("scream");
            zzzVfx.SetActive(false);

        }
        else if (scareValue > littleScaredLevel && scareValue < scaredLevel)
        {
            myScareLevel = passangerMode.littleScared;
            imageAnim.SetTrigger("scream");
            imageAnim.SetBool("littleScared", true);
        }
        else if (scareValue > scaredLevel && scareValue < 1)
        {
            myScareLevel = passangerMode.scared;
            imageAnim.SetTrigger("scream");
            imageAnim.SetBool("scared", true);
        }
        else if (scareValue >= 1)
        {
            myScareLevel = passangerMode.despawn;
            imageAnim.SetBool("despawn", true);
            dead = true;
        }
    }

}

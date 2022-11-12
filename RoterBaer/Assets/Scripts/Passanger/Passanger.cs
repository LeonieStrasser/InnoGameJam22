using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passanger : MonoBehaviour
{
    [Header("Scare Values")]
    [SerializeField] [Range(0.01f, 1f)] float stressIncreasePerScare;

    [SerializeField] Animator imageAnim;

    private float stressLevel = 0;
    public float StrassLevel
    {
        set
        {
            stressLevel = value;
            imageAnim.SetFloat("stressLevel", value);
        }
    }

    public void ScarePassanger()
    {
        stressLevel += stressIncreasePerScare;
    }
}

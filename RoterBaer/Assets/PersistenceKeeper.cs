using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceKeeper : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

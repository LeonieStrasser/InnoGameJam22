using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSoundmanager : MonoBehaviour
{
    [SerializeField] int musicLevelAtStart = -1;

    private void Awake()
    {
        if (musicLevelAtStart < 0)
        {
            Debug.LogWarning("[SceneSoundmanager] Musiklevel set below zero!", this);
            return;
        }

        AudioManager.instance.MusicStart();
        AudioManager.instance.AmbientStart();

        AudioManager.instance.MusicSetLevel(musicLevelAtStart);
        AudioManager.instance.AmbientSetLevel(musicLevelAtStart);
    }
}

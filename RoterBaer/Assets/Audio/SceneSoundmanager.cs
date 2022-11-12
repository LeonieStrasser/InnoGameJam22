using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSoundmanager : MonoBehaviour
{
    [SerializeField] bool ambientActive;

    [SerializeField] int musicLevelAtStart = -1;

    private void Awake()
    {
        if (musicLevelAtStart < 0) return;

        AudioManager.instance.MusicSetLevel(musicLevelAtStart);
        AudioManager.instance.MusicStart();

        if (ambientActive)
            AudioManager.instance.AmbientStart();
        else
            AudioManager.instance.AmbientStop();
    }
}

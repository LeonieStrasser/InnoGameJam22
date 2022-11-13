using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSoundmanager : MonoBehaviour
{
    [SerializeField] int musicLevelAtStart = -1;

    private void Awake()
    {
        if (musicLevelAtStart < 0) return;

        AudioManager.instance.MusicSetLevel(musicLevelAtStart);
        AudioManager.instance.AmbientSetLevel(musicLevelAtStart);

        AudioManager.instance.MusicStart();
        AudioManager.instance.AmbientStart();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] string loadedLevelName;

    public void Load()
    {
        AudioManager.instance.MenuButtonAccept();
        SceneManager.LoadSceneAsync(loadedLevelName);
    }
}

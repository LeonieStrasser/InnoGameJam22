using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{    
    public static void Quit()
    {
        AudioManager.instance.MenuButtonAccept();

        AudioManager.instance.MusicStop();
        AudioManager.instance.AmbientStop();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}

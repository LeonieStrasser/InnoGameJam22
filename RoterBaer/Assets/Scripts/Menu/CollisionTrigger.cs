using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    public bool QuitTrigger;
    public bool cameraChangerLevel;
    public bool cameraChangerCredits;
    public bool objectChange;


    bool inLevelMenu = false;
    [SerializeField] Animator camAnimator;
    [SerializeField] GameObject levelButtons;

    [Space(30)]
    [SerializeField] GameObject desableObject;
    [SerializeField] GameObject enableObject;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (QuitTrigger)
        {
            Quit();
        }
        else if (cameraChangerLevel)
        {
            if (!inLevelMenu)
            {
                camAnimator.SetTrigger("levelCam");
                StartCoroutine(buttonBlendIn());
                inLevelMenu = true;
            }

            else
            {
                camAnimator.SetTrigger("menuCam");
                levelButtons.SetActive(false);
                inLevelMenu = false;
            }

        }

        else if (cameraChangerCredits)
        {

            camAnimator.SetTrigger("creditCam");
            AudioManager.instance.MusicSetLevel(10);


        }
        else if (objectChange)
        {
            desableObject.SetActive(false);
            enableObject.SetActive(true);
        }



    }
    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    IEnumerator buttonBlendIn()
    {
        yield return new WaitForSeconds(3);
        levelButtons.SetActive(true);
    }
}



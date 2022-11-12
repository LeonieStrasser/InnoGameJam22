using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance = null;

    private FMOD.Studio.EventInstance MusicInstance;
    private FMOD.Studio.EventInstance AmbientInstance;
    private FMOD.Studio.Bus EnvEmittersBus;
    private FMOD.Studio.Bus MusicBus;
    private FMOD.Studio.Bus SFXBus;

    Dictionary<GameObject, EventInstance> wagonToEventInstance = new Dictionary<GameObject, EventInstance>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        EnvEmittersBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX/EnvEmitterBus");
        MusicBus = FMODUnity.RuntimeManager.GetBus("bus:/MusicBus");
        SFXBus = FMODUnity.RuntimeManager.GetBus("bus:/SFXBus");

        MusicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/2D/Music");
        AmbientInstance = FMODUnity.RuntimeManager.CreateInstance("event:/2D/Ambient");

        //AmbientStart();
        //MusicStart();
    }

    public void Button_Click()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/2D/Button_Click");        
    }

    public void Button_Hover()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/2D/Button_Hover");
    }

    public void Button_Cancel()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/2D/Button_Cancel");
    }


    // Environment Emitters ------------------------------------------------------------------


    public void WagonInitialize(GameObject FlyingCar)
    {
        FMOD.Studio.EventInstance FlyingCarInstance = FMODUnity.RuntimeManager.CreateInstance("event:/3D/Flying_Car");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(FlyingCarInstance, FlyingCar.transform, FlyingCar.GetComponent<Rigidbody>());
        FlyingCarInstance.start();
        FlyingCarInstance.release();

        wagonToEventInstance.Add(FlyingCar, FlyingCarInstance);
    }

    public void WagonReachedTrackSwitch(VisitorCart cart)
    {
        Vector3 cartPosition = cart.Position;

    }

    public void WagonRetirement(GameObject FlyingCar)
    {
        if (!wagonToEventInstance.ContainsKey(FlyingCar))
        {
            Debug.LogError($"Requested Retirement of unknown car {FlyingCar.gameObject.name}", FlyingCar);
            return;
        }

        wagonToEventInstance[FlyingCar].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        wagonToEventInstance.Remove(FlyingCar);
    }

    public void StopAllEnvEmitters()
    {
        //Stops all instances of Enviromental Emitters
        EnvEmittersBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PauseMenu()
    {
        EnvEmittersBus.setPaused(true);
    }
    public void UnPauseMenu()
    {
        EnvEmittersBus.setPaused(false);
    }

    /*
    public void BallRollingSpeedUpdate(float BallSpeed)
    {
        BallRollingInstance.setParameterByName("Speed", BallSpeed);
        MusicInstance.setParameterByName("Speed", BallSpeed);
    }
    

    public void BallRollingStop()
    {
        BallRollingInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    */

    // End of Environment Emitters ------------------------------------------------------------------


    /*
    public void BallImpact(Vector3 BallPosition)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/3D/Ball_Impact", BallPosition);
    }


    public void Collectible()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/2D/Collectible");
    }
    */

    public void AmbientStart()
    {
        AmbientInstance.start();
    }
    public void AmbientStop()
    {
        AmbientInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void MusicStart()
    {
        if (EventIsNotPlaying(MusicInstance))
        {
            MusicInstance.start();
        }
    }

    public void MusicStop()
    {
        MusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    /// <summary>
    /// Sets Level for the music event
    /// </summary>
    /// <param name="SetLevel">
    /// 0=Menu 
    /// 1=Level_1
    /// 2=Level_2
    /// 3=Level_3
    /// 4=Level_4
    /// 10=Credits
    /// </param>
    public void MusicSetLevel(int SetLevel)
    {
        MusicInstance.setParameterByName("Level", SetLevel);
    }

    bool EventIsNotPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.PLAYING;
    }

    public void MusicBusSetVolume(float volume)
    {
        MusicBus.setVolume(volume);
    }

    public void SFXBusSetVolume(float volume)
    {
        SFXBus.setVolume(volume);
    }

    public void MonsterActivated(MonsterActivator monster)
    {
        Vector3 monsterPosition = monster.Position;

        switch (monster.MyType)
        {
            case EMonsterType.MonsterA:

                break;
            case EMonsterType.MonsterB:

                break;
            case EMonsterType.MonsterC:

                break;
            case EMonsterType.MonsterD:

                break;

            default:
                Debug.LogError($"{nameof(MonsterActivated)} is UNDEFINED for {monster.MyType}.", this);
                break;
        }
    }
}

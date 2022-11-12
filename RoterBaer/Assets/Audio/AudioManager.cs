using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance = null;

    private EventInstance MusicInstance;
    private EventInstance AmbientInstance;
    private Bus EnvEmittersBus;
    private Bus MusicBus;
    private Bus SFXBus;

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
        EnvEmittersBus = FMODUnity.RuntimeManager.GetBus("bus:/SFXBus/EnvEmitterBus");
        MusicBus = FMODUnity.RuntimeManager.GetBus("bus:/MusicBus");
        SFXBus = FMODUnity.RuntimeManager.GetBus("bus:/SFXBus");

        MusicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Non-Spatialized/Music");
        AmbientInstance = FMODUnity.RuntimeManager.CreateInstance("event:Non-Spatialized/Ambient");

    }
    public void SwitchAuto(Vector3 OneShotPosition)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Non-Spatialized/SwitchAuto", OneShotPosition);
    }
    public void SwitchPlayer(Vector3 OneShotPosition)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Non-Spatialized/SwitchPlayer", OneShotPosition);
    }
    public void WagonPassesSwitch(Vector3 OneShotPosition)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Non-Spatialized/WagonPassesSwitch", OneShotPosition);
    }

    public void MenuButtonAccept()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Non-Spatialized/MenuButtonAccept");        
    }

    public void MenuButtonBack()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Non-Spatialized/MenuButtonBack()");
    }

    public void PointsPositive()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Non-Spatialized/PointsPositive");
    }
    public void PointsNegative()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Non-Spatialized/PointsNegative");
    }


    public void WagonInitialize(GameObject Wagon)
    {
        EventInstance WagonInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Spazialized/WagonRoll");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(WagonInstance, Wagon.transform, Wagon.GetComponent<Rigidbody>());
        WagonInstance.start();
        

        wagonToEventInstance.Add(Wagon, WagonInstance);
    }

    public void WagonReachedTrackSwitch(VisitorCart cart)
    {
        Vector3 cartPosition = cart.Position;

    }

    public void WagonRetirement(GameObject Wagon)
    {
        if (!wagonToEventInstance.ContainsKey(Wagon))
        {
            Debug.LogError($"Requested Retirement of unknown wagon {Wagon.gameObject.name}", Wagon);
            return;
        }
        
        wagonToEventInstance[Wagon].release();
        wagonToEventInstance[Wagon].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        
        wagonToEventInstance.Remove(Wagon);
    }

    public void StopAllEnvEmitters()
    {
        //Stops all instances of Enviromental Emitters
        EnvEmittersBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        wagonToEventInstance.Clear();
    }

    public void PauseMenu()
    {
        EnvEmittersBus.setPaused(true);
    }
    public void UnPauseMenu()
    {
        EnvEmittersBus.setPaused(false);
    }


    public void AmbientStart()
    {
        if (EventIsNotPlaying(AmbientInstance))
        {
            AmbientInstance.start();
        }
        
    }
    public void AmbientStop()
    {
        AmbientInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
    public void AmbientSetLevel(int SetLevel)
    {
        AmbientInstance.setParameterByName("Level", SetLevel);
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

    bool EventIsNotPlaying(EventInstance instance)
    {
        PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != PLAYBACK_STATE.PLAYING;
    }

    public void MusicBusSetVolume(float volume)
    {
        MusicBus.setVolume(volume);
    }

    public void SFXBusSetVolume(float volume)
    {
        SFXBus.setVolume(volume);
    }

    public void MonsterScare(MonsterActivator monster)
    {
        EventInstance MonsterScareInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Spazialized/MonsterScare");
        MonsterScareInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(monster.gameObject));

        switch (monster.MyType)
        {
            case EMonsterType.MonsterA:
                MonsterScareInstance.setParameterByName("MonsterType", 1);            
                break;
            case EMonsterType.MonsterB:
                MonsterScareInstance.setParameterByName("MonsterType", 2);
                break;
            case EMonsterType.MonsterC:
                MonsterScareInstance.setParameterByName("MonsterType", 3);
                break;
            case EMonsterType.MonsterD:
                MonsterScareInstance.setParameterByName("MonsterType", 4);
                break;

            default:
                Debug.LogError($"{nameof(MonsterScare)} is UNDEFINED for {monster.MyType}.", this);
                break;
        }

        MonsterScareInstance.start();
        MonsterScareInstance.release();
    }

    
    public void CharacterScare(Passenger passenger)
    {
        EventInstance CharacterScareInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Spazialized/CharacterScare");
        CharacterScareInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(passenger.gameObject));

        switch (passenger.PassengerType)
        {
            case EPassengerType.JohnDoe:
                Debug.LogError("Scare sound called for John Doe");
                CharacterScareInstance.setParameterByName("CharacterType", 2);
                break;
            case EPassengerType.WeirdGirl:
                CharacterScareInstance.setParameterByName("CharacterType", 1);
                break;
            case EPassengerType.Grandpa:
                CharacterScareInstance.setParameterByName("CharacterType", 2);
                break;
            case EPassengerType.GigaChad:
                CharacterScareInstance.setParameterByName("CharacterType", 3);
                break;
            default:
                Debug.LogError($"{nameof(CharacterScare)} is UNDEFINED for {passenger.PassengerType}.", this);
                break;
        }

        CharacterScareInstance.start();
        CharacterScareInstance.release();
    }
    
}

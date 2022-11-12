using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreCounter : MonoBehaviour
{
    const string PREFAB_HIGHSCORE = "HighScore1";

    public static HighscoreCounter Active { get; private set; }

    int currentHighscore = 0;
    public int Highscore
    {
        get => currentHighscore;
        set
        {
            currentHighscore = value;
            if (currentHighscore > bestHighscore)
                BestHighscore = currentHighscore;
            highscoreText.text = currentHighscore.ToString();
        }
    }

    int bestHighscore = 0;
    public int BestHighscore
    {
        get => bestHighscore;
        set
        {
            bestHighscore = value;
            bestScoreText.text = bestHighscore.ToString();
            bestScoreText.enabled = (bestHighscore != 0);
        }
    }

    [SerializeField] TMPro.TMP_Text highscoreText;
    [SerializeField] TMPro.TMP_Text bestScoreText;

    float biggerTextSize;
    float smallerTextSize;

    [Header("Points Per Passenger")]
    [SerializeField] int boredPoints = -10;
    [SerializeField] int normalPoints = -5;
    [SerializeField] int littleScaredPoints = 5;
    [SerializeField] int scaredPoints = 10;
    [SerializeField] int despawnPoints = -20;

    private void Awake()
    {
        Active = this;

        // to initialise text field
        Highscore = currentHighscore;

        if (PlayerPrefs.HasKey(PREFAB_HIGHSCORE))
            BestHighscore = PlayerPrefs.GetInt(PREFAB_HIGHSCORE);
        else
            BestHighscore = currentHighscore;
    }

    private void OnDestroy()
    {
        if (!PlayerPrefs.HasKey(PREFAB_HIGHSCORE) || currentHighscore > PlayerPrefs.GetInt(PREFAB_HIGHSCORE))
            PlayerPrefs.SetInt(PREFAB_HIGHSCORE, currentHighscore);
    }

    public void PassengerLeft(Passenger passenger)
    {
        Highscore += ScareLevelPoints(passenger.ScareLevel);
    }

    private int ScareLevelPoints(Passenger.EPassengerMode scareLevel)
    {
        switch (scareLevel)
        {
            case Passenger.EPassengerMode.bored:
                return boredPoints;
            case Passenger.EPassengerMode.normal:
                return normalPoints;
            case Passenger.EPassengerMode.littleScared:
                return littleScaredPoints;
            case Passenger.EPassengerMode.scared:
                return scaredPoints;
            case Passenger.EPassengerMode.despawn:
                return despawnPoints;

            default:
                Debug.LogError($"[{GetType().Name}] {nameof(ScareLevelPoints)} UNDEFINED for {scareLevel}.", this);
                return 0;
        }
    }
}

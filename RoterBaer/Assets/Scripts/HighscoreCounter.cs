using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreCounter : MonoBehaviour
{
    [SerializeField] int highscoreLevelID;
    [SerializeField] float roundLengthSeconds = 60;
    float remainingTime = 0;
    bool gameEnded = false;

    [Space]
    [SerializeField] HighscoreListVisual highscoreList;
    [SerializeField] Canvas runningRoundScoreCanvas;

    const int TRACKED_HIGHSCORES = 10;
    const string PREFAB_HIGHSCORE = "HighScore_Level{0}_Rank{1}";

    public static HighscoreCounter Active { get; private set; }

    bool highscoreSaved;

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
    [SerializeField] TMPro.TMP_Text timer;


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

        if (PlayerPrefs.HasKey(GetPlayerPrefabKey(highscoreLevelID, 1)))
            BestHighscore = PlayerPrefs.GetInt(GetPlayerPrefabKey(highscoreLevelID, 1));
        else
            BestHighscore = currentHighscore;

        if (roundLengthSeconds <= 0)
            enabled = false;
        else
            remainingTime = roundLengthSeconds;
    }

    private void OnDestroy()
    {
        if (!highscoreSaved) SaveHighscore(highscoreLevelID, currentHighscore);
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            Endgame();
            enabled = false;
            timer.enabled = false;
        }

        timer.text = Mathf.Round(remainingTime).ToString();
    }

    public void PassengerLeft(Passenger passenger)
    {
        if (gameEnded) return;

        int addedPoint = ScareLevelPoints(passenger.ScareLevel);

        if (addedPoint > 0)
            AudioManager.instance.PointsPositive();
        else if (addedPoint < 0)
            AudioManager.instance.PointsNegative();

        Highscore = Mathf.Max(0, Highscore + ScareLevelPoints(passenger.ScareLevel));
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

    private void Endgame()
    {
        gameEnded = true;

        VisitorCartController.Active.SetSpawnCarts(false);

        SaveHighscore(highscoreLevelID, currentHighscore);
        highscoreSaved = true;

        runningRoundScoreCanvas.enabled = false;
        highscoreList.SetupHighscore(highscoreLevelID, currentHighscore);
    }

    #region Saving Highscore
    private static string GetPlayerPrefabKey(int levelID, int rank) => string.Format(PREFAB_HIGHSCORE, levelID, rank);

    private static void SaveHighscore(int levelID, int newScore)
    {
        if (newScore <= 0) return;

        List<int> scores = GetHighscores(levelID);

        scores.Add(newScore);
        scores.Sort();
        scores.Reverse();

        for (int pos = 0; pos < Mathf.Min(TRACKED_HIGHSCORES, scores.Count); pos++)
        {
            PlayerPrefs.SetInt(GetPlayerPrefabKey(levelID, pos + 1), scores[pos]);
        }
    }

    public static List<int> GetHighscores(int levelID)
    {
        List<int> scores = new List<int>();

        for (int rank = 1; rank <= TRACKED_HIGHSCORES; rank++)
        {
            if (PlayerPrefs.HasKey(GetPlayerPrefabKey(levelID, rank)))
                scores.Add(PlayerPrefs.GetInt(GetPlayerPrefabKey(levelID, rank)));
        }
        return scores;
    }
    #endregion Saving Highscore
}

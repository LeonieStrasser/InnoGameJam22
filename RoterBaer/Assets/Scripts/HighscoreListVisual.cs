using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreListVisual : MonoBehaviour
{
    [Header("HighscoreList")]
    [SerializeField] Transform rankListParent;
    [SerializeField] float identicalScoresAdditionalFontSize = 5;
    TMPro.TMP_Text[] rankTexts;
    [Header("Player Result")]
    [SerializeField] TMPro.TMP_Text newHighscoreText;
    [SerializeField] TMPro.TMP_Text youReachedText;
    [SerializeField] TMPro.TMP_Text playerScoreText;

    List<TMPro.TMP_Text> playerIdenticalRankTexts = new List<TMPro.TMP_Text>();

    public void SetupHighscore(int levelID, int playerReached = 0)
    {
        gameObject.SetActive(true);

        rankTexts = rankListParent.GetComponentsInChildren<TMP_Text>(true);
        List<int> highscores = HighscoreCounter.GetHighscores(levelID);

        int minimumEntries = Mathf.Min(highscores.Count, rankTexts.Length);
        for (int i = 0; i < minimumEntries; i++)
        {
            rankTexts[i].text = highscores[i].ToString();

            if (highscores[i] == playerReached)
                playerIdenticalRankTexts.Add(rankTexts[i]);

            rankTexts[i].enabled = true;
        }

        for (int i = minimumEntries; i < rankTexts.Length; i++)
        {
            rankTexts[i].enabled = false;
        }

        SetupPersonalScoreArea(playerReached);

        foreach (var identicalScoreText in playerIdenticalRankTexts)
        {
            identicalScoreText.fontStyle = FontStyles.Bold;
            identicalScoreText.fontSize += identicalScoresAdditionalFontSize;
        }
    }

    private void SetupPersonalScoreArea(int reachedPoints)
    {
        bool inRankList = playerIdenticalRankTexts.Count > 0;

        newHighscoreText.enabled = inRankList;
        youReachedText.enabled = !inRankList;
        playerScoreText.text = reachedPoints.ToString();
    }
}

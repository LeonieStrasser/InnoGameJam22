using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreListVisual : MonoBehaviour
{
    [Header("HighscoreList")]
    [SerializeField] Transform rankListParent;
    [SerializeField] TMPro.TMP_Text prefabHighscoreRank;
    [Header("Player Result")]
    [SerializeField] TMPro.TMP_Text newHighscoreText;
    [SerializeField] TMPro.TMP_Text youReachedText;
    [SerializeField] TMPro.TMP_Text playerScoreText;
    
    List<TMPro.TMP_Text> playerIdenticalRankTexts = new List<TMPro.TMP_Text>();

    public void SetupHighscore(int levelID, int playerReached = 0)
    {
        List<int> highscores = HighscoreCounter.GetHighscores(levelID);

        foreach (var score in highscores)
        {
            TMPro.TMP_Text nextRankText = Instantiate(prefabHighscoreRank, rankListParent);
            nextRankText.text = score.ToString();

            if (score == playerReached)
                playerIdenticalRankTexts.Add(nextRankText);
        }

        SetupPersonalScoreArea(playerReached);
    }

    private void SetupPersonalScoreArea(int reachedPoints)
    {
        bool inRankList = playerIdenticalRankTexts.Count > 0;

//        if(inRankList)

    }
}

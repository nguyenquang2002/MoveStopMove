using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI aliveCountText;
    private GameObject endgamePanel;
    private GameObject gameOverBG, victoryBG;
    private TextMeshProUGUI killerText, rankingText;
    private void Awake()
    {
        endgamePanel = transform.Find("EndgamePanel").gameObject;
        gameOverBG = endgamePanel.transform.Find("GameOverBG").gameObject;
        victoryBG = endgamePanel.transform.Find("VictoryBG").gameObject;
        killerText = gameOverBG.transform.Find("KillerText").GetComponent<TextMeshProUGUI>();
        rankingText = gameOverBG.transform.Find("RankingText").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        DeActiveEndgame();
    }
    public void DeActiveEndgame()
    {
        endgamePanel.SetActive(false);
        gameOverBG.SetActive(false);
        victoryBG.SetActive(false);
    }
    public void SetGameOver()
    {
        endgamePanel.SetActive(true);
        gameOverBG.SetActive(true);
    }
    public void SetVictory()
    {
        endgamePanel.SetActive(true);
        victoryBG.SetActive(true);
    }
    public void DisplayAliveCount(int alive)
    {
        aliveCountText.text = alive.ToString();
    }
    public void SetKillerText(GameObject gameObject)
    {
        killerText.text = gameObject.name;
        if (gameObject.GetComponent<EnemyController>() != null)
        {
            killerText.color = gameObject.GetComponent<EnemyController>().GetColor();
        }
    }
    public void SetRanking(int ranking)
    {
        rankingText.text = "#" + ranking.ToString();
    }
}

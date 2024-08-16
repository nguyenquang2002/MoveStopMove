using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI aliveCountText;
    private GameObject endgamePanel;
    private void Awake()
    {
        endgamePanel = transform.Find("EndgamePanel").gameObject;
    }
    private void Start()
    {
        endgamePanel.SetActive(false);
    }
    public void DisplayAliveCount(int alive)
    {
        aliveCountText.text = alive.ToString();
    }
}

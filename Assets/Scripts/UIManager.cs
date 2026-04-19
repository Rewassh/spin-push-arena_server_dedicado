using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI roleText;

    [Header("Winner Panel")]
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private TextMeshProUGUI winnerText;

    public void UpdateScore(int player1Score, int player2Score)
    {
        if (scoreText == null)
        {
            return;
        }

        scoreText.text = $"P1: {player1Score} | P2: {player2Score}";
    }

    public void SetRole(string role)
    {
        if (roleText == null)
        {
            return;
        }

        roleText.text = $"You are: {role}";
    }

    public void SetStatus(string status)
    {
        if (statusText == null)
        {
            return;
        }

        statusText.text = status;
    }

    public void ShowWinner(string message)
    {
        if (winnerPanel != null)
        {
            winnerPanel.SetActive(true);
        }

        if (winnerText != null)
        {
            winnerText.text = message;
        }
    }

    public void HideWinner()
    {
        if (winnerPanel != null)
        {
            winnerPanel.SetActive(false);
        }
    }
}
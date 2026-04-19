using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField gameIdInput;

    public void PlayAsPlayer1()
    {
        StartGame("Player1");
    }

    public void PlayAsPlayer2()
    {
        StartGame("Player2");
    }

    private void StartGame(string playerId)
    {
        string enteredGameId = gameIdInput != null ? gameIdInput.text.Trim() : string.Empty;

        if (string.IsNullOrEmpty(enteredGameId))
        {
            enteredGameId = "test";
        }

        GameSession.GameId = enteredGameId;
        GameSession.SelectedPlayerId = playerId;

        SceneManager.LoadScene("GameScene");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Player Rigidbodies")]
    [SerializeField] private Rigidbody player1Rigidbody;
    [SerializeField] private Rigidbody player2Rigidbody;

    [Header("Spawn Points")]
    [SerializeField] private Transform player1Spawn;
    [SerializeField] private Transform player2Spawn;

    [Header("Boundary")]
    [SerializeField] private ArenaBoundary arenaBoundary;

    [Header("UI")]
    [SerializeField] private UIManager uiManager;

    [Header("Game Settings")]
    [SerializeField] private int maxScore = 3;
    [SerializeField] private string menuSceneName = "MainMenu";

    private int player1Score;
    private int player2Score;
    private bool roundActive;

    private void Start()
    {
        player1Score = 0;
        player2Score = 0;
        roundActive = false;

        if (uiManager != null)
        {
            uiManager.HideWinner();
            uiManager.UpdateScore(player1Score, player2Score);
            uiManager.SetStatus("Waiting for opponent...");
        }
    }

    private void Update()
    {
        if (!roundActive)
        {
            return;
        }

        CheckOutOfBounds();
    }

    public void StartMatch()
    {
        roundActive = true;

        if (uiManager != null)
        {
            uiManager.SetStatus("Fight!");
            uiManager.UpdateScore(player1Score, player2Score);
        }
    }

    public void StopMatch()
    {
        roundActive = false;

        if (uiManager != null)
        {
            uiManager.SetStatus("Waiting for opponent...");
        }
    }

    private void CheckOutOfBounds()
    {
        if (arenaBoundary == null)
        {
            return;
        }

        if (arenaBoundary.IsOutsideBounds(player1.position))
        {
            HandleRoundLoss(1);
            return;
        }

        if (arenaBoundary.IsOutsideBounds(player2.position))
        {
            HandleRoundLoss(2);
        }
    }

    private void HandleRoundLoss(int losingPlayerIndex)
    {
        if (!roundActive)
        {
            return;
        }

        roundActive = false;

        if (losingPlayerIndex == 1)
        {
            player2Score++;
        }
        else
        {
            player1Score++;
        }

        if (uiManager != null)
        {
            uiManager.UpdateScore(player1Score, player2Score);
        }

        if (player1Score >= maxScore || player2Score >= maxScore)
        {
            EndGame();
            return;
        }

        ResetRound();
    }

    private void ResetRound()
    {
        ResetPlayer(player1, player1Rigidbody, player1Spawn);
        ResetPlayer(player2, player2Rigidbody, player2Spawn);

        roundActive = true;

        if (uiManager != null)
        {
            uiManager.SetStatus("Fight!");
            uiManager.UpdateScore(player1Score, player2Score);
        }
    }

    private void ResetPlayer(Transform playerTransform, Rigidbody playerRigidbody, Transform spawnPoint)
    {
        if (playerTransform == null || playerRigidbody == null || spawnPoint == null)
        {
            return;
        }

        if (!playerRigidbody.isKinematic)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        playerTransform.position = spawnPoint.position;
        playerTransform.rotation = spawnPoint.rotation;

        playerRigidbody.position = spawnPoint.position;
        playerRigidbody.rotation = spawnPoint.rotation;
    }

    private void EndGame()
    {
        roundActive = false;

        if (uiManager != null)
        {
            uiManager.SetStatus("Game Over");

            if (player1Score > player2Score)
            {
                uiManager.ShowWinner("PLAYER 1 WINS!");
            }
            else
            {
                uiManager.ShowWinner("PLAYER 2 WINS!");
            }
        }
    }

    public void PlayAgain()
    {
        player1Score = 0;
        player2Score = 0;
        roundActive = false;

        ResetPlayer(player1, player1Rigidbody, player1Spawn);
        ResetPlayer(player2, player2Rigidbody, player2Spawn);

        if (uiManager != null)
        {
            uiManager.HideWinner();
            uiManager.UpdateScore(player1Score, player2Score);
            uiManager.SetStatus("Waiting for opponent...");
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
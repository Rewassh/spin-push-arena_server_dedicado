using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private GameObject player1Object;
    [SerializeField] private GameObject player2Object;

    [Header("Networking")]
    [SerializeField] private ServerApiClient serverApiClient;

    [Header("UI")]
    [SerializeField] private UIManager uiManager;

    [Header("Game")]
    [SerializeField] private GameManager gameManager;

    private GameObject localPlayerObject;
    private GameObject remotePlayerObject;

    private PlayerMovement localPlayerMovement;
    private PlayerPush localPlayerPush;

    private Rigidbody localPlayerRigidbody;
    private Rigidbody remotePlayerRigidbody;

    private Collider[] remoteColliders;
    private Renderer[] remoteRenderers;

    private bool matchStarted;

    private void Start()
    {
        Application.runInBackground = true;
        ConfigureSession();
    }

    private void Update()
    {
        if (serverApiClient == null || gameManager == null)
        {
            return;
        }

        if (!matchStarted && serverApiClient.RemotePlayerFound)
        {
            SnapRemotePlayerToNetworkPosition();
            SetRemotePlayerVisible(true);
            EnableLocalControl(true);
            gameManager.StartMatch();
            matchStarted = true;
        }
    }

    private void ConfigureSession()
    {
        bool isPlayerOne = GameSession.SelectedPlayerId == "Player1";

        localPlayerObject = isPlayerOne ? player1Object : player2Object;
        remotePlayerObject = isPlayerOne ? player2Object : player1Object;

        PlayerMovement player1Movement = player1Object.GetComponent<PlayerMovement>();
        PlayerMovement player2Movement = player2Object.GetComponent<PlayerMovement>();

        PlayerPush player1Push = player1Object.GetComponent<PlayerPush>();
        PlayerPush player2Push = player2Object.GetComponent<PlayerPush>();

        localPlayerMovement = isPlayerOne ? player1Movement : player2Movement;
        localPlayerPush = isPlayerOne ? player1Push : player2Push;

        localPlayerRigidbody = localPlayerObject.GetComponent<Rigidbody>();
        remotePlayerRigidbody = remotePlayerObject.GetComponent<Rigidbody>();

        remoteColliders = remotePlayerObject.GetComponentsInChildren<Collider>();
        remoteRenderers = remotePlayerObject.GetComponentsInChildren<Renderer>();

        SetComponentState(player1Movement, false);
        SetComponentState(player1Push, false);
        SetComponentState(player2Movement, false);
        SetComponentState(player2Push, false);

        if (localPlayerRigidbody != null)
        {
            localPlayerRigidbody.isKinematic = false;
        }

        if (remotePlayerRigidbody != null)
        {
            remotePlayerRigidbody.linearVelocity = Vector3.zero;
            remotePlayerRigidbody.angularVelocity = Vector3.zero;
            remotePlayerRigidbody.isKinematic = true;
        }

        remotePlayerObject.transform.position = new Vector3(1000f, 1000f, 1000f);

        if (serverApiClient != null)
        {
            serverApiClient.Configure(
                "http://127.0.0.1:5005",
                GameSession.GameId,
                GameSession.SelectedPlayerId,
                isPlayerOne ? "Player2" : "Player1",
                localPlayerObject.transform,
                remotePlayerObject.transform
            );
        }

        if (uiManager != null)
        {
            uiManager.SetRole(GameSession.SelectedPlayerId);
            uiManager.SetStatus("Waiting for opponent...");
        }

        if (gameManager != null)
        {
            gameManager.StopMatch();
        }

        EnableLocalControl(false);
        SetRemotePlayerVisible(false);
        matchStarted = false;
    }

    private void SnapRemotePlayerToNetworkPosition()
    {
        if (serverApiClient == null || remotePlayerObject == null)
        {
            return;
        }

        remotePlayerObject.transform.position = serverApiClient.CurrentRemotePosition;

        if (remotePlayerRigidbody != null)
        {
            remotePlayerRigidbody.position = serverApiClient.CurrentRemotePosition;
        }
    }

    private void EnableLocalControl(bool shouldEnable)
    {
        SetComponentState(localPlayerMovement, shouldEnable);
        SetComponentState(localPlayerPush, shouldEnable);
    }

    private void SetRemotePlayerVisible(bool shouldEnable)
    {
        if (remoteRenderers != null)
        {
            foreach (Renderer remoteRenderer in remoteRenderers)
            {
                remoteRenderer.enabled = shouldEnable;
            }
        }

        if (remoteColliders != null)
        {
            foreach (Collider remoteCollider in remoteColliders)
            {
                remoteCollider.enabled = shouldEnable;
            }
        }
    }

    private void SetComponentState(MonoBehaviour componentToToggle, bool shouldEnable)
    {
        if (componentToToggle != null)
        {
            componentToToggle.enabled = shouldEnable;
        }
    }
}
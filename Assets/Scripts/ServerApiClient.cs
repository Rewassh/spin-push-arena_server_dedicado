using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerData
{
    public float posX;
    public float posY;
    public float posZ;
}

public class ServerApiClient : MonoBehaviour
{
    [Header("Server Config")]
    [SerializeField] private string serverUrl = "http://127.0.0.1:5005";
    [SerializeField] private string gameId = "game1";
    [SerializeField] private string playerId = "Player1";
    [SerializeField] private string otherPlayerId = "Player2";

    [Header("References")]
    [SerializeField] private Transform localPlayer;
    [SerializeField] private Transform remotePlayer;

    [Header("Timing")]
    [SerializeField] private float sendInterval = 0.1f;
    [SerializeField] private float receiveInterval = 0.1f;
    [SerializeField] private float remoteLerpSpeed = 10f;

    private Vector3 remoteTargetPosition;
    private bool remotePlayerFound;
    private bool isConfigured;
    private bool routinesStarted;

    public bool RemotePlayerFound => remotePlayerFound;
    public Vector3 CurrentRemotePosition => remoteTargetPosition;

    private void Start()
    {
        TryStartNetworking();
    }

    private void Update()
    {
        if (remotePlayer == null || !remotePlayerFound)
        {
            return;
        }

        remotePlayer.position = Vector3.Lerp(
            remotePlayer.position,
            remoteTargetPosition,
            remoteLerpSpeed * Time.deltaTime
        );
    }

    public void Configure(
        string newServerUrl,
        string newGameId,
        string newPlayerId,
        string newOtherPlayerId,
        Transform newLocalPlayer,
        Transform newRemotePlayer
    )
    {
        serverUrl = newServerUrl;
        gameId = newGameId;
        playerId = newPlayerId;
        otherPlayerId = newOtherPlayerId;
        localPlayer = newLocalPlayer;
        remotePlayer = newRemotePlayer;

        isConfigured = true;
        remotePlayerFound = false;

        if (remotePlayer != null)
        {
            remoteTargetPosition = remotePlayer.position;
        }

        TryStartNetworking();
    }

    private void TryStartNetworking()
    {
        if (routinesStarted || !isConfigured)
        {
            return;
        }

        if (localPlayer == null || remotePlayer == null)
        {
            return;
        }

        routinesStarted = true;
        StartCoroutine(SendPositionRoutine());
        StartCoroutine(GetOtherPlayerRoutine());
    }

    private IEnumerator SendPositionRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(SendPositionRequest());
            yield return new WaitForSeconds(sendInterval);
        }
    }

    private IEnumerator GetOtherPlayerRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(GetOtherPlayerPosition());
            yield return new WaitForSeconds(receiveInterval);
        }
    }

    private IEnumerator SendPositionRequest()
    {
        if (localPlayer == null)
        {
            yield break;
        }

        string url = $"{serverUrl}/server/{gameId}/{playerId}";

        PlayerData payload = new PlayerData
        {
            posX = localPlayer.position.x,
            posY = localPlayer.position.y,
            posZ = localPlayer.position.z
        };

        string json = JsonUtility.ToJson(payload);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Send error: " + request.error + " | URL: " + url);
        }
    }

    private IEnumerator GetOtherPlayerPosition()
    {
        if (remotePlayer == null)
        {
            yield break;
        }

        string url = $"{serverUrl}/server/{gameId}/{otherPlayerId}";

        using UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            remotePlayerFound = false;
            yield break;
        }

        PlayerData data = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);

        if (data != null)
        {
            remoteTargetPosition = new Vector3(data.posX, data.posY, data.posZ);
            remotePlayerFound = true;
        }
    }
}
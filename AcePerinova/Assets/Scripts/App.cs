using AcePerinova.Utilities;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public enum ConnectionStatus {
    Disconnecting,
    Disconnected,
    Connecting,
    Connected
}

/// <summary>
/// Entry point for the application located on it's own separate scene. Locate anywhere with 'App.FindInstance()'
/// </summary>
public class App : MonoBehaviour, INetworkRunnerCallbacks {

    public ConnectionStatus connectionStatus = ConnectionStatus.Disconnected;
    [HideInInspector]
    public SceneSelectManager sceneSelectManager;
    [HideInInspector]
    public NetworkSceneManager networkSceneManager;

    private HomeMenuController homeMenu;
    private NetworkRunner _runner;
    /// <summary>
    /// Call this to retrieve App.cs from anywhere.
    /// </summary>
    public static App FindInstance() {
        return FindObjectOfType<App>();
    }
    public void SetHomeMenu(HomeMenuController ui) {
        homeMenu = ui;
    }

    private void Start() {
        SceneManager.LoadSceneAsync(1);
    }

    private void Awake() {
        //Singleton implementation
        App[] apps = FindObjectsOfType<App>();

        if (apps != null && apps.Length > 1) {
            Destroy(gameObject);
            return;
        }

        if (networkSceneManager == null) {
            networkSceneManager = GetComponentInChildren<NetworkSceneManager>();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void CheckRunner() {
        //Set up runner/session and have it listen to App.
        if (_runner == null) {
            connectionStatus = ConnectionStatus.Connecting;
            GameObject runnerObject = new GameObject("Session");
            runnerObject.transform.SetParent(transform);
            _runner = runnerObject.AddComponent<NetworkRunner>();
            _runner.AddCallbacks(this);
            if (homeMenu == null) {
                Debug.LogWarning("Home Menu not properly set or found. App.cs will search the scene.");
                homeMenu = FindObjectOfType<UIDocument>().rootVisualElement.Q<HomeMenuController>();
                if (homeMenu == null) {
                    Debug.LogError("Home Menu not found. App.cs cannot update connection status text.");
                }
                else {
                    Debug.LogWarning("Home Menu found.");
                }
            }
            Debug.Log("Connecting...");
        }
    }

    public NetworkRunner GetRunner() {
        CheckRunner();
        return _runner;
    }

    public void Disconnect() {
        if (_runner != null) {
            connectionStatus = ConnectionStatus.Disconnecting;
            _runner.Shutdown();

        }
    }

    public async Task FindSession() {
        CheckRunner();
        StartGameResult result = await _runner.StartGame(new StartGameArgs() {
            GameMode = GameMode.Shared,
            SceneManager = networkSceneManager,
            PlayerCount = 1,
            //Scene = 2,
        });
        Debug.Log("Start Game called.");
        if (!result.Ok) {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
            Debug.LogError($"Failure Message: {result.ErrorMessage}");
        }
    }

    #region Interface Callbacks
    public void OnConnectedToServer(NetworkRunner runner) {
        Debug.Log("Connected to server.");
        connectionStatus = ConnectionStatus.Connected;
        
        homeMenu.action_message.Q<Label>().text = "Connected.";
        homeMenu.action_message.Q<Button>().RegisterCallback<ClickEvent>(ev => Disconnect()); homeMenu.action_message.Q<Button>().RegisterCallback<NavigationSubmitEvent>(ev => Disconnect());
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {
        homeMenu.action_message.Q<Label>().text = "Connect failed.";
        homeMenu.action_message.style.display = DisplayStyle.None;
        Debug.LogWarning($"Connection failed: {reason}");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner) {
        connectionStatus = ConnectionStatus.Disconnected;
        homeMenu.action_message.Q<Label>().text = "Disconnected.";
        homeMenu.action_message.style.display = DisplayStyle.None;
        Debug.LogWarning("Disconnected from server.");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
        runner.SceneManager.IsReady(runner);
        if (!runner.IsSharedModeMasterClient) return;
        if (runner.ActivePlayers.Count() == runner.SessionInfo.PlayerCount) {
            //networkSceneManager.LoadSelectedSceneObject();
            runner.SetActiveScene(2);
            Debug.Log("Begin countdown!");
        }
    }

    public void OnSceneLoadStart(NetworkRunner runner) {
        homeMenu.action_message.Q<Label>().text = "Begin countdown."; 
    }
    
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {
        connectionStatus = ConnectionStatus.Disconnected;
        homeMenu.action_message.Q<Label>().text = "Shutdown.";
        homeMenu.action_message.style.display = DisplayStyle.None;
        homeMenu.ReturnHome();
        Debug.Log("Connection shutdown.");
    }
    #endregion

    #region Unused Interface
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    #endregion
}

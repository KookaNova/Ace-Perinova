using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSceneManager : NetworkSceneManagerBase {
    List<SceneRef> sceneRefs = new List<SceneRef>();
    FinishedLoadingDelegate LevelLoaded;

    [SerializeField]
    [Tooltip("Playlist Objects to control what types of games are being selected.")]
    private ScenePlaylistObject singleplayerScenes, quickplay;
    [HideInInspector]
    public SceneObject queuedScene;
    private void Awake() {
        queuedScene = singleplayerScenes.SelectSceneObject(2);
    }
    public ScenePlaylistObject GetSinglePlayerPlaylist() {
        return singleplayerScenes;
    }

    protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished) {
        Debug.Log($"Switching Scene from {prevScene} to {newScene}");

        //Loadscreen On

        List<NetworkObject> sceneObjects = new List<NetworkObject>();


        yield return SceneManager.LoadSceneAsync(queuedScene.buildIndex, LoadSceneMode.Single);
        var loadedScene = SceneManager.GetSceneByBuildIndex(queuedScene.buildIndex);
        Debug.Log($"Loaded scene {loadedScene.name}");
        sceneObjects = FindNetworkObjects(loadedScene, disable: false);

        // Delay one frame
        yield return null;
        finished(sceneObjects);

        Debug.Log($"Switched Scene from {prevScene} to {newScene} - loaded {sceneObjects.Count} scene objects");

        //LoadScreen off
    }
}

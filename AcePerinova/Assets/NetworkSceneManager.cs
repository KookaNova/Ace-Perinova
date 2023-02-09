using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSceneManager : NetworkSceneManagerBase {
    List<SceneRef> sceneRefs = new List<SceneRef>();
    FinishedLoadingDelegate LevelLoaded;


    protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished) {
        Debug.Log($"Switching Scene from {prevScene} to {newScene}");

        //Loadscreen On

        List<NetworkObject> sceneObjects = new List<NetworkObject>();


        yield return SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        var loadedScene = SceneManager.GetSceneByBuildIndex(2);
        Debug.Log($"Loaded scene {loadedScene.name}");
        sceneObjects = FindNetworkObjects(loadedScene, disable: false);

        // Delay one frame
        yield return null;
        finished(sceneObjects);

        Debug.Log($"Switched Scene from {prevScene} to {newScene} - loaded {sceneObjects.Count} scene objects");

        //LoadScreen off
    }
}

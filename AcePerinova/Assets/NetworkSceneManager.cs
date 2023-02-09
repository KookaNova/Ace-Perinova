using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSceneManager : NetworkSceneManagerBase {
    List<SceneRef> sceneRefs = new List<SceneRef>();
    FinishedLoadingDelegate LevelLoaded;

    protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished) {
        yield return 0;
    }
}

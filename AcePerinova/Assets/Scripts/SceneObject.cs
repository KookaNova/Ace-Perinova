using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "NewSceneObject", menuName = "SceneObject")]
public class SceneObject : ScriptableObject
{
    [SerializeField]
    public SceneAsset sceneAsset;
    //possibilites:
    //Include loading screen data, loading music or character select music or general music playlists for the level.
    //Loading screen level bio. Map for the loading screen. Lots of possibilities.


    public string GetSceneName() {
        return sceneAsset.name;
    }


}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScenePlaylist", menuName = "ScenePlaylistObject")]
public class ScenePlaylistObject : ScriptableObject
{
    public List<SceneObject> scenes;

    public SceneObject FindRandomScene() {
        int selection = Random.Range(0, scenes.Count);
        return scenes[selection];
    }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScenePlaylist", menuName = "ScenePlaylistObject")]
public class ScenePlaylistObject : ScriptableObject
{
    public List<SceneObject> scenes;

    public SceneObject FindRandomSceneObject() {
        int selection = Random.Range(0, scenes.Count);
        return scenes[selection];
    }

    public int FindRandomSceneIndex() {
        int selection = Random.Range(0, scenes.Count);
        return scenes[selection].buildIndex;
    }

    public SceneObject SelectSceneObject(int index) {
        return scenes[index];
    }
}

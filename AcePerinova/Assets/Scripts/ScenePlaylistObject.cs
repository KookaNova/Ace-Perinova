using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScenePlaylist", menuName = "ScenePlaylistObject")]
public class ScenePlaylistObject : ScriptableObject
{
    public List<SceneObject> scenes;
}

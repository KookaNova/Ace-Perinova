using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AcePerinova.Utilities {
    public class SceneSelectManager : MonoBehaviour {
        public ScenePlaylistObject storyPlaylist;
        public ScenePlaylistObject quickplay;
        public SceneObject selectedScene;

        public void LoadSelectedScene() {
            SceneManager.LoadSceneAsync(selectedScene.GetSceneName());
        }
    }
}

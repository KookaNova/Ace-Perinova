using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace AcePerinova.Utilities {
    public class SceneSelectManager : MonoBehaviour {
        public SceneObject selectedScene;
        public ScenePlaylistObject storyPlaylist, quickplay;

        void Start() {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }
        public void LoadSelectedScene() {
            SceneManager.LoadSceneAsync(selectedScene.GetSceneName(), LoadSceneMode.Additive);
        }
        void LoadRandomScene(ScenePlaylistObject scenePlaylist) {
            selectedScene = scenePlaylist.FindRandomScene();
        }
    }
}

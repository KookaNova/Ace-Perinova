using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AcePerinova.Utilities {
    public class SceneSelectManager : MonoBehaviour {
        public SceneObject selectedScene;
        public ScenePlaylistObject storyPlaylist, quickplay;
        

        public void LoadSelectedScene() {
            SceneManager.LoadSceneAsync(selectedScene.GetSceneName());
        }
        private void LoadRandomScene(ScenePlaylistObject scenePlaylist) {
            selectedScene = scenePlaylist.FindRandomScene();
        }

    }
}

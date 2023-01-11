using NanoSockets;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AcePerinova.Utilities {
    public class SceneSelectManager : MonoBehaviour {
        public SceneObject homeScene, selectedScene;
        public ScenePlaylistObject storyPlaylist, quickplay;
        public float loadProgress;


        [SerializeField] CanvasGroup loadingScreen;
        [SerializeField] float transitionSpeed = 0.01f;

        int openedScene = 0, loadingScene = 1;
        AsyncOperation status;
        bool isLoading = false;

        void Start() {
            SceneManager.LoadSceneAsync(homeScene.buildIndex, LoadSceneMode.Additive);
            openedScene = homeScene.buildIndex;
        }
        public void ReturnToHomeScene() {
            selectedScene = homeScene;
            LoadSelectedScene();
        }
        public void LoadSelectedScene() {
            isLoading = true;
            SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);
            status = SceneManager.LoadSceneAsync(selectedScene.buildIndex, LoadSceneMode.Additive);
            status.allowSceneActivation = false;
            StartCoroutine(DuringLoading());
        }
        private IEnumerator LoadComplete() {
            if (openedScene > 0)SceneManager.UnloadSceneAsync(openedScene);
            SceneManager.UnloadSceneAsync(loadingScene);
            yield return new WaitForSeconds(1.5f);
            openedScene = selectedScene.buildIndex;
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(selectedScene.buildIndex));

            loadProgress = 0;
            Debug.LogFormat("Level Loaded: {0}", status.isDone);
            isLoading = false;
        }
        private void Update() {
            if (isLoading && loadingScreen.alpha < 1) {
                loadingScreen.alpha += transitionSpeed;
                if (loadingScreen.alpha >= 1) {
                    if (openedScene > 0)SceneManager.UnloadSceneAsync(openedScene);
                    status.allowSceneActivation = true;
                }
            }
            else if (!isLoading && loadingScreen.alpha > 0) { 
                loadingScreen.alpha -= transitionSpeed;
            }
        }

        IEnumerator DuringLoading() {
            loadProgress = status.progress * 100;
            Debug.LogFormat("Level loading: {0}%", loadProgress);
            if(status.isDone) {
                StartCoroutine(LoadComplete());
                yield break;
            }
            yield return new WaitForSeconds(.1f);
            StartCoroutine(DuringLoading());
        }
    }
}

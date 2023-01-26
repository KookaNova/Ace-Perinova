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

        [SerializeField] int openedScene = 0;
        AsyncOperation loadStatus;
        bool isLoading = false;

        void Start() {
            if(SceneManager.sceneCount > 1) {
                openedScene = SceneManager.GetSceneAt(0).buildIndex;
                return;
            }
            selectedScene = homeScene;
            LoadSelectedScene();
        }
        public void ReturnToHomeScene() {
            selectedScene = homeScene;
            LoadSelectedScene();
        }
        public void LoadSelectedScene() {
            isLoading = true;
            loadStatus = SceneManager.LoadSceneAsync(selectedScene.buildIndex, LoadSceneMode.Additive);
            loadStatus.allowSceneActivation = false;
            StartCoroutine(DuringLoading());
        }
        private IEnumerator LoadComplete() {
            //if (openedScene > 0)SceneManager.UnloadSceneAsync(openedScene);
            yield return new WaitForSeconds(.75f);
            openedScene = selectedScene.buildIndex;
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(selectedScene.buildIndex));

            loadProgress = 0;
            Debug.LogFormat("Level Loaded: {0}", loadStatus.isDone);
            isLoading = false;
        }
        private void Update() {
            if (isLoading && loadingScreen.alpha < 1) {
                loadingScreen.alpha += transitionSpeed;
                if (loadingScreen.alpha >= 1) {
                    if(openedScene > 0) {
                        SceneManager.UnloadScene(openedScene); //implement async later. Async gets stuck for some reason.
                        loadStatus.allowSceneActivation = true;
                    }
                    else{
                        loadStatus.allowSceneActivation = true;
                    }

                }
                
            }
            else if (!isLoading && loadingScreen.alpha > 0) { 
                loadingScreen.alpha -= transitionSpeed;
            }
        }

        IEnumerator DuringLoading() {
            loadProgress = loadStatus.progress * 100;
            Debug.LogFormat("Level loading: {0}%", loadProgress);
            if(loadStatus.isDone) {
                StartCoroutine(LoadComplete());
                yield break;
            }
            yield return new WaitForSeconds(.1f);
            StartCoroutine(DuringLoading());
        }
    }
}

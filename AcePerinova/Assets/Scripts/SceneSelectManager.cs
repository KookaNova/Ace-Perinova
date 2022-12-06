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

        [SerializeField] int countDownSec = 5;

        HomeMenuController menu;

        Coroutine countRoutine;

        private void Awake() {
            menu = FindObjectOfType<UIDocument>().rootVisualElement.Q<HomeMenuController>();
            if (menu == null) Debug.LogErrorFormat("{0}: Menu not found.", this.name);
        }
        void LoadSelectedScene() {
            SceneManager.LoadSceneAsync(selectedScene.GetSceneName());
        }
        void LoadRandomScene(ScenePlaylistObject scenePlaylist) {
            selectedScene = scenePlaylist.FindRandomScene();
        }
        public void BeginCountDown() {
            if (countRoutine != null) StopCoroutine(countRoutine);
            countRoutine = StartCoroutine(CountDown());
        }
        void CancelCountdown() {
            StopCoroutine(countRoutine);
            menu.EditActionMessage(null);
        }
        IEnumerator CountDown() {
            menu.EditActionMessage("Prepare to Launch in " + countDownSec + ".", "CANCEL");
            menu.action_message.Q<Button>().RegisterCallback<ClickEvent>(ev => CancelCountdown());
            menu.action_message.Q<Button>().RegisterCallback<NavigationSubmitEvent>(ev => CancelCountdown());

            for (int i = countDownSec; i > 0; i--) {
                menu.EditActionMessage("Prepare to Launch in " + i + ".", "CANCEL");
                yield return new WaitForSecondsRealtime(1);
            }
            menu.EditActionMessage("Launching...");
            yield return new WaitForSecondsRealtime(1);
            LoadSelectedScene();
        }
    }
}

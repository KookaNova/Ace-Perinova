using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AcePerinova.Utilities {
    public class SceneListController : VisualElement {
        #region Factory
        public new class UxmlFactory : UxmlFactory<SceneListController, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }
        #endregion

        HomeMenuController controller;
        NetworkSceneManager networkSceneManager;
        ScrollView list = new ScrollView();
        Button play = new Button();
        int countdown = 5;

        public SceneListController() {
            Initialize();   
            for (int i = 0; i < 9; i++) {
                list.Add(new SceneSelectorButton());
            }

        }
        public SceneListController(NetworkSceneManager _networkSceneManager) {
            Initialize();
            networkSceneManager = _networkSceneManager;
            GenerateButtons();
            
        }
        public void Initialize() {
            controller = Object.FindObjectOfType<UIDocument>().rootVisualElement.Q<HomeMenuController>();
            AddToClassList("playlist");
            list.AddToClassList("playlist-list");
            list.mode = ScrollViewMode.Horizontal;
            Add(list);
            play.text = "PLAY";
            play.AddToClassList("button");
            Add(play);
            play.SetEnabled(false);
            play.RegisterCallback<ClickEvent>(ev => controller.BeginCountdown(countdown));
            play.RegisterCallback<NavigationSubmitEvent>(ev => controller.BeginCountdown(countdown));
        }

        public void GenerateButtons() {
            foreach (var scene in networkSceneManager.GetSinglePlayerPlaylist().scenes) {
                SceneSelectorButton sceneButton = new SceneSelectorButton(scene);
                list.Add(sceneButton);
                sceneButton.RegisterCallback<ClickEvent>(ev => { networkSceneManager.queuedScene = scene; play.SetEnabled(true); play.Focus(); });
                sceneButton.RegisterCallback<NavigationSubmitEvent>(ev => { networkSceneManager.queuedScene = scene; play.SetEnabled(true); play.Focus(); });
                

            }
        }
    }
}

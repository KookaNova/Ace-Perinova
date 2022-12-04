using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AcePerinova.Utilities {
    public class SceneListController : VisualElement {
        #region Factory
        public new class UxmlFactory : UxmlFactory<SceneListController, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }
        #endregion

        ScenePlaylistObject scenePlaylist;
        SceneSelectManager sceneSelectManager;
        ScrollView list = new ScrollView();
        Button play = new Button();

        public SceneListController() {
            Initialize();
            for (int i = 0; i < 9; i++) {
                list.Add(new SceneSelectorButton());
            }

        }
        public SceneListController(ScenePlaylistObject scenePlaylistObject, SceneSelectManager _sceneSelectManager) {
            Initialize();
            scenePlaylist = scenePlaylistObject;
            sceneSelectManager = _sceneSelectManager;
            GenerateButtons();
            
        }
        public void Initialize() {
            AddToClassList("playlist");
            list.AddToClassList("playlist-list");
            list.mode = ScrollViewMode.Horizontal;
            Add(list);
            play.text = "PLAY";
            play.AddToClassList("button");
            Add(play);
            play.RegisterCallback<ClickEvent>(ev => sceneSelectManager.LoadSelectedScene());
            play.RegisterCallback<NavigationSubmitEvent>(ev => sceneSelectManager.LoadSelectedScene());
        }

        public void GenerateButtons() {
            foreach (var scene in scenePlaylist.scenes) {
                SceneSelectorButton sceneButton = new SceneSelectorButton(scene);
                list.Add(sceneButton);
                sceneButton.RegisterCallback<ClickEvent>(ev => { sceneSelectManager.selectedScene = scene; play.Focus(); });
                sceneButton.RegisterCallback<NavigationSubmitEvent>(ev => { sceneSelectManager.selectedScene = scene; play.Focus(); });
                

            }
        }
    }
}

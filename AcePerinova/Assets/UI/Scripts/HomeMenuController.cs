using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UIElements;

namespace AcePerinova.Utilities {
    public class HomeMenuController : VisualElement {
        #region Factory
        public new class UxmlFactory : UxmlFactory<HomeMenuController, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }
        #endregion

        Screen s_title;
        Screen s_home;
        Screen s_story;
        Screen s_multiplayer;
        Screen s_scene_select;

        VisualElement nav;
        Button b_back;

        SceneSelectManager sceneSelectManager;

        public HomeMenuController() {
            this.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            sceneSelectManager = Object.FindObjectOfType<SceneSelectManager>();
        }

        private void OnGeometryChanged(GeometryChangedEvent evt) {
            Initialize();
            AssignScreens();
            RegisterButtonCallbacks();


            OpenScreen(s_title);

            this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        void Initialize() {
            //Brings focus to an element when using keyboard or gamepad and nothing currently has focus.
            this.RegisterCallback<NavigationMoveEvent>(ev => {
                if (focusController.focusedElement == null) {
                    this.Q<Button>()?.Focus();
                }
            });
        }

        void AssignScreens() {
            //Screen Elements
            s_title = new Screen(this.Q("s-title"));
            s_home = new Screen(this.Q("s-home"), true);
            s_story = new Screen(this.Q("s-story"), s_home, true);
            s_multiplayer = new Screen(this.Q("s-multiplayer"), s_home);
            //story screen
            s_scene_select = new Screen(this.Q("s-scene-select"), s_story);
            s_scene_select.visualElement.Add(new SceneListController(sceneSelectManager.storyPlaylist, sceneSelectManager));
            
            //generic
            nav = this.Q("nav");
            b_back = this.Q<Button>("b-back");
        }

        void RegisterButtonCallbacks() {
            //Title
            s_title.visualElement.RegisterCallback<NavigationSubmitEvent>(ev => TitleClicked());
            s_title.visualElement.RegisterCallback<ClickEvent>(ev => TitleClicked());
            //Home Screen
            s_home.visualElement?.Q<Button>("b-story").RegisterCallback<NavigationSubmitEvent>(ev => OpenScreen(s_story));
            s_home.visualElement?.Q<Button>("b-story").RegisterCallback<ClickEvent>(ev => OpenScreen(s_story));
            s_home.visualElement?.Q<Button>("b-multiplayer").RegisterCallback<NavigationSubmitEvent>(ev => OpenScreen(s_multiplayer));
            s_home.visualElement?.Q<Button>("b-multiplayer").RegisterCallback<ClickEvent>(ev => OpenScreen(s_multiplayer));
            //Story Screen
            s_story.visualElement?.Q<Button>("b-scene-select").RegisterCallback<NavigationSubmitEvent>(ev => OpenScreen(s_scene_select));
            s_story.visualElement?.Q<Button>("b-scene-select").RegisterCallback<ClickEvent>(ev => OpenScreen(s_scene_select));
            //Multiplayer


        }

        void TitleClicked() {
            s_title.visualElement.AddToClassList("anim-opacity-out");
            s_title.visualElement.RegisterCallback<TransitionEndEvent>(ev => { OpenScreen(s_home); });


        }

        void CloseAllScreens() {
            s_title.Close();
            s_home.Close();
            s_story.Close();
            s_multiplayer.Close();
            s_scene_select.Close();
            nav.style.display = DisplayStyle.None;

            b_back.style.display = DisplayStyle.None;


        }

        void OpenScreen(Screen screen) {
            CloseAllScreens();
            screen.visualElement.style.display = DisplayStyle.Flex;
            screen.visualElement.BringToFront();
            screen.visualElement.RegisterCallback<NavigationMoveEvent>(ev => {
                if (focusController.focusedElement == null) {
                    screen.visualElement.Q<Button>()?.Focus();
                }
            });

            if (screen.useNav) nav.style.display = DisplayStyle.Flex;
            if (screen.backLocation != null) {
                b_back.style.display = DisplayStyle.Flex;
                b_back.RegisterCallback<NavigationSubmitEvent>(ev => OnBackButton(screen));
                b_back.RegisterCallback<ClickEvent>(ev => OnBackButton(screen));
                this.RegisterCallback<NavigationCancelEvent>(ev => OnBackButton(screen));
            }

        }

        void OnBackButton(Screen screen) {
            b_back.UnregisterCallback<NavigationSubmitEvent>(ev => OnBackButton(screen));
            b_back.UnregisterCallback<ClickEvent>(ev => OnBackButton(screen));
            this.UnregisterCallback<NavigationCancelEvent>(ev => OnBackButton(screen));
            OpenScreen(screen.backLocation);
        }

        class Screen {
            public VisualElement visualElement;
            public Screen backLocation = null;
            public bool useNav = false;

            public Screen(VisualElement _visualElement, Screen _backLocation) {
                visualElement = _visualElement;
                backLocation = _backLocation;
            }
            public Screen(VisualElement _visualElement) {
                visualElement = _visualElement;
            }

            public Screen(VisualElement _visualElement, bool _useNav) {
                visualElement = _visualElement;
                useNav = _useNav;
            }
            public Screen(VisualElement _visualElement, Screen _backLocation, bool _useNav) {
                visualElement = _visualElement;
                backLocation = _backLocation;
                useNav = _useNav;
            }

            public void Close() {
                visualElement.style.display = DisplayStyle.None;
            }
        }

    }
}
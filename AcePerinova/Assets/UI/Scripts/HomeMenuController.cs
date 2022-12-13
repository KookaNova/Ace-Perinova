using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

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

        IVisualElementScheduledItem timer = null;

        Screen nav;
        public VisualElement action_message;
        Button b_back;

        int countdown = 0;

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
        #region Assign and Register
        void AssignScreens() {
            //Screen Elements
            s_title = new Screen(this.Q("s-title"), Vector2.zero);
            s_home = new Screen(this.Q("s-home"), true, new Vector2(-3000,0));
            s_story = new Screen(this.Q("s-story"), s_home, true, new Vector2(-3000, 0));
            s_multiplayer = new Screen(this.Q("s-multiplayer"), s_home, new Vector2(-3000, 0));
            //story screen
            s_scene_select = new Screen(this.Q("s-scene-select"), s_story, Vector2.zero);
            s_scene_select.visualElement.Add(new SceneListController(sceneSelectManager.storyPlaylist, sceneSelectManager));
            //generic
            nav = new Screen(this.Q("nav"), new Vector2(0,-500));
            action_message = this.Q("action-message"); 
            action_message.style.display = DisplayStyle.None;
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
        #endregion
        #region Opening and Closing
        void TitleClicked() {
            s_title.Close();
            OpenScreen(s_home);
        }
        void CloseAllScreens() {
            s_title.Close();
            s_home.Close();
            s_story.Close();
            s_multiplayer.Close();
            s_scene_select.Close();

            nav.Close();
            b_back.style.display = DisplayStyle.None;
        }
        void OpenScreen(Screen screen) {
            CloseAllScreens();
            screen.Open();
            screen.visualElement.RegisterCallback<NavigationMoveEvent>(ev => {
                if (focusController.focusedElement == null) {
                    screen.visualElement.Q<Button>()?.Focus();
                }
            });
            if (screen.useNav) {
                nav.Open();
            }
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
        #endregion
        #region Methods
        public void EditActionMessage(string messageText) {
            action_message.style.display = DisplayStyle.Flex;
            action_message.Q<Button>().style.display = DisplayStyle.None;
            action_message.Q<Label>().text = messageText;
            if(messageText == null) {
                action_message.style.display = DisplayStyle.None;
            }
        }
        public void EditActionMessage(string messageText, string interactionText) {
            action_message.style.display = DisplayStyle.Flex;
            action_message.Q<Button>().style.display = DisplayStyle.Flex;
            action_message.Q<Label>().text = messageText;
            action_message.Q<Button>().text = interactionText;
        }

        public void BeginCountdown(int seconds) {
            EditActionMessage("Prepare to Launch in " + seconds + ".", "CANCEL");
            countdown = seconds;
            timer = schedule.Execute(count);
            timer.Every(1000);
            timer.ForDuration(seconds * 1000);
        }

        public void CancelCountdown() {
            timer = null;
            EditActionMessage(null);
            action_message.Q<Button>().RegisterCallback<ClickEvent>(ev => CancelCountdown());
            action_message.Q<Button>().RegisterCallback<NavigationSubmitEvent>(ev => CancelCountdown());
        }

        void count() {
            countdown--;
            EditActionMessage("Prepare to Launch in " + countdown + ".", "CANCEL");
            if (countdown == 0) {
                EditActionMessage("Launching...");
                sceneSelectManager.LoadSelectedScene();
            }

        }

        #endregion
        class Screen {
            public VisualElement visualElement;
            public Screen backLocation = null;
            public Vector2 transition = new Vector2(0, 0);
            public bool useNav = false;
            public Screen(VisualElement _visualElement, Screen _backLocation, Vector2 _transition) {
                visualElement = _visualElement;
                backLocation = _backLocation;
                transition = _transition;
            }
            public Screen(VisualElement _visualElement, Vector2 _transition) {
                visualElement = _visualElement;
                transition = _transition;
            }
            public Screen(VisualElement _visualElement, bool _useNav, Vector2 _transition) {
                visualElement = _visualElement;
                useNav = _useNav;
                transition = _transition;
            }
            public Screen(VisualElement _visualElement, Screen _backLocation, bool _useNav, Vector2 _transition) {
                visualElement = _visualElement;
                backLocation = _backLocation;
                useNav = _useNav;
                transition = _transition;
            }
            public void Open() {
                visualElement.style.opacity = new StyleFloat(1f);
                visualElement.style.display = DisplayStyle.Flex;
                visualElement.style.translate = new StyleTranslate(new Translate(0, 0, 0));
                visualElement.BringToFront();
            }
            public void Close() {
                visualElement.style.opacity = new StyleFloat(-1f);
                visualElement.style.translate = new StyleTranslate(new Translate(transition.x, transition.y, 0));
                visualElement.style.display = DisplayStyle.None;
            }
        }
    }
}

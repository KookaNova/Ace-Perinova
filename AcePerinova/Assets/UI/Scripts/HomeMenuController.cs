using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AcePerinova.Utilities {
    public class HomeMenuController : VisualElement {
        #region Factory
        public new class UxmlFactory : UxmlFactory<HomeMenuController, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }
        #endregion

        App app = App.FindInstance();

        Screen s_title,
            s_home,
            s_story,
            s_multiplayer,
            s_scene_select,
            s_waiting_room;

        IVisualElementScheduledItem timer = null;

        public VisualElement action_message;
        Button b_back;

        int countdown = 0;

        NetworkSceneManager networkSceneManager;

        #region BaseSetup

        public HomeMenuController() {
            this.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            networkSceneManager = app?.networkSceneManager;
            

            if (app == null) {
#if UNITY_EDITOR
                if (Application.isPlaying && !Application.isEditor) {
                    SceneManager.LoadSceneAsync(0);
                }
#else
                SceneManager.LoadSceneAsync(0);
#endif
            }
        }
        private void OnGeometryChanged(GeometryChangedEvent evt) {
            Initialize();
            AssignScreens();
            RegisterButtonCallbacks();
#if UNITY_EDITOR
            if (Application.isPlaying) {
                OpenScreen(s_title);
            }
#else
            OpenScreen(s_title);
#endif
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
        #endregion
        #region Assign and Register
        void AssignScreens() {
            //Screen Elements
            s_title = new Screen(this.Q("s-title"), Vector2.zero);
            s_home = new Screen(this.Q("s-home"), new Vector2(-3000, 0));
            s_story = new Screen(this.Q("s-story"), s_home, new Vector2(-3000, 0));
            s_multiplayer = new Screen(this.Q("s-multiplayer"), s_home, new Vector2(-3000, 0));
            s_waiting_room = new Screen(this.Q("s-waiting-room"), s_multiplayer, new Vector2(3000, 0));
            //story screen
            s_scene_select = new Screen(this.Q("s-scene-select"), s_story, Vector2.zero);
            if (networkSceneManager != null) s_scene_select.visualElement.Add(new SceneListController(networkSceneManager));
            //generic
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
            s_multiplayer.visualElement?.Q<Button>("b-quickplay").RegisterCallback<ClickEvent>(ev => Quickplay());
        }
        #endregion
        #region Multiplayer Buttons
        void Quickplay() {

            if(app == null) {
                app = App.FindInstance();
                app.SetHomeMenu(this);
            }
            
            app?.FindSession();
            OpenScreen(s_waiting_room);
            action_message.style.display = DisplayStyle.Flex;
            action_message.Q<Button>().text = "DISCONNECT";
            action_message.Q<Label>().text = "Establishing connection...";
            //s_waiting_room.visualElement.Q<Label>("l-waiting-status").text = "Establishing connection...";
        }
        #endregion
        #region Opening and Closing
        public void ReturnHome() {
            CloseAllScreens();
            s_home.Open();
            s_home.visualElement.RegisterCallback<NavigationMoveEvent>(ev => {
                if (focusController.focusedElement == null) {
                    s_home.visualElement.Q<Button>()?.Focus();
                }
            });
        }
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
            s_waiting_room.Close();

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
            if (messageText == null) {
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

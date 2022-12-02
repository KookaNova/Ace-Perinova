using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UIElements;

public class HomeMenuController : VisualElement {
    #region Factory
    public new class UxmlFactory : UxmlFactory<HomeMenuController, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion

    Screen s_title;
    Screen s_home;
    Screen s_story;
    Screen s_scene_select;

    VisualElement nav;
    Button b_back;

    public HomeMenuController() {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt) {
        foreach (VisualElement v in this.Children()) {
            if (v.focusable) {
                v.RegisterCallback<ClickEvent>(ev => v.Focus());
            }
        }
        this.RegisterCallback<NavigationMoveEvent>(ev => {
            if (focusController.focusedElement == null) {
                this.Q<Button>()?.Focus();
            }
        });
        //Screen Elements
        s_title = new Screen(this.Q("s-title"));
        s_home = new Screen(this.Q("s-home"),true);
        s_story = new Screen(this.Q("s-story"), s_home, true);
        //story screen
        s_scene_select = new Screen(this.Q("s-scene-select"), s_story);
        //generic
        nav = this.Q("nav");
        b_back = this.Q<Button>("b-back");

        //Home Screen
        s_home.visualElement?.Q<Button>("b-story").RegisterCallback<NavigationSubmitEvent>(ev => OpenScreen(s_story));

        //Story Screen
        s_story.visualElement?.Q<Button>("b-scene-select").RegisterCallback<NavigationSubmitEvent>(ev => OpenScreen(s_scene_select));

        s_title.visualElement.RegisterCallback<NavigationSubmitEvent>(ev => OpenScreen(s_home));

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    void CloseAllScreens() {
        s_title.Close();
        s_home.Close();
        s_story.Close();
        s_scene_select.Close();
        nav.style.display = DisplayStyle.None;

        b_back.style.display = DisplayStyle.None;


    }

    void OpenScreen(Screen screen) {
        CloseAllScreens();
        screen.visualElement.style.display = DisplayStyle.Flex;
        screen.visualElement.BringToFront();
        if (screen.useNav) nav.style.display = DisplayStyle.Flex;
        if (screen.backLocation != null) {
            b_back.style.display = DisplayStyle.Flex;
            b_back.RegisterCallback<NavigationSubmitEvent>(ev => OnBackButton(screen));
            RegisterCallback<NavigationCancelEvent>(ev => OnBackButton(screen));
        }
        screen.visualElement.RegisterCallback<NavigationMoveEvent>(ev => {
            if(focusController.focusedElement == null) {
                screen.visualElement.Q<Button>()?.Focus();
            }
        });
    }

    void OnBackButton(Screen screen) {
        b_back.UnregisterCallback<NavigationSubmitEvent>(ev => OnBackButton(screen));
        UnregisterCallback<NavigationCancelEvent>(ev => OnBackButton(screen));
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

using UnityEngine;
using UnityEngine.UIElements;

namespace AcePerinova.Utilities {
    public class SceneSelectorButton : VisualElement {
        #region Factory
        public new class UxmlFactory : UxmlFactory<SceneSelectorButton, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }
        #endregion

        public string sceneName;

        Label title = new Label("l-title");

        public SceneSelectorButton() {
            Initialize();
        }

        public SceneSelectorButton(SceneObject scene) {
            Initialize();
            //create button using the scene image as a background, scene name, and assign appropriate styles.
            sceneName = scene.name;
            title.text = sceneName;

        }

        private void Initialize() {
            title.text = "Debug Scene Title";
            Add(title);

            AddToClassList("button");
            AddToClassList("button-picture");
            AddToClassList("text-t2");
        }

    }
}
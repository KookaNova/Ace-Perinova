using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBuddy : MonoBehaviour
{
    [SerializeField] GameObject ui;
    void Start()
    {
        if(SceneManager.sceneCount <= 1) {
            
            SceneManager.LoadScene(0, LoadSceneMode.Additive);
            ui.SetActive(false);
            ui.SetActive(true);
        }
    }
}

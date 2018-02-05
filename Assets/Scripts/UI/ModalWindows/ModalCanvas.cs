using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.GameManagement;

namespace Assets.Scripts.UI.ModalWindows
{
    public class ModalCanvas : MonoBehaviour
    {
        public Canvas canvas;
        
        protected static ModalCanvas instance;

        protected void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                FindSceneCamera();
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }

        protected void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            FindSceneCamera();
        }

        /// <summary>
        /// Find camera on scene and set it to canvas target camera 
        /// </summary>
        protected void FindSceneCamera()
        {
            canvas.worldCamera = Camera.main;
        }

    }
}


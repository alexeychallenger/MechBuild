using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.UI;
using Assets.Scripts.Utils;
using Assets.Scripts.Sound;
using Assets.Scripts.UI.ModalWindows;
using Assets.Scripts.FileManagement;

namespace Assets.Scripts.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public GameData gameData;
        public SoundManager soundManager;
        public ModalManager modalManager;
        public SceneLoader sceneLoader;
        public DebugTest debugTest;
        public ErrorInfoDisplay errorInfoDisplay;
        public FileManager fileManager;

        protected static GameManager instance;

        protected void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}



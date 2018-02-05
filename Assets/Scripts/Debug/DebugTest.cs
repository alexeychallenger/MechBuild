using Assets.Scripts.GameManagement;
using Assets.Scripts.UI.ModalWindows.Parameters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class DebugTest : MonoBehaviour
    {
        protected GameManager gameManager;

        protected void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameManager.sceneLoader.LoadScene(Scenes.Menu);
            }
            if (Input.GetKeyDown(KeyCode.Pause))
            {
                Debug.Break();
            }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                gameManager.sceneLoader.LoadScene(Scenes.LoadingScene);
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                gameManager.sceneLoader.LoadScene(Scenes.DevScene);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                gameManager.modalManager.CreateModal(new InfoModalParameters("Info", "Test message"));
            }
        }
    }
}

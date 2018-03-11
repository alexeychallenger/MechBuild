using Assets.Scripts.GameManagement;
using Assets.Scripts.UI.ModalWindows.Parameters;
using System;
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

            if (Input.GetKeyDown(KeyCode.F10))
            {
                Time.timeScale = Mathf.Clamp(Time.timeScale - 0.05f, 0f, 10f);
            }

            if (Input.GetKeyDown(KeyCode.F11))
            {
                Time.timeScale = 1f;
            }

            if (Input.GetKeyDown(KeyCode.F12))
            {
                Time.timeScale = Mathf.Clamp(Time.timeScale + 0.05f, 0f, 10f);
            }

            string gravityCoefStr = GInputField.inputField != null ? GInputField.inputField.text : null;
            float gravityCoef = gravityCoefStr != null ? Convert.ToSingle(gravityCoefStr) : 9.8f;

            if (Input.GetKeyDown(KeyCode.Home))
            {
                Physics.gravity = Vector3.up *  gravityCoef;
            }
            if (Input.GetKeyDown(KeyCode.End))
            {
                Physics.gravity = Vector3.down * gravityCoef;
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Physics.gravity = Vector3.left * gravityCoef;
            }
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                Physics.gravity = Vector3.right * gravityCoef;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameManagement
{
    public class SceneLoader : MonoBehaviour
    {
        
        public void LoadScene(Scenes scene)
        {
            StartCoroutine(LoadSceneAsync(scene));
        }

        /// <summary>
        /// Loads loading scene where preload next scene data, then loads target scene
        /// </summary>
        /// <param name="scene">scene to load</param>
        /// <returns></returns>
        public IEnumerator LoadSceneAsync(Scenes scene)
        {
            yield return SceneManager.LoadSceneAsync(Scenes.LoadingScene.ToString());
            
            //TODO scene data preload methods

            yield return SceneManager.LoadSceneAsync(scene.ToString());
        }
    }
}




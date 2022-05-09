using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZombieDefense
{
    public class GameOverPanel : MonoBehaviour
    {
        public void RestartButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void MenuButton()
        {
            //SceneManager.LoadScene(0);
            Debug.Log("MenuButton");
        }

        public void ExitButton()
        {
#if UNITY_EDITOR
            Debug.Log("GAMEOVER");
            UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();
#endif

        }
    }
}

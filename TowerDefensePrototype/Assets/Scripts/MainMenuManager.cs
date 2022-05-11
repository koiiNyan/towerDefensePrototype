using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZombieDefense
{
    public class MainMenuManager : MonoBehaviour
    {
        public void StartGameButton()
        {
            SceneManager.LoadScene(1);
        }
    }
}

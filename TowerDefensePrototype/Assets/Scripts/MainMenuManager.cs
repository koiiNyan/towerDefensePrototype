using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ZombieDefense
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _menuPanel;
        [SerializeField]
        private GameObject _settingsPanel;
        public void StartGameButton()
        {
            SceneManager.LoadScene(1);
        }

        public void ExitGameButton()
        {
#if UNITY_EDITOR
            Debug.Log("GAMEOVER");
            UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();
#endif

        }

        public void SetDifficulty(Dropdown DifficultyDropDown)
        {
            Settings.Instance.DifficultyLevel = DifficultyDropDown.value;
            Debug.Log(Settings.Instance.DifficultyLevel);
        }

        public void SetVolume(Slider VolumeSlider)
        {
            var audio = GetComponent<AudioSource>();
            audio.volume = VolumeSlider.value;
            Settings.Instance.VolumeValue = VolumeSlider.value;
            Debug.Log(Settings.Instance.VolumeValue);
        }

        public void OpenCloseSettings()
        {
            _settingsPanel.SetActive(!_settingsPanel.activeInHierarchy);
            _menuPanel.SetActive(!_menuPanel.activeInHierarchy);
        }
    }
}

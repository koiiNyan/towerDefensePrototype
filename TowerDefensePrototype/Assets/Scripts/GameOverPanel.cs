using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ZombieDefense
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField]
        private Text _scoreText;
        private int _playerScore;
        [SerializeField]
        private Text _highscoresText;
        [SerializeField]
        private Button _saveBtn;
        [SerializeField]
        private InputField _playerName;
        [SerializeField]
        private Text _gameOverText;



        private void Awake()
        {
            PrintScores();
        }

        private void PrintScores()
        {
            string scoresString = string.Empty;

            var scores = SaveLoad.GetHighestScores();

            if (scores == null) scoresString = "No scores recorded";

            else
            {
                foreach (var score in scores)
                    scoresString += $"{score.Name}: {score.Points}\n";
            }
            

            _highscoresText.text = scoresString;
        }


        public void SetPlayerScore(int value) => _playerScore = value;


        public void SaveButton()
        {
            if (_playerName.text == "") return;
            else
            {
                SaveLoad.SaveScore(_playerName.text, _playerScore);
                _saveBtn.interactable = false;
                PrintScores();
            }
        }

        public void RestartButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void MenuButton()
        {
            SceneManager.LoadScene(0);
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

        public void UpdateScoreText()
        {
            _scoreText.text = $"Your Score: {_playerScore}";
        }

        public void UpdateGameOverText(bool IsWin) => _gameOverText.text = IsWin ? "YOU WIN!" : "YOU LOST!";

    }
}

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

        public void UpdateScoreText()
        {
            _scoreText.text = $"Your Score: {_playerScore}";
        }
    }
}

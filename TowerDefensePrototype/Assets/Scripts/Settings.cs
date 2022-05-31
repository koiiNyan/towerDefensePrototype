using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZombieDefense
{
    public class Settings : MonoBehaviour
    {
        public static Settings Instance;
        public int DifficultyLevel;
        public float VolumeValue;


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}


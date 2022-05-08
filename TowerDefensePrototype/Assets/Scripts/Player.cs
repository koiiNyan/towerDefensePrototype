using System.Collections;
using TMPro;
using UnityEngine;


namespace ZombieDefense
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private int _currentMoney;
        [SerializeField]
        private TextMeshProUGUI _moneyText;
        [SerializeField]
        private GameObject _turretUpgradeMenu;
        public int Money
        {
            get => _currentMoney;
            private set
            {
                if (value < 0)
                {

                    Debug.LogError("Money can't be less than 0");
                }
                else
                {
                    _currentMoney = value;
                }
            }
        }

        private int _moneyPerSec = 5;

        private int _zombiesMissed = 0;
        private const int _zombiesMissedToLose = 20;

        private void Awake()
        {
            _currentMoney = 20;
            UpdateMoneyText();
            StartCoroutine(AddMoneyPassively());
        }

        private IEnumerator AddMoneyPassively()
        {
            while (true)
            {
                yield return new WaitForSeconds(5f);
                _currentMoney += _moneyPerSec;
                UpdateMoneyText();
            }
        }

        private void UpdateMoneyText()
        {
            _moneyText.text = _currentMoney.ToString();
            if (_turretUpgradeMenu.activeInHierarchy) _turretUpgradeMenu.GetComponent<TurretUpgradeMenu>().EnoughMoney(_currentMoney);
        }

        public void AddMoney(int money)
        {
            _currentMoney += money;
            UpdateMoneyText();

        }


        public void SetZombieMissed()
        {
            _zombiesMissed++;
            if (_zombiesMissed >= _zombiesMissedToLose) GameOverEventHandler?.Invoke();
        }

        public event GameOverEvent GameOverEventHandler;
        public delegate void GameOverEvent();

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieDefense
{
    public class TurretUpgradeMenu : MonoBehaviour
    {
        private AttackerTurret _chosenTurret;
        public AttackerTurret ChosenTurret { get => _chosenTurret; set => _chosenTurret = value; }

        [SerializeField]
        private Text _turretLevelText;
        [SerializeField]
        private Text _turretCostText;
        [SerializeField]
        private Text _turretHPText;
        [SerializeField]
        private Text _turretADText;
        [SerializeField]
        private Text _turretASText;
        [SerializeField]
        private Button[] _upgradeButtons;

        [SerializeField]
        private int _attackDamagePerLvl = 2;
        [SerializeField]
        private int _attackSpeedPerLvl = 2;

        private Player _player;

        [SerializeField]
        private GameObject _turretStatsPanel;
        [SerializeField]
        private GameObject _turretTypePanel;

        private void Awake()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        private void OnEnable()
        {
            _turretStatsPanel.SetActive(true);
            _turretTypePanel.SetActive(false);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        public void UpdateStatsText()
        {
            _turretLevelText.text = $"Current Level: {_chosenTurret.CurrentLevel}";
            _turretCostText.text = $"Turret Cost: {_chosenTurret.Cost}";
            _turretHPText.text = $"Health: {_chosenTurret.BasicHp}";
            _turretADText.text = $"Damage: {_chosenTurret.AttackDamage}";
            _turretASText.text = $"Attack Speed: {_chosenTurret.AttackSpeed}";


            if (_chosenTurret.Cost > _player.Money) SetUpgradeButtonActivity(false);
            if (_chosenTurret.CurrentLevel >= 5 && _chosenTurret.TurretType == AttackerType.None) OpenTurretType();


        }

        public void EnoughMoney(int money)
        {
            if (money >= _chosenTurret.Cost) SetUpgradeButtonActivity(true);
        }

        public void SetUpgradeButtonActivity(bool active)
        {
            foreach (Button btn in _upgradeButtons)
                btn.interactable = active;
        }

        public void UpdateAD()
        {
            _chosenTurret.AttackDamage += _attackDamagePerLvl;
            _chosenTurret.UpdateLvl();
            UpdateStatsText();
        }

        public void UpdateAS()
        {
            _chosenTurret.AttackSpeed += _attackSpeedPerLvl;
            _chosenTurret.UpdateLvl();
            UpdateStatsText();
        }

        public void UpdateHP()
        {
            _chosenTurret.UpdateHp();
            _chosenTurret.UpdateLvl();
            UpdateStatsText();
        }

        private void OpenTurretType()
        {
            // Открывать плашку с выбором башни (список)
            // В зависимости от выбранного значения в списке, показывать информационные статы
            // При нажатии на плюс вызывать метод из башни на изменение статов башни
            
            SetUpgradeButtonActivity(false);
            _turretStatsPanel.SetActive(false);
            _turretTypePanel.SetActive(true);

        }

        public void SetTurretType()
        {

        }

    }
}
    

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

        private void Awake()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
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


            if (_chosenTurret.Cost > _player.Money) SetButtonActivity(false);
            if (_chosenTurret.CurrentLevel >= 5) SetButtonActivity(false);


        }

        public void EnoughMoney(int money)
        {
            if (money >= _chosenTurret.Cost) SetButtonActivity(true);
        }

        public void SetButtonActivity(bool active)
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

    }
}
    

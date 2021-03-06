using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieDefense
{
    public class TurretUpgradeMenu : MonoBehaviour
    {
        private FarmerTurret _chosenTurretFarmer;
        public FarmerTurret ChosenTurretFarmer { get => _chosenTurretFarmer; set => _chosenTurretFarmer = value; }

        private AttackerTurret _chosenTurretAttacker;
        public AttackerTurret ChosenTurretAttacker { get => _chosenTurretAttacker; set => _chosenTurretAttacker = value; }


       // private CellType _cellType;
      //  public CellType CellType { get => _cellType; set => _cellType = value; }

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
        private Button[] _upgradeAttackerButtons;

        [SerializeField]
        private Button[] _upgradeFarmerButtons;
        [SerializeField]
        private Button _updateHealFarmer;
        [SerializeField]
        private Button _updateMoneyPerTickFarmer;
        [SerializeField]
        private Button _updateMoneyPerZombieFarmer;

        [SerializeField]
        private int _attackDamagePerLvl = 2;
        [SerializeField]
        private int _attackSpeedPerLvl = 2;

        private Player _player;

        [SerializeField]
        private GameObject _turretStatsPanel;
        [SerializeField]
        private GameObject _turretTypePanel;

        [SerializeField]
        private GameObject _turretStatsPanelAttacker;
        [SerializeField]
        private GameObject _turretStatsPanelFarmer;

        [SerializeField]
        private Text _turretLevelTextFarmer;
        [SerializeField]
        private Text _turretCostTextFarmer;


        private void Awake()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        private void OnEnable()
        {

            OpenPanels();

            SetUpgradeButtonActivityAttacker(true);
            UpdateStatsText();
        }


        private void OpenPanels()
        {
            _turretStatsPanel.SetActive(true);
            _turretTypePanel.SetActive(false);

        }

        public void OpenPanels(AttackerTurret attackerTurret)
        {
            _chosenTurretAttacker = attackerTurret;
            _chosenTurretFarmer = null;
            _turretStatsPanelFarmer.SetActive(false);
            _turretStatsPanelAttacker.SetActive(true);
        }

        public void OpenPanels(FarmerTurret farmerTurret)
        {
            _chosenTurretAttacker = null;
            _chosenTurretFarmer = farmerTurret;
            _turretStatsPanelFarmer.SetActive(true);
            _turretStatsPanelAttacker.SetActive(false);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        public void UpdateStatsText()
        {
            if (_chosenTurretAttacker != null)
            {
                if (_chosenTurretAttacker.Cost > _player.Money || _chosenTurretAttacker.CurrentLevel >= _chosenTurretAttacker.TurretMaxLevel) SetUpgradeButtonActivityAttacker(false);
                _turretLevelText.text = $"Current Level: {_chosenTurretAttacker.CurrentLevel}";
                _turretCostText.text = $"Turret Cost: {_chosenTurretAttacker.Cost}";
                _turretHPText.text = $"Health: {_chosenTurretAttacker.BasicHp}";
                _turretADText.text = $"Damage: {_chosenTurretAttacker.AttackDamage}";
                _turretASText.text = $"Attack Speed: {_chosenTurretAttacker.AttackSpeed}";

                if (_chosenTurretAttacker.CurrentLevel >= 5 && _chosenTurretAttacker.TurretType == AttackerType.None) OpenTurretType();
            }

            if (_chosenTurretFarmer != null)
            {
                if (_chosenTurretFarmer.Cost > _player.Money || _chosenTurretFarmer.CurrentLevel >= _chosenTurretFarmer.TurretMaxLevel) SetUpgradeButtonActivityFarmer(false);

                _turretLevelTextFarmer.text = $"Current Level: {_chosenTurretFarmer.CurrentLevel}";
                _turretCostTextFarmer.text = $"Turret Cost: {_chosenTurretFarmer.Cost}";

            }

        }

        public void EnoughMoney(int money)
        {
            if (_chosenTurretAttacker != null)
            {
                if (money >= _chosenTurretAttacker.Cost && _chosenTurretAttacker.CurrentLevel < _chosenTurretAttacker.TurretMaxLevel) SetUpgradeButtonActivityAttacker(true);
            }

            if (_chosenTurretFarmer != null)
            {
                if (money >= _chosenTurretFarmer.Cost && _chosenTurretFarmer.CurrentLevel < _chosenTurretFarmer.TurretMaxLevel) SetUpgradeButtonActivityFarmer(true);
                if (_chosenTurretFarmer.ChangedMoneyPerTick) SetButtonActivity(_updateMoneyPerTickFarmer, false);
                if (_chosenTurretFarmer.ChangedMoneyPerZombie) SetButtonActivity(_updateMoneyPerZombieFarmer, false);
                if (_chosenTurretFarmer.IsHealer) SetButtonActivity(_updateHealFarmer, false);
            }
        }

        private void SetUpgradeButtonActivityAttacker(bool active)
        {
            foreach (Button btn in _upgradeAttackerButtons)
                btn.interactable = active;
        }

        private void SetUpgradeButtonActivityFarmer(bool active)
        {
            foreach (Button btn in _upgradeFarmerButtons)
                btn.interactable = active;
        }

        private void SetButtonActivity(Button btn, bool active)
        {
            btn.interactable = active;
        }

        public void UpdateAD()
        {
            _chosenTurretAttacker.AttackDamage += _attackDamagePerLvl;
            _chosenTurretAttacker.UpdateLvl();
            UpdateStatsText();
        }

        public void UpdateAS()
        {
            _chosenTurretAttacker.AttackSpeed += _attackSpeedPerLvl;
            _chosenTurretAttacker.UpdateLvl();
            UpdateStatsText();
        }

        public void UpdateHP()
        {
            _chosenTurretAttacker.UpdateHp();
            _chosenTurretAttacker.UpdateLvl();
            UpdateStatsText();
        }

        private void OpenTurretType()
        {
            // Открывать плашку с выбором башни (список)
            // В зависимости от выбранного значения в списке, показывать информационные статы
            // При нажатии на плюс вызывать метод из башни на изменение статов башни
            
            SetUpgradeButtonActivityAttacker(false);
            _turretStatsPanel.SetActive(false);
            _turretTypePanel.SetActive(true);

        }

        public void SetTurretType(int value)
        {
            _chosenTurretAttacker.SetTurretType((AttackerType)value);
            ClosePanel();
        }

        public void MakeHealer()
        {
            _chosenTurretFarmer.MakeFarmerHealer();
            _chosenTurretFarmer.UpdateLvl(); // TO DO Вынести в метод
            UpdateStatsText();
        }

        public void ChangeMoneyPerTick()
        {
            _chosenTurretFarmer.ChangeMoneyPerTick();
            _chosenTurretFarmer.UpdateLvl();
            UpdateStatsText();
        }

        public void ChangeMoneyPerZombie()
        {
            _chosenTurretFarmer.ChangeMoneyPerZombie();
            _chosenTurretFarmer.UpdateLvl();
            UpdateStatsText();
        }

    }
}
    

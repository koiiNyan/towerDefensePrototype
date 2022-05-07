using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieDefense
{
    public class TurretBuyMenu : MonoBehaviour
    {
        private Cell _chosenCell;
        public Cell ChosenCell {get => _chosenCell; set => _chosenCell = value; }

        [SerializeField]
        private Button _attackerTurretBtn;

        private LevelManager _levelManager;

        private void Awake()
        {
            _levelManager = GameObject.Find("Level").GetComponent<LevelManager>();
        }

        public void BuildAttackerTurret()
        {
             _levelManager.CreateAttackerTurret(_chosenCell, _chosenCell.transform.position);
            gameObject.SetActive(false);
        }

        public void BuildFarmerTurret()
        {
            _levelManager.CreateFarmerTurret(_chosenCell, _chosenCell.transform.position);
            gameObject.SetActive(false);
        }


        public void SetButtonActivity()
        {
            _attackerTurretBtn.interactable = CanBuildAttacker();
        }

        private bool CanBuildAttacker()
          {
            var pairedCell = _chosenCell.ParallelCell;
            if (pairedCell.CellTypo != CellType.Attacker) return true;

            return false;
        }


    }
}

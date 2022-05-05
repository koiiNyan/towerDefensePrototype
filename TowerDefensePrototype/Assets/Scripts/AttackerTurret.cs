using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieDefense
{
    public class AttackerTurret : MonoBehaviour
    {
        #region Fields

        [SerializeField, Range(0, 1)]
        private float _hp;
        private const float c_min_hp = 0f;
        private const float c_max_hp = 1f;

        public float Health
        {
            get => _hp;
            set
            {
                if (value < c_min_hp || value > c_max_hp)
                {

                    Debug.LogError("Attacker turrent cant have less than 0 or more than 1 hp!");
                }
                else
                {
                    _hp = value;
                }
            }
        }

        [SerializeField, Range(1, 5)]
        private int _level;
        private const int c_min_lvl = 1;
        private const int c_max_lvl = 5;

        public int CurrentLevel
        {
            get => _level;
            set
            {
                if (value < c_min_lvl || value > c_max_lvl)
                {

                    Debug.LogError("Attacker turret level cant be less than 1 or more than 5 lvl!");
                }
                else
                {
                    _level = value;
                }
            }
        }

        private const float c_turretRotationX = 90f;

        #endregion

        public float GetRotation(float x) => (x > 0) ? -c_turretRotationX : c_turretRotationX;

        [SerializeField]
        private Cell _turretCell;
        public Cell TurretCell { get => _turretCell; set => _turretCell = value; }

        [SerializeField]
        private EnemyCell _enemyCell;
        public EnemyCell EnemyCell { get => _enemyCell; set => _enemyCell = value; }

        private void Start()
        {
            Debug.Log("turretCell= " + _turretCell);
            Debug.Log("turretCellGetEnemyCell= " + _turretCell.GetEnemyCell);
            _enemyCell = _turretCell.GetEnemyCell;
            _enemyCell.ZombieEnterEvent += Attack;
        }

        private void Attack(List<Cell> component, List<Zombie> zombies)
        {
            Debug.Log("ATTACK");
        }
    }

}

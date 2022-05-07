using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


        public float GetRotation(float x) => (x > 0) ? -c_turretRotationX : c_turretRotationX;

        [SerializeField]
        private Cell _turretCell;
        public Cell TurretCell { get => _turretCell; set => _turretCell = value; }

        [SerializeField]
        private EnemyCell _enemyCell;
        public EnemyCell EnemyCell { get => _enemyCell; set => _enemyCell = value; }

        [SerializeField]
        private int _attackDamage = 5;
        [SerializeField]
        private float _attackSpeed = 1f;
        [SerializeField]
        private Slider _hpBar;
        [SerializeField]
        private int _cost = 20;

        public int Cost { get => _cost; private set => _cost = value; }
   

        #endregion

        private void Start()
        {
            _enemyCell = _turretCell.GetEnemyCell;
            _enemyCell.ZombieEnterEvent += Attack;
        }


        private void Attack(List<Zombie> zombies)
        {
            StartCoroutine(AttackCor(zombies));
        }

        private IEnumerator AttackCor(List<Zombie> zombies)
        {

            while (zombies.Count != 0)
            {
                //Debug.Log(zombies[0].Health);
                if (zombies[0].Health > 0) zombies[0].Health -= _attackDamage;
                else
                {
                    zombies[0].Die();
                    zombies.Remove(zombies[0]);
                   // Debug.Log(zombies.Count);
                }
                
                yield return new WaitForSeconds(_attackSpeed);

            }
           // Debug.Log("no zombies");

            yield return null;

        }
       
        public void DealDamage(float damage)
        {
            _hp -= damage;
            _hpBar.value -= damage;
        }

        public void Die()
        {
            _turretCell.CellTypo = CellType.Empty;
            _turretCell.AttackerTurret = null;
           Destroy(gameObject);
        }
       
    }

}

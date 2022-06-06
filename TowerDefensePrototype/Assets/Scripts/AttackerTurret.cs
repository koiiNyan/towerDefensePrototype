using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ZombieDefense
{
    public class AttackerTurret : MonoBehaviour
    {
        #region Fields

        [SerializeField, Range(0, 1)]
        private float _hp;
        private float _basicHp;
        public float BasicHp { get => _basicHp; }
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
        public int TurretMaxLevel => c_max_lvl;

        public int CurrentLevel
        {
            get => _level;
            set
            {
                if (value < c_min_lvl || value >= c_max_lvl)
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


        public float GetRotation(float x) 
        {
            if (x == 1.5f || x == 4.5f || x == 7.5f) return -c_turretRotationX;
            else return c_turretRotationX;
        }


        [SerializeField]
        private Cell _turretCell;
        public Cell TurretCell { get => _turretCell; set => _turretCell = value; }

        [SerializeField]
        private EnemyCell _enemyCell;
        public EnemyCell EnemyCell { get => _enemyCell; set => _enemyCell = value; }

        [SerializeField]
        private int _attackDamage = 5;
        public int AttackDamage
        {
            get => _attackDamage;
            set
            {
                if (value < 0)
                {

                    Debug.LogError("Attacker turrent cant have less than 0 AD!");
                }
                else
                {
                    _attackDamage = value;
                }
            }
        }
        [SerializeField]
        private float _attackSpeed = 1f;
        public float AttackSpeed { get => _attackSpeed;
            set
            {
                if (value < 0)
                {

                    Debug.LogError("Attacker turrent cant have less than 0 AS!");
                }
                else
                {
                    _attackSpeed = value;
                }
            }
        }
        [SerializeField]
        private Slider _hpBar;
        [SerializeField]
        private int _cost = 20;

        public int Cost { get => _cost; private set => _cost = value; }

        [SerializeField, Range(1, 20), Tooltip("Процент увеличения цены при повышении уровня")]
        private int _costPercent = 10;

        private AttackerType _turretType = AttackerType.None;
        public AttackerType TurretType => _turretType;

        [SerializeField]
        private GameObject _turretTypeMark;

        #endregion

        private void Start()
        {
            _enemyCell = _turretCell.GetEnemyCell;
            _enemyCell.ZombieEnterEvent += Attack;
            _basicHp = _hp;
        }


        private void Attack(List<Zombie> zombies)
        {
            StartCoroutine(AttackCor(zombies));
        }

        private IEnumerator AttackCor(List<Zombie> zombies)
        {

            while (zombies.Count != 0)
            {
                if (_turretType == AttackerType.None || _turretType == AttackerType.Ice)
                {
                    Debug.Log("Usual Attack");
                    if (zombies[0].Health > 0)
                    {
                        zombies[0].Health -= _attackDamage;
                        if (_turretType == AttackerType.Ice) zombies[0].SlowAS();
                    }
                    else
                    {
                        zombies[0].Die();
                        zombies.Remove(zombies[0]);
                        // Debug.Log(zombies.Count);
                    }
                }

                if (_turretType == AttackerType.Fire)
                {
                    Debug.Log("Fire Attack Turret");
                    if (zombies[0].Health > 0) zombies[0].SetOnFire();
                    else
                    {
                        zombies[0].Die();
                        zombies.Remove(zombies[0]);
                        // Debug.Log(zombies.Count);
                    }
                }

                if (_turretType == AttackerType.Electricity)
                {
                    foreach (Zombie zombie in zombies)
                    {
                        Debug.Log("Electricity Attack");
                        if (zombie.Health > 0)
                        {
                            zombie.Health -= _attackDamage;
                        }
                        else
                        {
                            zombie.Die();
                            zombies.Remove(zombies[0]);
                            // Debug.Log(zombies.Count);
                        }
                    }
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

        public void UpdateLvl()
        {
            _level++;
            _cost += _cost / _costPercent;
        }
        public void UpdateHp()
        {
            _hp = _basicHp + 0.5f;
            _basicHp = _hp;
            _hpBar.maxValue = _hp;
            _hpBar.value = _hp;
        }

        public void HealTurret()
        {
            _hp = _basicHp;
            _hpBar.value = _hp;
        }

        public void SetTurretType(AttackerType turretType)
        {
            _turretType = turretType;
            _turretTypeMark.SetActive(true);
            var meshRenderer = _turretTypeMark.GetComponent<MeshRenderer>();

            var tempMaterial = new Material(meshRenderer.sharedMaterial);
            tempMaterial.color = GetColorByType(turretType);
            meshRenderer.sharedMaterial = tempMaterial;
        }

        private Color GetColorByType(AttackerType turretType)
        {
            Dictionary<AttackerType, Color> colorDictionary = new Dictionary<AttackerType, Color>()
        {
            { AttackerType.Fire , Color.red },
            { AttackerType.Ice, Color.blue },
            { AttackerType.Electricity, Color.magenta },
        };

            return colorDictionary[turretType];
        }

    }


}

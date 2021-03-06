using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieDefense
{
    public class Zombie : MonoBehaviour
    {
        #region Fields

        [SerializeField, Range(0, 100)]
        private int _hp = 100;
        [SerializeField]  //TODO убрать серилизацию
        private int _currentHp;
        private const int c_min_hp = 0;
        private const int c_max_hp = 100;

        public int Health
        {
            get => _currentHp;
            set
            {
                if (value < c_min_hp || value > c_max_hp)
                {

                    Debug.LogError("Zombie cant have less than 0 or more than 100 hp!");
                }
                else
                {
                    _currentHp = value;
                }
            }
        }

        [SerializeField]
        private float _moveSpeed;
        private Animator _zombieAnimator;

        public Vector3 InitialPosition { get; } = new Vector3(0.5f, -0f, -16.5f); //TODO
        public Vector3 InitialPositionNormal { get; } = new Vector3(3.5f, -0f, -16.5f); //TODO
        public Vector3 InitialPositionHard { get; } = new Vector3(6.5f, -0f, -16.5f); //TODO

        private bool _walking = true;
        private bool _dead = false;
        private bool _isOnFire = false;

        [SerializeField]
        private int _money = 5;
        public int Money { get => _money; set
            {
                if (value <= 0)
                {

                    Debug.LogError("Money per zombie can't be less than 0");
                }
                else
                {
                    _currentHp = value;
                }
            }
        }

        private Player _player;

        [SerializeField, Range (0.01f, 0.5f)]
        private float _attackDamage = 0.05f;

        [SerializeField, Range (0.5f, 5f), Tooltip("Чем значение ниже, тем выше скорость атаки")]
        private float _attackSpeed = 2f;
        private float _originalAS = 2f;


        [SerializeField]
        private EnemyType _enemyType = EnemyType.Normal;
        public EnemyType ZombieType { get => _enemyType; }

        #endregion

        private void Awake()
        {
            _zombieAnimator = GetComponent<Animator>();
            _currentHp = _hp;
            _player = GameObject.Find("Player").GetComponent<Player>();


        }


        private void Update()  //TODO
        {
            if (_walking && _player.GameActive) Move();
            if (_dead) Die();


        }

        private void OnTriggerEnter(Collider other)
        {
            ReturnToPool();
            _player.SetZombieMissed();
        }

        public void SetMoveSpeedAnimator(bool moving)
        {
            var speed = moving ? _moveSpeed : 0;
            _zombieAnimator.SetFloat("MoveSpeed", speed);
        }

        public void Die()
        {
            _dead = false;
            _zombieAnimator.SetTrigger("Dead");
            Debug.Log("Die");
            _player.AddMoney(_money);
            StartCoroutine(Death());

        }

        private IEnumerator Death()
        {
            yield return new WaitForSeconds(3f);

            if (gameObject.activeInHierarchy) ReturnToPool();

            yield return null;
        }

        private void ReturnToPool()
        {
            gameObject.SetActive(false);
            _currentHp = _hp;
            _walking = true;

        }

        private void Move()
        {
            var targetPosition = GetTargetPosition();
            if (!InRadius(targetPosition)) 
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime); //TODO
            if (InRadius(targetPosition)) StartCoroutine(Attack()); //walking = false;
        }

        private bool InRadius(Vector3 targetPosition)
        {
            var caseOne = transform.position == targetPosition;
            var caseTwo = Mathf.Abs(targetPosition.z - transform.position.z) <= 0.5;
            if (caseOne || caseTwo) return true;
            return false;
        }

        private Vector3 GetTargetPosition()//TODO
        {
            var allTurrets = FindAllAttackerTurrets();

            var distance = _player.transform.position;

            distance.x = transform.position.x;

            if (allTurrets.Length > 0)
            {
    
                distance = new Vector3(0f, 0f, -20f);
                foreach (AttackerTurret turret in allTurrets)
                {
                    if (Mathf.Abs(transform.position.x - turret.transform.position.x) != 1) continue;

                    var deltaDistance = turret.transform.position - transform.position;
                    if (deltaDistance.z > distance.z)
                    {
                        distance = turret.transform.position;

                        if (turret.EnemyCell.Zombies.Count >= 3) distance.z = transform.position.z;
                    }

                    /* Если дельта самая большая, проверяем, что ботов на клетке !=3
                     * Если ботов 3 или более, идем дальше по списку башен
                     * Если башен со свободными слотами нет, targetPosition = клетка перед последней активной башней
                     * Проверяем колво зомби на клетке, если больше 3 - targetPost пред. клетка и т.д
                     */
                }

                var normalDistance = new Vector3(transform.position.x, 0f, distance.z);

                if (transform.position.z > normalDistance.z) normalDistance = _player.transform.position;

                normalDistance.x = transform.position.x;
                return normalDistance;
            }


            return distance; 
        }

        private IEnumerator Attack() //TODO
        {
            _walking = false;
            SetMoveSpeedAnimator(false);
            _zombieAnimator.SetTrigger("Attack");



            var allTurrets = FindAllAttackerTurrets();
            AttackerTurret currentTurret = null;
            foreach (AttackerTurret turret in allTurrets)
            {
                if (Mathf.Abs(turret.transform.position.z - transform.position.z) <= 0.5) currentTurret = turret;
            }
            Debug.Log(currentTurret);

            while (currentTurret != null && !_dead)
            {
                if (currentTurret.Health > 0) currentTurret.DealDamage(_attackDamage);

                yield return new WaitForSeconds(_attackSpeed);

                if (currentTurret.Health <= 0)
                {
                    currentTurret.Die();
                    currentTurret = null;
                }
            }


            _zombieAnimator.ResetTrigger("Attack");
            _walking = true;
            SetMoveSpeedAnimator(true);

            yield return null;


        }

        private AttackerTurret[] FindAllAttackerTurrets() => FindObjectsOfType<AttackerTurret>();

        public void SetOnFire()
        {
            if (!_isOnFire)
            {
                _isOnFire = true;
                StartCoroutine(DealFireDamage());

            }
            return;
        }

        public void SlowAS()
        {
            StartCoroutine(SlowAttackSpeed());
        }

        private IEnumerator DealFireDamage()
        {
            for (int i=0; i < 5; i++)
            {
                Debug.Log("Fire Damage");
                _hp -= 3;
                yield return new WaitForSeconds(1);
            }
            _isOnFire = false;

            yield return null;
        }

        private IEnumerator SlowAttackSpeed()
        {
            Debug.Log($"AS BEFORE = {_attackSpeed}");
            _attackSpeed++;
            
            yield return new WaitForSeconds(3);
            _attackSpeed = _originalAS;
            Debug.Log($"AS AFTER = {_attackSpeed}");

        }



    }
}
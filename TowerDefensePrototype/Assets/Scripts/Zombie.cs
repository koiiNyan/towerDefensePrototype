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

        private bool walking = true;

        #endregion

        private void Awake()
        {
            _zombieAnimator = GetComponent<Animator>();
            _currentHp = _hp;
  
        }


        private void Update()  //TODO
        {
            if (walking) Move();


        }

        private void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
            _currentHp = _hp;
        }

        public void SetMoveSpeedAnimator(bool moving)
        {
            var speed = moving ? _moveSpeed : 0;
            _zombieAnimator.SetFloat("MoveSpeed", speed);
        }

        public void Die()
        {
            _zombieAnimator.SetTrigger("Dead");
        }

        private void Move()
        {
            var targetPosition = GetTargetPosition();
            if (!InRadius(targetPosition)) 
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime); //TODO
            if (InRadius(targetPosition)) Attack(); //walking = false;
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
            var _allTurrets = FindObjectsOfType<AttackerTurret>();

            if (_allTurrets.Length > 0)
            {
                Vector3 distance = new Vector3(0f, 0f, -20f);
                foreach (AttackerTurret turret in _allTurrets)
                {
                    var deltaDistance = turret.transform.position - transform.position;
                    if (deltaDistance.z > distance.z) distance = turret.transform.position;//deltaDistance;
                }

                var normalDistance = new Vector3(0.5f, 0f, distance.z);

                
                return normalDistance;
            }


            return new Vector3(0.5f, 0f, 12.5f); //TODO
        }

        private void Attack() //TODO
        {
            Debug.Log("Attack!");
            walking = false;
            SetMoveSpeedAnimator(false);
        }
      
    }
}
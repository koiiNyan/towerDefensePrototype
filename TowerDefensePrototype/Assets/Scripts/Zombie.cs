using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieDefense
{
    public class Zombie : MonoBehaviour
    {
        #region Fields

        [SerializeField, Range(0, 100)]
        private float _hp;
        private const int c_min_hp = 0;
        private const int c_max_hp = 100;

        public float Health
        {
            get => _hp;
            set
            {
                if (value < c_min_hp || value > c_max_hp)
                {

                    Debug.LogError("Attacker turrent cant have less than 0 or more than 100 hp!");
                }
                else
                {
                    _hp = value;
                }
            }
        }

        [SerializeField]
        private float _moveSpeed;
        private Animator _zombieAnimator;

        public Vector3 InitialPosition { get; } = new Vector3(0.5f, -0f, -16.5f); //TODO

        #endregion

        private void Awake()
        {
            _zombieAnimator = GetComponent<Animator>();           
        }

        private void Update()  //TODO
        {
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

           
        }

        private void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
        }

        public void SetMoveSpeedAnimator()
        {
            _zombieAnimator.SetFloat("MoveSpeed", _moveSpeed);
        }
    }
}
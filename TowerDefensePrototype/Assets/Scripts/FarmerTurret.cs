using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieDefense
{
    public class FarmerTurret : MonoBehaviour
    {
        #region Fields

 
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
        private int _cost = 10;

        public int Cost { get => _cost; private set => _cost = value; }

        [SerializeField]
        private int _moneyPerTick = 5;
   

        #endregion

        private void Awake()
        {
            StartCoroutine(AddMoneyToPlayer());
        }


        private IEnumerator AddMoneyToPlayer()
        {
            var playerComponent = GameObject.Find("Player").GetComponent<Player>();
           while (true)
            {
                Debug.Log("Adding Money!!!!");
                playerComponent.AddMoney(_moneyPerTick);

                yield return new WaitForSeconds(5f);
            }
            

        }
       
       
    }

}

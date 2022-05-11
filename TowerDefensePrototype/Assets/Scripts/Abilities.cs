using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieDefense
{
    public class Abilities : MonoBehaviour
    {
        [SerializeField]
        private Button _healTurretsBtn;
        [SerializeField]
        private Button _invulnerabilityBtn;

        [SerializeField]
        private float _healTurretsBtnCD = 1f;
        [SerializeField]
        private float _invulnerabilityBtnCD = 1f;

        public void HealTurretButton()
        {
            Debug.Log("HealTurretButton()");
        }

        public void InvulnerabilityButton()
        {
            Debug.Log("InvulnerabilityButton()");
        }


        //Если нет денег или кд - кнопки неактивны. Если игра закончена - кнопки неактивны

        public void SetButtonActivity()
        {

        }
    }
}

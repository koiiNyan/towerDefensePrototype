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
        private float _healTurretsBtnCD = 5f;
        [SerializeField]
        private float _invulnerabilityBtnCD = 5f;
        [SerializeField]
        private float _invulnerabilityDuration = 2f;

        public void HealTurretButton()
        {
            Debug.Log("HealTurretButton()");
            var allTurrets = FindObjectsOfType<AttackerTurret>();
            foreach (AttackerTurret turret in allTurrets)
            {
                turret.HealTurret();
            }
            StartCoroutine(AbilityCD(_healTurretsBtn, _healTurretsBtnCD));
        }



        public void InvulnerabilityButton()
        {
            Debug.Log("InvulnerabilityButton()");
        }


        //Если нет денег или кд - кнопки неактивны. Если игра закончена - кнопки неактивны

        private IEnumerator AbilityCD(Button btn, float cd)
        {
            btn.interactable = false;
            yield return new WaitForSeconds(cd);
            btn.interactable = true;
        }

        private IEnumerator MakePlayerInvinsible()
        {
            /// +++ inv
            yield return new WaitForSeconds(1f);
            /// ---- inv
        }

    }
}

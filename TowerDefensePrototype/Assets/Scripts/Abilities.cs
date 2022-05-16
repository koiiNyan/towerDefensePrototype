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
        private Button _damageBtn;

        [SerializeField]
        private float _healTurretsBtnCD = 5f;
        [SerializeField]
        private float _invulnerabilityBtnCD = 5f;
        [SerializeField]
        private float _invulnerabilityDuration = 5f;
        [SerializeField]
        private float _damageBtnCD = 5f;

        [SerializeField]
        private int _invulnerabilityCost = 100;
        [SerializeField]
        private int _damageCost = 100;
        [SerializeField]
        private int _damage = 10;

        [SerializeField]
        private GameObject _player;

        [SerializeField]
        private Text _invulnerabilityText;

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
            if (_player.GetComponent<Player>().Money >= _invulnerabilityCost)
            {
                StartCoroutine(AbilityCD(_invulnerabilityBtn, _invulnerabilityBtnCD));
                StartCoroutine(MakePlayerInvulnerable());
            }
        }

        public void DealDamageButton()
        {
            if (_player.GetComponent<Player>().Money >= _damageCost)
            {
                Debug.Log("DealDamageButton()");
                var allEnemies = FindObjectsOfType<Zombie>();
                foreach (Zombie zombie in allEnemies)
                {
                    zombie.Health -= _damage;
                }
                StartCoroutine(AbilityCD(_damageBtn, _damageBtnCD));
            }
        }



        //Если нет денег или кд - кнопки неактивны. Если игра закончена - кнопки неактивны

        private IEnumerator AbilityCD(Button btn, float cd)
        {
            btn.interactable = false;
            yield return new WaitForSeconds(cd);
            btn.interactable = true;
        }

        private IEnumerator MakePlayerInvulnerable()
        {
            _player.GetComponent<Player>().NotInvulnerable = false;
            _invulnerabilityText.gameObject.SetActive(true);
            yield return new WaitForSeconds(_invulnerabilityDuration);
            _player.GetComponent<Player>().NotInvulnerable = true;
            _invulnerabilityText.gameObject.SetActive(false);
        }



    }
}

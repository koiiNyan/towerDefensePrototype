using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieDefense
{
    public class EnemyCell : MonoBehaviour
    {
        private List<Zombie> _zombies = new List<Zombie>();

        [SerializeField]
        private List<Cell> _pairTurretCell;

        public event ZombieEnterEventHandler ZombieEnterEvent;


        private void OnCollisionEnter(Collision collision)
        {
            //Вызов ивента на стрельбу в AttackerTurret у парной клетки
            //Добавить зомби в пул

            _zombies.Add(collision.gameObject.GetComponent<Zombie>());
            ZombieEnterEvent?.Invoke(_zombies);

        }

        private void OnCollisionExit(Collision collision)
        {
            //Если врагов на клетке нет, башня перестает стрелять

            _zombies.Remove(collision.gameObject.GetComponent<Zombie>());
            ZombieEnterEvent?.Invoke(_zombies);

        }

        public void SetTurretCell(Cell pairCell)
        {
            _pairTurretCell.Add(pairCell);
        }


        public delegate void ZombieEnterEventHandler(List<Zombie> zombies);
    }
}

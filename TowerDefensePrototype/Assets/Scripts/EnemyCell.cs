using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieDefense
{
    public class EnemyCell : MonoBehaviour
    {
        private List<Zombie> _zombies;

        [SerializeField]
        private List<Cell> _pairTurretCell;

        public event ZombieEnterEventHandler ZombieEnterEvent;


        private void OnCollisionEnter(Collision collision)
        {
            //Вызов ивента на стрельбу в AttackerTurret у парной клетки
            //Добавить зомби в пул

            ZombieEnterEvent?.Invoke(_pairTurretCell, _zombies);


        }

        private void OnCollisionExit(Collision collision)
        {
            //Если врагов на клетке нет, башня перестает стрелять
        }

        public void SetTurretCell(Cell pairCell)
        {
            _pairTurretCell.Add(pairCell);
        }


        public delegate void ZombieEnterEventHandler(List<Cell> component, List<Zombie> zombies);
    }
}

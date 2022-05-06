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



        public bool AttackTurretAlive()
        {
            int cells = _pairTurretCell.Count;
            int cellsWoTurret = 0;
            foreach (Cell cell in _pairTurretCell)
            {
                if (cell.AttackerTurret == null)
                {

                    cellsWoTurret++;
                }
            }

            if (cellsWoTurret == cells) return false;
            return true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Вызов ивента на стрельбу в AttackerTurret у парной клетки
            //Добавить зомби в пул

            _zombies.Add(collision.gameObject.GetComponent<Zombie>());
            
            if (AttackTurretAlive()) ZombieEnterEvent?.Invoke(_zombies);

        }

        private void OnCollisionExit(Collision collision)
        {
            //Если врагов на клетке нет, башня перестает стрелять

            _zombies.Remove(collision.gameObject.GetComponent<Zombie>());
            if (AttackTurretAlive())  ZombieEnterEvent?.Invoke(_zombies);

        }

        public void SetTurretCell(Cell pairCell)
        {
            _pairTurretCell.Add(pairCell);
        }


        public delegate void ZombieEnterEventHandler(List<Zombie> zombies);

       
    }
}

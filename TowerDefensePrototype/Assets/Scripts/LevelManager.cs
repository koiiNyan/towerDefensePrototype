using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


namespace ZombieDefense
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private Cell[] _allCells;

        [SerializeField]
        private GameObject _attackerTurretPrefab;


        private void Awake()
        {
            _allCells = GetComponentsInChildren<Cell>();

            foreach (Cell cell in _allCells)
            {
                cell.OnClickEventHandler += SelectActionCellComponent;
                cell.OnFocusEventHandler += HoverAction;
                //  Debug.Log(cell.transform.position);
            }
        }

        public void SelectActionCellComponent(Cell component)
        {

            CreateAttackerTurret(component.transform.position);
        }

        public void HoverAction(Cell component, bool isSelect)
        {

        }



        private void CreateAttackerTurret(Vector3 cellPosition)
        {
            var rotation = Quaternion.Euler(
                new Vector3(0f, _attackerTurretPrefab.GetComponent<AttackerTurret>().GetRotation(cellPosition.x), 0f));     //TODO
            var turretPosition = cellPosition;
            turretPosition.y = 0;
            Instantiate(_attackerTurretPrefab, turretPosition, rotation);
        }
    }

 
}

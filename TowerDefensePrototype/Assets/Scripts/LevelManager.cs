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

        private List<GameObject> _pooledObjects;
        [SerializeField]
        private GameObject _objectToPool;
        [SerializeField]
        private int _amountToPool;
        [SerializeField]
        private int _spawnSeconds;


        private void Awake()
        {
            FollowEvents();
            CreateEnemiesPool();
            StartCoroutine(SpawnEnemies());
        }

        private void FollowEvents()
        {
            _allCells = GetComponentsInChildren<Cell>();

            foreach (Cell cell in _allCells)
            {
                cell.OnClickEventHandler += SelectActionCellComponent;
                cell.OnFocusEventHandler += HoverAction;
            }
        }

        public void SelectActionCellComponent(Cell component)
        {

            CreateAttackerTurret(component, component.transform.position);
        }

        public void HoverAction(Cell component, bool isSelect)
        {

        }



        private void CreateAttackerTurret(Cell component, Vector3 cellPosition)
        {
            var rotation = Quaternion.Euler(
                new Vector3(0f, _attackerTurretPrefab.GetComponent<AttackerTurret>().GetRotation(cellPosition.x), 0f));     //TODO
            var turretPosition = cellPosition;
            turretPosition.y = 0;
            Instantiate(_attackerTurretPrefab, turretPosition, rotation);
            component.CellTypo = CellType.Attacker;
        }

        private void CreateEnemiesPool()
        {
            _pooledObjects = new List<GameObject>();
            for (int i = 0; i < _amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(_objectToPool);
                obj.SetActive(false);
                _pooledObjects.Add(obj);
                obj.transform.SetParent(GameObject.Find("Enemies").transform); //TODO
            }
        }

        private GameObject GetPooledObject()
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            } 
            return null;
        }

        private IEnumerator SpawnEnemies()
        {
            while (true) //TODO
            {
                GameObject pooledZombie = GetPooledObject();
                if (pooledZombie != null)
                {
                    pooledZombie.SetActive(true);
                    pooledZombie.transform.position = pooledZombie.GetComponent<Zombie>().InitialPosition; //TODO
                    pooledZombie.GetComponent<Zombie>().SetMoveSpeedAnimator();//TODO
                }

                yield return new WaitForSeconds(_spawnSeconds);
            }
        }

    }

 
}

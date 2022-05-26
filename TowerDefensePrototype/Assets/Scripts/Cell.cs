using UnityEngine;
using UnityEngine.EventSystems;

namespace ZombieDefense
{
    public class Cell : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private CellType _cellType;

        public CellType CellTypo { get => _cellType; set => _cellType = value; }


        [SerializeField]
        private EnemyCell _pairEnemyCell;
        public EnemyCell GetEnemyCell => _pairEnemyCell;

        [SerializeField]
        private AttackerTurret _attackerTurret;
        public AttackerTurret AttackerTurret { get => _attackerTurret; set => _attackerTurret = value; }

        [SerializeField]
        private FarmerTurret _farmerTurret;
        public FarmerTurret FarmerTurret { get => _farmerTurret; set => _farmerTurret = value; }

        public void SetEnemyCell(EnemyCell enemyCell)
        {
            _pairEnemyCell = enemyCell;
        }
        [SerializeField]
        private Cell _parallelCell;
        public Cell ParallelCell { get => _parallelCell; set => _parallelCell = value; }

        public event ClickEventHandler OnClickEventHandler;


        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickEventHandler?.Invoke(this);
        }

        public delegate void ClickEventHandler(Cell component);

    }
}


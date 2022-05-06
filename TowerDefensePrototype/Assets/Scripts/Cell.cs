using UnityEngine;
using UnityEngine.EventSystems;

namespace ZombieDefense
{
    public class Cell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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

        public void SetEnemyCell(EnemyCell enemyCell)
        {
            _pairEnemyCell = enemyCell;
        }

        public event ClickEventHandler OnClickEventHandler;

        public event FocusEventHandler OnFocusEventHandler;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickEventHandler?.Invoke(this);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            CallBackEvent(this, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CallBackEvent(this, false);
        }

        protected void CallBackEvent(Cell target, bool isSelect)
        {
            OnFocusEventHandler?.Invoke(target, isSelect);
        }

        public delegate void ClickEventHandler(Cell component);
        public delegate void FocusEventHandler(Cell component, bool isSelect);
    }
}


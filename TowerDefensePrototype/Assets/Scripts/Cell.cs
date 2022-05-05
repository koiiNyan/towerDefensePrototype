using UnityEngine;

namespace ZombieDefense
{
    public class Cell : MonoBehaviour
    {
        [SerializeField]
        private CellType _cellType;

        public CellType CellTypo { get; set; }
    }
}


using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField]
        private GameObject _farmerTurretPrefab;

        private List<GameObject> _pooledObjects;
        [SerializeField]
        private GameObject[] _objectsToPool;
        [SerializeField, Tooltip("Сколько объектов отправлять в пул, от 20")]
        private int _amountToPool;
        [SerializeField]
        private int _zombiesPerWave = 10;


        private Player _player;

        [SerializeField]
        private int _waveToWin = 21;
        
        [SerializeField]
        private int _currentWawe = 0;
        [SerializeField]
        private TextMeshProUGUI _waveText;
        [SerializeField]
        private int _spawnSeconds = 1;
        [SerializeField]
        private float _pauseBetweenWaves = 3;
        private float _timer;

        private bool _canSpawn = true;

        [SerializeField, Tooltip ("Сильный зомби спавнится каждую N волну")]
        private int _strongZombieSpawnWave = 3;
        [SerializeField, Tooltip("Босс зомби спавнится каждую N волну. Число должно быть кратно 10")]
        private int _bossZombieSpawnWave = 10;
        [SerializeField, Tooltip("С какой волны начинается рандом во врагах")]
        private int _randomWave = 11;

        [SerializeField]
        private GameObject _turretBuyUI;
        [SerializeField]
        private GameObject _turretUpgradeUI;
        [SerializeField]
        private GameObject _pauseUI;
        [SerializeField]
        private GameObject _gameOverUI;

        private bool _gameActive = true;
        public bool GameActive { get => _gameActive; private set => _gameActive = value; }

        private void Awake()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            _timer = _pauseBetweenWaves;

            FollowEvents();
            CreateEnemiesPool();
            StartCoroutine(SpawnEnemies());
            SetCells();
            UpdateSpawnText();

            var audio = GetComponent<AudioSource>();
            audio.volume = Settings.Instance.VolumeValue > 0 ? Settings.Instance.VolumeValue : 1;
            Debug.Log($"Volume = {Settings.Instance.VolumeValue}");

            Debug.Log($"Difficulty = {Settings.Instance.DifficultyLevel}");
        }

        private void Update()
        {
            var zombiesCount = FindObjectsOfType<Zombie>().Length;
            if (zombiesCount == 0 && _gameActive)
            {
                Timer();
            }
        }

        private void Timer()
        {
            if (_timer >= 0)
            {
                _timer -= Time.deltaTime;
                Debug.Log("_timer");

            }
            else
            {
                _timer = _pauseBetweenWaves;
                Debug.Log("timer update");
                StartCoroutine(SpawnEnemies());
                _canSpawn = true;
            }

        }
        private void FollowEvents()
        {
            _allCells = GetComponentsInChildren<Cell>();

            foreach (Cell cell in _allCells)
            {
                cell.OnClickEventHandler += SelectActionCellComponent;
            }
            _player.GameOverEventHandler += GameOver;
            _player.PauseEventHandler += Pause;
        }

        public void SelectActionCellComponent(Cell component)
        {
            if(component.CellTypo == CellType.Empty)
            //CreateAttackerTurret(component, component.transform.position);
            {
                TurretBuildMenu(component);
            }
            //Здесь вызывать не создание башни, а функцию показа юи строительства (если клетка пустая, через celltype). Передавать в параметры положение для юи
            //В Юи - выбор башни
            //Проверять, что на клетка - 1) уже не занята башней
            // Для атакующей башни - проверять, что противоположная клетка не занята атакующей башней (через cell type)
           else if(component.CellTypo == CellType.Attacker)
            {
                TurretUpgradeMenu(component.AttackerTurret);
            }

            else if (component.CellTypo == CellType.Farmer)
            {
                TurretUpgradeMenu(component.FarmerTurret);
            }
            
            else Debug.Log("Cant'build");
        }

        private void TurretBuildMenu(Cell component)
        {
            if (TurretUIActive())
            {
                Debug.Log("active");
                return;
            }
            else
            {
                _turretBuyUI.SetActive(true);
                _turretBuyUI.GetComponent<TurretBuyMenu>().ChosenCell = component;
                _turretBuyUI.GetComponent<TurretBuyMenu>().SetButtonActivity();
            }
        }

        private void TurretUpgradeMenu(AttackerTurret turret)
        {
            if (TurretUIActive())
            {
                Debug.Log("active");
                return;
            }
            else
            {
                _turretUpgradeUI.SetActive(true);
                _turretUpgradeUI.GetComponent<TurretUpgradeMenu>().OpenPanels(turret);
                
                //_turretUpgradeUI.GetComponent<TurretUpgradeMenu>().SetUpgradeButtonActivity(true);
                //_turretUpgradeUI.GetComponent<TurretUpgradeMenu>().UpdateStatsText();
            }
        }

        private void TurretUpgradeMenu(FarmerTurret turret)
        {
            if (TurretUIActive())
            {
                Debug.Log("active");
                return;
            }
            else
            {
                _turretUpgradeUI.SetActive(true);
                _turretUpgradeUI.GetComponent<TurretUpgradeMenu>().OpenPanels(turret);


            }
        }

        private bool TurretUIActive() => (_turretUpgradeUI.activeInHierarchy || _turretBuyUI.activeInHierarchy);


        public void CreateAttackerTurret(Cell component, Vector3 cellPosition) //TODO
        {
            var turretCost = _attackerTurretPrefab.GetComponent<AttackerTurret>().Cost;

            if (EnoughMoney(turretCost))
            {
                var rotation = Quaternion.Euler(
                    new Vector3(0f, _attackerTurretPrefab.GetComponent<AttackerTurret>().GetRotation(cellPosition.x), 0f));     //TODO
                var turretPosition = cellPosition;
                turretPosition.y = 0;
                var turret = Instantiate(_attackerTurretPrefab, turretPosition, rotation);
                component.CellTypo = CellType.Attacker;
                turret.GetComponent<AttackerTurret>().TurretCell = component;

                //Debug.Log(turret.GetComponent<AttackerTurret>());
                component.AttackerTurret = turret.GetComponent<AttackerTurret>();

                _player.AddMoney(-turretCost);
                //Debug.Log(turretCost);
            }
            else Debug.Log("Player doesn't have enough Money!");
        }




        //TODO MUST BE IN ONE METHOD

        public void CreateFarmerTurret(Cell component, Vector3 cellPosition) //TODO
        {
            Debug.Log(_farmerTurretPrefab);
            var turretCost = _farmerTurretPrefab.GetComponent<FarmerTurret>().Cost;

            if (EnoughMoney(turretCost))
            {
                var rotation = Quaternion.Euler(
                    new Vector3(0f, _farmerTurretPrefab.GetComponent<FarmerTurret>().GetRotation(cellPosition.x), 0f));     //TODO
                var turretPosition = cellPosition;
                turretPosition.y = 0;
                var turret = Instantiate(_farmerTurretPrefab, turretPosition, rotation);
                turret.GetComponent<FarmerTurret>().TurretCell = component;

                component.CellTypo = CellType.Farmer;

                component.FarmerTurret = turret.GetComponent<FarmerTurret>();

                _player.AddMoney(-turretCost);
                //Debug.Log(turretCost);
            }
            else Debug.Log("Player doesn't have enough Money!");
        }

        private bool EnoughMoney(int cost) => _player.Money >= cost;

        private void CreateEnemiesPool()
        {
            _amountToPool++;
            _pooledObjects = new List<GameObject>();
            for (int i = 0; i < _amountToPool; i++)
            {
                GameObject obj = null;
                if (i < _amountToPool / 2)
                {
                    obj = (GameObject)Instantiate(_objectsToPool[0]); // Pool normal Zombie
                }
                else if (i > _amountToPool /2 && i <= _amountToPool-1)
                {
                    obj = (GameObject)Instantiate(_objectsToPool[1]); // Pool strong Zombie
                }
                else
                {
                    obj = (GameObject)Instantiate(_objectsToPool[2]); // Pool boss Zombie
                }
                
                obj.SetActive(false);
                _pooledObjects.Add(obj);
                obj.transform.SetParent(GameObject.Find("Enemies").transform); //TODO
            }
        }

        private GameObject GetPooledObject(EnemyType zombieType)
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].activeInHierarchy && _pooledObjects[i].GetComponent<Zombie>().ZombieType == zombieType)
                {
                    return _pooledObjects[i];
                }
            } 
            return null;
        }

        private IEnumerator SpawnEnemies()
        {

            while (_canSpawn && _gameActive) //TODO
            {
                _currentWawe++;
                var allFarmers = GameObject.FindObjectsOfType<FarmerTurret>();
                foreach (FarmerTurret farmer in allFarmers)
                {
                    farmer.HealAttackerTurret();
                    
                }


                UpdateSpawnText();
                if (_currentWawe == _waveToWin)
                {
                    GameOver(true);
                    _player.GameActive = false;
                    yield return null;
                }
                else
                {
                    EnemyType zombieType;

                    if (_currentWawe % _bossZombieSpawnWave == 0) zombieType = EnemyType.Boss;
                    else if (_currentWawe % _strongZombieSpawnWave == 0) zombieType = EnemyType.Strong;
                    else zombieType = EnemyType.Normal;

                    for (int i = 0; i < _zombiesPerWave; i++)
                    {
                        if (zombieType == EnemyType.Boss && i > 0) zombieType = EnemyType.Normal;
                        
                        if ( _currentWawe >= _randomWave && (zombieType == EnemyType.Normal || zombieType == EnemyType.Strong))
                        {
                            int randomInt = Random.Range(0, 2);
                            zombieType = (EnemyType)randomInt;
                        }
                        GameObject pooledZombie = GetPooledObject(zombieType);
                        if (pooledZombie != null)
                        {
                            pooledZombie.SetActive(true);
                            pooledZombie.transform.position = pooledZombie.GetComponent<Zombie>().InitialPosition; //TODO
                            
                            pooledZombie.GetComponent<Zombie>().SetMoveSpeedAnimator(true);//TODO
                        }

                        yield return new WaitForSeconds(_spawnSeconds);
                    }
                }
                _canSpawn = false;

            }
        }


        private void SetCells()
        {
            var allEnemiesCells = GetComponentsInChildren<EnemyCell>();

            foreach (Cell cell in _allCells)
            {
                foreach(EnemyCell enemy in allEnemiesCells)
                {
                    if (cell.transform.position.z == enemy.transform.position.z)
                    {
                        cell.SetEnemyCell(enemy);
                        enemy.SetTurretCell(cell);

                        break;
                    }
                }

                foreach (Cell goodCell in _allCells)
                {
                   
                    if (cell.transform.position.z == goodCell.transform.position.z &&
                        Mathf.Abs(cell.transform.position.x - goodCell.transform.position.x) == 2)
                    {
                        cell.ParallelCell = goodCell;
                        break;
                    }
                }
            }
        }

        private void UpdateSpawnText()
        {
            _waveText.text = _currentWawe.ToString();
        }


        private void GameOver(bool IsWin)
        {
            _gameOverUI.GetComponent<GameOverPanel>().SetPlayerScore(_currentWawe);
            _gameOverUI.GetComponent<GameOverPanel>().UpdateScoreText();
            _gameOverUI.GetComponent<GameOverPanel>().UpdateGameOverText(IsWin);
            _gameOverUI.SetActive(true);
            _gameActive = false;
        }

        private void Pause()
        {
            _pauseUI.SetActive(!_pauseUI.activeInHierarchy);
            Time.timeScale = _pauseUI.activeInHierarchy ? 0 : 1;
        }

    }

 
}

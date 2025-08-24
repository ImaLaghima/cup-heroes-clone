// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System.Collections;
using System.Collections.Generic;
using CupHeroesClone.Gameplay;
using CupHeroesClone.Common.Pool;
using UnityEngine;

namespace CupHeroesClone.Gameplay.Enemy
{
    [DisallowMultipleComponent]
    public class EnemyFactory : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private List<GameObject> spawnPoints;
        [Space]
        [SerializeField] private int spawnRate = 1;

        private int _targetSpawnCount;
        private int _currentSpawnCount = 0;
        private List<Enemy> spawnedEnemies = new List<Enemy>();
        private ObjectPool _enemyPool;

        private int _indexOfLastSpawner;

        #endregion
        
        
        #region MonoBehavior

        private void Awake()
        {
            //
        }

        #endregion
        
        
        #region Public Methods

        public void Init()
        {
            CreateEnemyPool();
        }

        public void SpawnEnemies(int amount)
        {
            spawnedEnemies.Clear();
            _targetSpawnCount = amount;
            _currentSpawnCount = 0;
            StartCoroutine(SpawnEnemies());
        }

        public void WithdrawEnemies()
        {
            foreach (Enemy enemy in spawnedEnemies)
                TakeEnemyBack(enemy);
            
            spawnedEnemies.Clear();
        }
        
        #endregion
        
        
        #region Private Methods

        private void CreateEnemyPool()
        {
            GameObject go = new GameObject("EnemyPool");
            go.transform.SetParent(transform, false);
            _enemyPool = go.AddComponent<ObjectPool>();
            _enemyPool.Init(enemyPrefab);
        }

        private GameObject GeNextSpawnPoint()
        {
            int nextIndex = Random.Range(0, spawnPoints.Count);
            while (nextIndex == _indexOfLastSpawner)
                nextIndex = Random.Range(0, spawnPoints.Count);
            
            _indexOfLastSpawner = nextIndex;
            return spawnPoints[nextIndex];
        }

        private void TakeEnemyBack(Enemy enemy)
        {
            enemy.Clear();
            _enemyPool.Return(enemy.gameObject);
        }
        
        #endregion
        
        
        #region Coroutines

        private IEnumerator SpawnEnemies()
        {
            while (_currentSpawnCount < _targetSpawnCount)
            {
                yield return new WaitForSeconds(1f / spawnRate);
                
                GameObject spawnPoint = GeNextSpawnPoint();
                GameObject enemyObj = _enemyPool.Borrow();
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                spawnedEnemies.Add(enemy);
                
                enemy.OnUnitDeath.AddListener(() =>
                {
                    GameManager.Instance.CountEnemyDeath();
                    TakeEnemyBack(enemy);
                });
                
                enemy.ActivateAt(spawnPoint.transform);
                _currentSpawnCount++;
            }
        }
        
        #endregion
    }
}
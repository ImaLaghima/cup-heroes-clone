// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Gameplay.User;
using CupHeroesClone.Gameplay.Enemy;
using CupHeroesClone.UI;
using UnityEngine;

namespace CupHeroesClone.Gameplay
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        #region Fields

        [Header("General")]
        [SerializeField] private UIManager uiManager;
        
        [Header("Combat")]
        [SerializeField] private EnemyFactory enemyFactory;
        [SerializeField] private int firstWaveSize = 2;
        [SerializeField] private int increaseEachWave = 2;
        [SerializeField] private int moneyPerKill = 2;

        private int _waveCounter = 0;
        private int _waveSize;
        private int _enemiesKillCounter = 0;
        
        #endregion
        
        
        #region MonoBehavior

        private void Awake()
        {
            EnsureSingleton();
        }

        private void Start()
        {
            Player.Instance.Init();
            enemyFactory.Init();
        }
        
        #endregion
        
        
        #region Public Methods

        public void CountEnemyDeath()
        {
            Player.Instance.AddMoney(moneyPerKill);
            _enemiesKillCounter += 1;

            if (_enemiesKillCounter >= _waveSize)
                RewardPlayer();
        }
        
        public void CountPlayerDeath()
        {
            GameOver();
        }
        
        #endregion
        
        
        #region Private Methods

        [ContextMenu("StartCombat()")]
        private void StartCombat()
        {
            _enemiesKillCounter = 0;
            _waveSize = firstWaveSize + _waveCounter * increaseEachWave;
            _waveCounter++;
            
#if UNITY_EDITOR
            Debug.Log($"Wave {_waveCounter}: {_waveSize} enemies");
#endif
            
            enemyFactory.SpawnEnemies(_waveSize);
            Player.Instance.StartCombat();
        }

        private void RewardPlayer()
        {
            Player.Instance.StopCombat();
            // open overlay
        }

        private void GameOver()
        {
            // open overlay
        }
        
        private void EnsureSingleton()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        
        #endregion
    }
}
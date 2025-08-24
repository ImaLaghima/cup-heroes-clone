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

        private int _waveCounter = 0;
        private int _nextWaveSize;
        
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
            
        }
        
        #endregion
        
        
        #region Private Methods

        [ContextMenu("StartCombat()")]
        private void StartCombat()
        {
            _nextWaveSize = firstWaveSize + _waveCounter * increaseEachWave;
            _waveCounter++;
#if UNITY_EDITOR
            Debug.Log($"Wave {_waveCounter}: {_nextWaveSize} enemies");
#endif
            
            enemyFactory.SpawnEnemies(_nextWaveSize);
            Player.Instance.StartCombat();
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
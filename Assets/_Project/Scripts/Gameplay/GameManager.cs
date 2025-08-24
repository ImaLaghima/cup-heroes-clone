// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System;
using System.Collections;
using CupHeroesClone.Common;
using CupHeroesClone.Gameplay.Basic;
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
        
        [Header("Combat")]
        [SerializeField] private EnemyFactory enemyFactory;
        [SerializeField] private int firstWaveSize = 2;
        [SerializeField] private int increaseEachWaveBy = 2;
        [SerializeField] private int moneyPerKill = 2;
        [Space]
        [SerializeField] private float runForSeconds = 3f;

        private int _waveCounter = 0;
        private int _enemiesInWave = 0;
        private int _enemiesKillCounter = 0;
        
        #endregion
        
        
        #region MonoBehavior

        private void Awake()
        {
            EnsureSingleton();
        }

        private void Start()
        {
            UIManager.Instance.Init();
            Player.Instance.Init();
            enemyFactory.Init();
        }
        
        #endregion
        
        
        #region Public Methods

        public void StartNewGame()
        {
            AudioManager.Instance.PlayNextMelody();
            StartNextWave();
        }

        public void StartNextWave()
        {
            StartCoroutine(StartNextWaveRoutine());
        }
        
        public void SetGamePause(bool value)
        {
            if (value)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }

        public void CountEnemyDeath()
        {
            Player.Instance.AddMoney(moneyPerKill);
            _enemiesKillCounter += 1;

            if (_enemiesKillCounter >= _enemiesInWave)
                RewardPlayer();
        }
        
        public void CountPlayerDeath()
        {
            AudioManager.Instance.PlayGameOver();
            GameOver();
        }

        public void PurchaseUpgrade(UpgradeTarget upgradeTarget, float value, float cost)
        {
            Player.Instance.UpgradeHero(upgradeTarget, value);
            Player.Instance.TryWithdrawMoney(cost);
        }
        
        #endregion
        
        
        #region Private Methods
        
        private void StartCombat()
        {
            _enemiesKillCounter = 0;
            _enemiesInWave = firstWaveSize + _waveCounter * increaseEachWaveBy;
            _waveCounter++;
            
            Debug.Log($"Wave {_waveCounter}: {_enemiesInWave} enemies");
            
            enemyFactory.SpawnEnemies(_enemiesInWave);
            Player.Instance.StartCombat();
        }

        private void RewardPlayer()
        {
            Player.Instance.StopCombat();
            UIManager.Instance.ShowRewardScreen();
            // TODO: GameManager has to pass parameters, in order
            // to configure reward options, not random bs
        }

        private void GameOver()
        {
            Player.Instance.StopCombat();
            enemyFactory.WithdrawEnemies();
            
            int wavesSurvived = Util.Clamp(_waveCounter - 1, 0, Int32.MaxValue);
            UIManager.Instance.ShowGameOver(wavesSurvived);
            
            _waveCounter = 0;
            _enemiesInWave = 0;
            _enemiesKillCounter = 0;
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
        
        
        #region Coroutines

        private IEnumerator StartNextWaveRoutine()
        {
            Player.Instance.RunForward();
            yield return new WaitForSeconds(runForSeconds);
            StartCombat();
        }
        
        #endregion
    }
}
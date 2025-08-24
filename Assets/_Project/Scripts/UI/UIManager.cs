// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Common.Pool;
using CupHeroesClone.Gameplay;
using CupHeroesClone.Gameplay.User;
using CupHeroesClone.UI.Components;
using UnityEngine;

namespace CupHeroesClone.UI
{
    [DisallowMultipleComponent]
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        
        #region Fields
        
        [Header("Screens")]
        [SerializeField] private GameObject screensParent;
        [SerializeField] private GameObject overlaysParent;
        [Space]
        [SerializeField] private GameObject homeScreenPrefab;
        [SerializeField] private GameObject hudOverlayPrefab;
        [SerializeField] private GameObject rewardOverlayPrefab;
        [SerializeField] private GameObject pauseOverlayPrefab;
        [SerializeField] private GameObject gameOverScreenPrefab;
        
        private HomeScreen _homeScreen;
        private HudOverlay _hudOverlay;
        private RewardOverlay _rewardOverlay;
        private PauseOverlay _pauseOverlay;
        private GameOverScreen _gameOverScreen;
        
        [Header("Gameplay Overlays")]
        [SerializeField] private GameObject damageOverlay;
        [SerializeField] private GameObject damageTextPrefab;
        [SerializeField] private GameObject healthBarOverlay;
        [SerializeField] private GameObject healthBarPrefab;
        [Space]
        [SerializeField] private GameObject backgroundOverlay;

        private ObjectPool _healthBarPool;
        private ObjectPool _damageTextPool;
        
        #endregion
        
        
        #region MonoBehavior
        
        private void Awake()
        {
            EnsureSingleton();
        }
        
        #endregion
        
        
        #region Public Methods

        public void Init()
        {
            CreateDamageTextPool();
            CreateHealthBarPool();
            CreateScreens();
            backgroundOverlay.SetActive(false);
        }

        public GameObject GetHealthBarObject()
        {
            return _healthBarPool.Borrow();
        }

        public void ReturnHealthBarObject(GameObject healthBarObj)
        {
            if (!healthBarObj) return;
            healthBarObj.SetActive(false);
            _healthBarPool.Return(healthBarObj);
        }

        public void ShowDamage(float damage, Vector3 position)
        {
            GameObject damageTextObj = _damageTextPool.Borrow();
            DamageText damageText = damageTextObj.GetComponent<DamageText>();
            
            damageText.Init(damage, position);
            damageText.OnAnimationFinish.AddListener(() =>
            {
                damageText.Clear();
                _damageTextPool.Return(damageTextObj);
            });
            
            damageText.Show();
        }

        public void ShowRewardScreen()
        {
            backgroundOverlay.SetActive(true);
            _rewardOverlay.gameObject.SetActive(true);
            _rewardOverlay.Init();
            _rewardOverlay.OnRewardComplete.AddListener(() =>
            {
                backgroundOverlay.SetActive(false);
                _rewardOverlay.gameObject.SetActive(false);
                _rewardOverlay.Clear();
                GameManager.Instance.StartNextWave();
            });
        }

        public void ShowGameOver(int waveSurvived)
        {
            backgroundOverlay.SetActive(true);
            _gameOverScreen.gameObject.SetActive(true);
            _gameOverScreen.Init(waveSurvived);
            _gameOverScreen.OnRestartGame.AddListener(() =>
            {
                backgroundOverlay.SetActive(false);
                _gameOverScreen.Clear();
                _gameOverScreen.gameObject.SetActive(false);
                _rewardOverlay.Clear();
                _rewardOverlay.gameObject.SetActive(false);
                Player.Instance.Restart();
                GameManager.Instance.StartNewGame();
            });
        }
        
        #endregion
        
        
        #region Private Methods
        
        private void EnsureSingleton()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void CreateDamageTextPool()
        {
            GameObject go = new GameObject("DamageTextPool");
            go.transform.SetParent(damageOverlay.transform, false);
            _damageTextPool = go.AddComponent<ObjectPool>();
            _damageTextPool.Init(damageTextPrefab);
        }

        private void CreateHealthBarPool()
        {
            GameObject go = new GameObject("HealthBarPool");
            go.transform.SetParent(healthBarOverlay.transform, false);
            _healthBarPool = go.AddComponent<ObjectPool>();
            _healthBarPool.Init(healthBarPrefab);
        }

        private void CreateScreens()
        {
            _homeScreen = Instantiate(
                homeScreenPrefab,
                screensParent.transform
            ).GetComponent<HomeScreen>();
            SetupHomeScreen();
            
            _hudOverlay = Instantiate(
                hudOverlayPrefab,
                overlaysParent.transform
            ).GetComponent<HudOverlay>();
            _hudOverlay.gameObject.SetActive(false);
            SetupHudOverlay();
            
            _rewardOverlay = Instantiate(
                rewardOverlayPrefab,
                overlaysParent.transform
            ).GetComponent<RewardOverlay>();
            _rewardOverlay.gameObject.SetActive(false);
            
            _pauseOverlay = Instantiate(
                pauseOverlayPrefab,
                overlaysParent.transform
            ).GetComponent<PauseOverlay>();
            _pauseOverlay.gameObject.SetActive(false);
            SetupPauseOverlay();
            
            _gameOverScreen = Instantiate(
                gameOverScreenPrefab,
                screensParent.transform
            ).GetComponent<GameOverScreen>();
            _gameOverScreen.gameObject.SetActive(false);
        }

        private void SetupHomeScreen()
        {
            _homeScreen.Init();
            _homeScreen.OnGameStart.AddListener(() =>
            {
                GameManager.Instance.StartNewGame();
                _homeScreen.gameObject.SetActive(false);
                
                _hudOverlay.gameObject.SetActive(true);
                _hudOverlay.UpdateNumbers();
            });
        }

        private void SetupHudOverlay()
        {
            _hudOverlay.Init();
            _hudOverlay.OnGamePause.AddListener(() =>
            {
                backgroundOverlay.SetActive(true);
                _pauseOverlay.gameObject.SetActive(true);
                GameManager.Instance.SetGamePause(true);
            });
        }
        
        private void SetupPauseOverlay()
        {
            _pauseOverlay.Init();
            _pauseOverlay.OnGameContinue.AddListener(() =>
            {
                backgroundOverlay.SetActive(false);
                _pauseOverlay.gameObject.SetActive(false);
                GameManager.Instance.SetGamePause(false);
            });
        }
        
        #endregion
    }
}

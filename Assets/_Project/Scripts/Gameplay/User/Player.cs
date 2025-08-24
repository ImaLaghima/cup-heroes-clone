// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Common;
using CupHeroesClone.Gameplay.Basic;
using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone.Gameplay.User
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        
        #region Fields

        [SerializeField] private float moneyBalance;
        [SerializeField] private float moneyBalanceMin = 0;
        [SerializeField] private float moneyBalanceMax = 999;
        [Space]
        [SerializeField] GameObject heroPrefab;
        [SerializeField] GameObject heroSpawnPoint;
        
        private Hero _hero;
        
        #endregion
        
        
        #region Properties

        public float MoneyBalance
        {
            get => moneyBalance;
            private set
            {
                moneyBalance = Util.Clamp(value, moneyBalanceMin, moneyBalanceMax);
                OnBalanceChange.Invoke(moneyBalance);
            }
        }
        
        public float MaxHealth => _hero.MaxHealth;
        public float Health => _hero.Health;
        public float AttackDamage => _hero.AttackDamage;
        public float AttackSpeed => _hero.AttackSpeed;
        
        #endregion
        
        
        #region Events
        
        public readonly UnityEvent<float> OnBalanceChange = new UnityEvent<float>();
        
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
            GameObject heroObj = Instantiate(heroPrefab, heroSpawnPoint.transform);
            _hero = heroObj.GetComponent<Hero>();
            _hero.OnUnitDeath.AddListener(() =>
            {
                GameManager.Instance.CountPlayerDeath();
                _hero.OnUnitDeath.RemoveAllListeners();
            });
            _hero.Init();
        }

        public void StartCombat()
        {
            _hero.StartCombat();
        }

        public void StopCombat()
        {
            _hero.StopCombat();
        }

        public void RunForward()
        {
            _hero.RunForward();
        }

        public void AddMoney(float amount)
        {
            MoneyBalance += amount;
        }
        
        public bool TryWithdrawMoney(float amount, out float actualBalance)
        {
            if (MoneyBalance - amount < moneyBalanceMin)
            {
                MoneyBalance -= amount;
                actualBalance = MoneyBalance;
                return true;
            }
            
            actualBalance = MoneyBalance;
            return false;
        }

        public void UpgradeHero(UpgradeTarget target, float amount)
        {
            _hero.Upgrade(target, amount);
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
        
        #endregion
    }
}
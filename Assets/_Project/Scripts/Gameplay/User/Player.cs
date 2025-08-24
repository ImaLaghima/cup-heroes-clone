// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Common;
using CupHeroesClone.Gameplay.Basic;
using CupHeroesClone.UI.Components;
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
        [Space]
        [SerializeField] private InfiniteBackground background;
        
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
        public readonly UnityEvent<float> OnMaxHealthChange = new UnityEvent<float>();
        public readonly UnityEvent<float> OnAttackDamageChange = new UnityEvent<float>();
        public readonly UnityEvent<float> OnAttackSpeedChange = new UnityEvent<float>();
        
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
            background.Init();
            
            GameObject heroObj = Instantiate(heroPrefab, heroSpawnPoint.transform);
            _hero = heroObj.GetComponent<Hero>();
            _hero.OnUnitDeath.AddListener(() =>
            {
                GameManager.Instance.CountPlayerDeath();
                _hero.OnUnitDeath.RemoveAllListeners();
            });
            _hero.Init();
        }

        public void Restart()
        {
            _hero.RestoreDefault();
            MoneyBalance = 10;
            
            _hero.OnUnitDeath.AddListener(() =>
            {
                GameManager.Instance.CountPlayerDeath();
                _hero.OnUnitDeath.RemoveAllListeners();
            });
        }

        public void StartCombat()
        {
            background.StopScrolling();
            _hero.StartCombat();
        }

        public void StopCombat()
        {
            background.StopScrolling();
            _hero.StopCombat();
        }

        public void RunForward()
        {
            background.StartScrolling();
            _hero.RunForward();
        }

        public void AddMoney(float amount)
        {
            MoneyBalance += amount;
        }
        
        public bool TryWithdrawMoney(float amount)
        {
            if (MoneyBalance - amount >= moneyBalanceMin)
            {
                MoneyBalance -= amount;
                return true;
            }
            
            return false;
        }

        public void UpgradeHero(UpgradeTarget target, float amount)
        {
            _hero.Upgrade(target, amount);

            switch (target)
            {
                case UpgradeTarget.MaxHealth:
                    OnMaxHealthChange.Invoke(MaxHealth);
                    break;
                
                case UpgradeTarget.AttackDamage:
                    OnAttackDamageChange.Invoke(AttackDamage);
                    break;
                
                case UpgradeTarget.AttackSpeed:
                    OnAttackSpeedChange.Invoke(AttackSpeed);
                    break;
            }
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
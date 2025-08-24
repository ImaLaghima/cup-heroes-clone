// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Common;
using CupHeroesClone.UI;
using CupHeroesClone.UI.Components;
using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone.Gameplay.Basic
{
    [DisallowMultipleComponent]
    public class CombatUnit : MonoBehaviour
    {
        #region Fields
        
        [Header("Combat Unit")]
        [SerializeField] protected float minHealth = 0f;
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float currentHealth = 100f;
        [Space]
        [SerializeField] protected float attackDamage = 10f;
        [SerializeField] protected float attackSpeed = 2f;
        [Space]
        [SerializeField] protected GameObject healthBarOrigin;
        [SerializeField] protected GameObject projectileTargetOrigin;
        [SerializeField] protected GameObject damageTextSpawnOrigin;
        [Space]
        [SerializeField] protected Rigidbody2D rigidbodyComponent;
        [SerializeField] protected Collider2D bodyCollider;
        [SerializeField] protected Collider2D attackCollider;
        
        protected GameObject healthBarObj;
        protected HealthBar healthBar;

        #endregion
        
        
        #region Properties
        
        public GameObject ProjectileTargetOrigin => projectileTargetOrigin;

        public float Health
        {
            get => currentHealth;
            protected set
            {
                currentHealth = Util.Clamp(value, minHealth, maxHealth);
                OnHealthChange.Invoke(currentHealth);
                if (currentHealth <= 0)
                    HandleDeath();
            }
        }

        public float MaxHealth
        {
            get => maxHealth;
            protected set => maxHealth = value;
        }
        
        public float MinHealth
        {
            get => minHealth;
            protected set => minHealth = value;
        }

        public float AttackDamage
        {
            get => attackDamage;
            protected set => attackDamage = value;
        }

        public float AttackSpeed
        {
            get => attackSpeed;
            protected set => attackSpeed = value;
        }
        
        #endregion
        
        
        #region Events
        
        public readonly UnityEvent<float> OnHealthChange = new UnityEvent<float>();
        public readonly UnityEvent OnUnitDeath = new UnityEvent();
        
        #endregion
        
        
        #region Public Methods

        public virtual void Init()
        {
            SetupHealthBar();
        }

        public void Upgrade(UpgradeTarget target, float amount)
        {
            switch (target)
            {
                case UpgradeTarget.MaxHealth:
                    maxHealth += amount;
                    Health += amount;
                    break;
                case UpgradeTarget.AttackDamage:
                    attackDamage += amount;
                    break;
                case UpgradeTarget.AttackSpeed:
                    attackSpeed += amount;
                    break;
            }
        }

        public void ReceiveDamage(float amount)
        {
            TakeDamage(amount);
        }
        
        public virtual void Clear()
        {
            ReleaseHealthBar();
            RestoreDefault();
            OnHealthChange.RemoveAllListeners();
            OnUnitDeath.RemoveAllListeners();
        }

        public virtual void RestoreDefault()
        {
            Health = MaxHealth;
        }
        
        #endregion
        
        
        #region Protected Methods

        protected virtual void SetupHealthBar()
        {
            healthBarObj = UIManager.Instance.GetHealthBarObject();
            healthBar = healthBarObj.GetComponent<HealthBar>();
            
            healthBar?.Init(minHealth, maxHealth);
            healthBar?.UpdatePosition(healthBarOrigin.transform.position);
            healthBarObj.SetActive(true);
            healthBar?.UpdateNumber(currentHealth);
        }

        protected virtual void ReleaseHealthBar()
        {
            if (healthBarObj)
            {
                UIManager.Instance.ReturnHealthBarObject(healthBarObj);
                healthBarObj = null;
                healthBar = null;
            }
        }
        
        #endregion
        
        
        #region Private Methods

        private void TakeDamage(float amount)
        {
            Health -= amount;
            healthBar?.UpdateNumber(Health);
            UIManager.Instance.ShowDamage(amount, damageTextSpawnOrigin.transform.position);
        }

        private void HandleDeath()
        {
            ReleaseHealthBar();
            OnUnitDeath.Invoke();
        }
        
        #endregion
    }
}
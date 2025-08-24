// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Common;
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
        [Space]
        [SerializeField] protected Rigidbody2D rigidbody2D;
        [SerializeField] protected Collider2D bodyCollider;
        [SerializeField] protected Collider2D attackCollider;

        #endregion
        
        
        #region Properties

        public float Health
        {
            get => currentHealth;
            private set
            {
                currentHealth = Util.Clamp(value, minHealth, maxHealth);
                OnHealthChange.Invoke(currentHealth);
                if (currentHealth <= 0)
                    OnUnitDeath.Invoke();
            }
        }
        
        #endregion
        
        
        #region Events
        
        public readonly UnityEvent<float> OnHealthChange = new UnityEvent<float>();
        public readonly UnityEvent OnUnitDeath = new UnityEvent();
        
        #endregion
        
        
        #region Public Methods

        public void Upgrade(UpgradeTarget target, float amount)
        {
            switch (target)
            {
                case UpgradeTarget.Health:
                    maxHealth += amount;
                    currentHealth += amount;
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
        
        #endregion
        
        
        #region Private Methods

        private void TakeDamage(float amount)
        {
            Health -= amount;
            // change health bar somehow
        }
        
        #endregion
    }
}
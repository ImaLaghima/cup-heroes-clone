// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System.Collections;
using System.Collections.Generic;
using CupHeroesClone.Common.Pool;
using CupHeroesClone.Gameplay.Basic;
using UnityEngine;

namespace CupHeroesClone.Gameplay.User
{
    public class Hero : CombatUnit
    {
        #region  Fields

        [Header("Hero")]
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected GameObject projectileSpawnPoint;
        [SerializeField] private LayerMask attackTargetLayers;
        
        private ObjectPool _projectilePool;
        private HeroState _state;
        private Queue<CombatUnit> _targetsOfAttack = new Queue<CombatUnit>();
        private CombatUnit _currentTarget = null;

        #endregion
        
        
        #region Public Methods

        public void Init()
        {
            _state = HeroState.Idle;
            CreateProjectilePool();
        }

        public void StartCombat()
        {
            _state = HeroState.Fight;
            _targetsOfAttack.Clear();
            _currentTarget = null;
            StartCoroutine(Fighting());
        }
        
        public void StopCombat()
        {
            _state = HeroState.Idle;
        }
        
        public void RunForward()
        {
            _state = HeroState.Run;
        }
        
        #endregion
        
        
        #region Private Methods

        private void CreateProjectilePool()
        {
            GameObject go = new GameObject("ProjectilePool");
            go.transform.SetParent(transform, false);
            _projectilePool = go.AddComponent<ObjectPool>();
            _projectilePool.Init(projectilePrefab);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Debug.Log("Hero collide with " + other.name + "; Tag = " + other.tag);

            if (Physics2D.IsTouching(attackCollider, other) && other.CompareTag("Enemy"))
                _targetsOfAttack.Enqueue(other.GetComponentInParent<CombatUnit>());
        }
        
        #endregion
        
        
        #region Coroutines

        private IEnumerator Fighting()
        {
            while (_state == HeroState.Fight)
            {
                while (_targetsOfAttack.Count == 0)
                    yield return null;

                if (_currentTarget == null)
                {
                    _currentTarget = _targetsOfAttack.Dequeue();
                    _currentTarget.OnUnitDeath.AddListener(() => _currentTarget = null);
                }
                
                GameObject projectileObj = _projectilePool.Borrow();
                Projectile projectile = projectileObj.GetComponent<Projectile>();
                projectile.OnTargetReach.AddListener(() =>
                {
                    _currentTarget.ReceiveDamage(attackDamage);
                    projectile.Clear();
                    _projectilePool.Return(projectileObj);
                });
                
                projectile.ActivateAt(projectileSpawnPoint.transform, _currentTarget.ProjectileTargetOrigin);
                
                yield return new WaitForSeconds(1f / attackSpeed);
            }
        }
        
        #endregion
    }
}
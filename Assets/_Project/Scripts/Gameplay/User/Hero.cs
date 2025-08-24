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
        private static readonly int StartRunT = Animator.StringToHash("startRun_t");
        private static readonly int StartCombatT = Animator.StringToHash("startCombat_t");
        private static readonly int HasTargetT = Animator.StringToHash("hasTarget_t");
        private static readonly int StopCombatT = Animator.StringToHash("stopCombat_t");

        #region  Fields

        [Header("Hero")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected GameObject projectileSpawnPoint;
        [SerializeField] private LayerMask attackTargetLayers;
        
        private ObjectPool _projectilePool;
        private List<Projectile> _spawnedProjectiles = new List<Projectile>();
        private HeroState _state;
        private Queue<CombatUnit> _targetsOfAttack = new Queue<CombatUnit>();
        private CombatUnit _currentTarget = null;
        
        private Coroutine _fightingCoroutine;

        #endregion
        
        
        #region Public Methods

        public override void Init()
        {
            _state = HeroState.Idle;
            CreateProjectilePool();
        }

        public override void RestoreDefault()
        {
            MaxHealth = 100;
            MinHealth = 0;
            Health = MaxHealth;
            AttackDamage = 10f;
            AttackSpeed = 1.5f;
        }

        public void StartCombat()
        {
            SetupHealthBar();
            
            animator.SetTrigger(StartCombatT);
            
            _state = HeroState.Fight;
            _currentTarget = null;
            _fightingCoroutine = StartCoroutine(Fighting());
        }
        
        public void StopCombat()
        {
            StopCoroutine(_fightingCoroutine);
            WithdrawProjectiles();
            _targetsOfAttack.Clear();
            ReleaseHealthBar();
            
            animator.ResetTrigger(HasTargetT);
            animator.SetTrigger(StopCombatT);
            _state = HeroState.Idle;
        }
        
        public void RunForward()
        {
            _state = HeroState.Run;
            animator.SetTrigger(StartRunT);
        }
        
        #endregion
        
        
        #region Private Methods

        private void CreateProjectilePool()
        {
            GameObject go = new GameObject("ProjectilePool");
            go.transform.SetParent(transform, false);
            _projectilePool = go.AddComponent<ObjectPool>();
            _projectilePool.Init(projectilePrefab, initialPoolSize: 20);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Physics2D.IsTouching(attackCollider, other) && other.CompareTag("Enemy"))
                _targetsOfAttack.Enqueue(other.GetComponentInParent<CombatUnit>());
        }

        private void WithdrawProjectiles()
        {
            foreach (Projectile projectile in _spawnedProjectiles)
                TakeProjectileBack(projectile);
            
            _spawnedProjectiles.Clear();
        }

        private void TakeProjectileBack(Projectile projectile)
        {
            projectile.Clear();
            _projectilePool.Return(projectile.gameObject);
        }
        
        #endregion
        
        
        #region Coroutines

        private IEnumerator Fighting()
        {
            while (_state == HeroState.Fight)
            {
                while (_targetsOfAttack.Count == 0 && !_currentTarget)
                    yield return null;

                if (_currentTarget == null)
                {
                    _currentTarget = _targetsOfAttack.Dequeue();
                    _currentTarget.OnUnitDeath.AddListener(() => _currentTarget = null);
                    animator.SetTrigger(HasTargetT);
                }
                
                GameObject projectileObj = _projectilePool.Borrow();
                Projectile projectile = projectileObj.GetComponent<Projectile>();
                _spawnedProjectiles.Add(projectile);
                
                projectile.OnTargetReach.AddListener(() =>
                {
                    _currentTarget?.ReceiveDamage(attackDamage);
                });

                projectile.OnProjectileEnd.AddListener(() =>
                {
                    TakeProjectileBack(projectile);
                });
                
                projectile.ActivateAt(projectileSpawnPoint.transform, _currentTarget.ProjectileTargetOrigin);
                
                yield return new WaitForSeconds(1f / attackSpeed);
            }
        }
        
        #endregion
    }
}
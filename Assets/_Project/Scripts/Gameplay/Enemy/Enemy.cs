// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System.Collections;
using CupHeroesClone.Common;
using CupHeroesClone.Gameplay.Basic;
using UnityEngine;

namespace CupHeroesClone.Gameplay.Enemy
{
    public class Enemy : CombatUnit
    {
        #region Fields

        [Header("Enemy")]
        [SerializeField] protected float moveVelocity;
        [SerializeField] protected MoveDirection moveDirection = MoveDirection.Left;
        
        private Vector2 _effectiveMoveDirection;
        private CombatUnit _attackTarget;
        
        protected EnemyState State;

        #endregion
        
        
        #region Events
        
        //
        
        #endregion


        #region MonoBehavior

        private void OnEnable()
        {
            _effectiveMoveDirection = moveDirection == MoveDirection.Left ? Vector2.left : Vector2.right;
            State = EnemyState.Move;
            StartMoving();
        }

        private void FixedUpdate()
        {
            
                
        }
        
        private void OnDisable()
        {
            State = EnemyState.Move;
        }

        #endregion
        
        
        #region Public Methods

        public void ActivateAt(Transform placeTransform)
        {
            transform.position = placeTransform.position;
            gameObject.SetActive(true);
        }

        public void Clear()
        {
            OnHealthChange.RemoveAllListeners();
            OnUnitDeath.RemoveAllListeners();
        }
        
        #endregion
        
        
        #region Private Methods

        private void StartMoving()
        {
            rigidbody2D.linearVelocity = _effectiveMoveDirection * moveVelocity;
        }

        private void StartFighting()
        {
            State = EnemyState.Fight;
            rigidbody2D.linearVelocity = Vector2.zero;
            StartCoroutine(Fighting());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Enemy collide with " + other.name + "; Tag = " + other.tag);
            
            if (Physics2D.IsTouching(attackCollider, other) && other.CompareTag("Hero"))
            {
                _attackTarget = other.GetComponentInParent<CombatUnit>();
                StartFighting();
            }
        }

        #endregion
        
        
        #region Coroutines

        private IEnumerator Fighting()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / attackSpeed);
                _attackTarget.ReceiveDamage(attackDamage);
            }
            
            yield return null;
        }
        
        #endregion
    }
}
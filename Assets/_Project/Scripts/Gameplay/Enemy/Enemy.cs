// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System.Collections;
using CupHeroesClone.Animation;
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
        private CombatUnit _targetOfAttack;
        
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

        private void LateUpdate()
        {
            healthBar?.UpdatePosition(healthBarOrigin.transform.position);
        }
        
        private void OnDisable()
        {
            State = EnemyState.Move;
        }

        #endregion
        
        
        #region Public Methods

        public void ActivateAt(Transform placeTransform)
        {
            base.Init();
            transform.localScale = new Vector3(-1, 1, 1);
            transform.position = placeTransform.position;
            gameObject.SetActive(true);
        }
        
        #endregion
        
        
        #region Private Methods

        private void StartMoving()
        {
            rigidbodyComponent.linearVelocity = _effectiveMoveDirection * moveVelocity;
        }

        private void StartFighting()
        {
            State = EnemyState.Fight;
            rigidbodyComponent.linearVelocity = Vector2.zero;
            StartCoroutine(Fighting());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Physics2D.IsTouching(attackCollider, other) && other.CompareTag("Hero"))
            {
                _targetOfAttack = other.GetComponentInParent<CombatUnit>();
                if (State != EnemyState.Fight)
                    StartFighting();
            }
        }

        #endregion
        
        
        #region Coroutines

        private IEnumerator Fighting()
        {
            while (true)
            {
                AnimationManager.Instance.PlayMeleeLunge(transform,() =>
                {
                    _targetOfAttack.ReceiveDamage(attackDamage);
                });
                yield return new WaitForSeconds(1f / attackSpeed);
            }
        }
        
        #endregion
    }
}
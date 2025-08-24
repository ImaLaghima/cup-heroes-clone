// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System;
using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone.Gameplay.Basic
{
    public class Projectile : MonoBehaviour
    {
        #region Fields
        
        [Header("Projectile")]
        [SerializeField] private float speed = 12f;
        [SerializeField] private float arcHeightRatio = 0.2f;
        [SerializeField] private float hitRadius = 0.2f;
        [SerializeField] private float fallbackLifetime = 1f;
        
        private Vector3 _endPosition;
        private bool _isActive;
        private float _timeSinceStart;
        private GameObject _target;
        
        #endregion
        
        
        #region Events
        
        public readonly UnityEvent OnTargetReach = new UnityEvent();
        public readonly UnityEvent OnProjectileEnd = new UnityEvent();
        
        #endregion
        
        
        #region MonoBehaviour
        
        private void FixedUpdate()
        {
            if (!_isActive)
                EndProjectile();

            _timeSinceStart += Time.fixedDeltaTime;
            if (_timeSinceStart >= fallbackLifetime)
                EndProjectile();
            
            try
            {
                _endPosition = _target.transform.position;
            }
            catch
            {
                EndProjectile();
                return;
            }
            
            Vector3 currentPosition = transform.position;
            Vector2 toEnd2D = new Vector2(_endPosition.x - currentPosition.x, _endPosition.y - currentPosition.y);
            float planarDistance = toEnd2D.magnitude;
            if (planarDistance <= hitRadius)
                TargetReached();

            // TODO: fix trajectory, recalculates each time - doesnt look parabolic
            
            // Формируем очень пологую параболу (квадр. Безье) на текущем шаге:
            // старт — текущая позиция, конец — актуальная конечная позиция, контрольная — середина + небольшой подъём
            Vector3 midpoint = (currentPosition + _endPosition) * 0.5f;
            float arcHeightWorld = planarDistance * Mathf.Max(0f, arcHeightRatio);
            Vector3 controlPoint = new Vector3(midpoint.x, midpoint.y + arcHeightWorld, midpoint.z);

            // Нормированный шаг вдоль дуги (приближение по хорде для простоты)
            float normalizedStep = Mathf.Clamp01((speed / Mathf.Max(planarDistance, 0.001f)) * Time.fixedDeltaTime);

            Vector3 nextPosition = EvaluateQuadraticBezier(currentPosition, controlPoint, _endPosition, normalizedStep);
            transform.position = nextPosition;
        }
        
        #endregion
        
        
        #region Public Methods

        public void ActivateAt(Transform placeTransform, GameObject target)
        {
            _target = target;
            transform.position = placeTransform.position;
            _endPosition = _target.transform.position;
            _timeSinceStart = 0;
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Clear()
        {
            OnTargetReach.RemoveAllListeners();
            OnProjectileEnd.RemoveAllListeners();
            _target = null;
            _isActive = false;
        }
        
        #endregion
        
        
        #region Private Methods

        private static Vector3 EvaluateQuadraticBezier(
            Vector3 startPosition,
            Vector3 controlPoint,
            Vector3 endPosition,
            float normalizedTime
        )
        {
            float clamped = Mathf.Clamp01(normalizedTime);
            float oneMinus = 1f - clamped;

            // B(t) = (1 - t)^2 * A + 2(1 - t)t * C + t^2 * B
            return oneMinus * oneMinus * startPosition
                   + 2f * oneMinus * clamped * controlPoint
                   + clamped * clamped * endPosition;
        }

        private void EndProjectile()
        {
            _isActive = false;
            OnProjectileEnd.Invoke();
        }

        private void TargetReached()
        {
            OnTargetReach.Invoke();
            EndProjectile();
        }

        #endregion

    }
}
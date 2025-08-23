// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Animation;
using CupHeroesClone.Common;
using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone.UI.Basic
{
    [DisallowMultipleComponent]
    public class NumberPanel : Demonstrative
    {
        [Header("NumberPanel")]
        [SerializeField] protected float minValue = float.MinValue;
        [SerializeField] protected float maxValue = float.MaxValue;
        
        public float CurrentValue { get; private set; }
        
        public readonly UnityEvent OnUpdate = new UnityEvent();
        
        
        #region Public Methods
        
        public virtual void UpdateNumber(float value)
        {
            AnimateUpdate(value);
        }
        
        #endregion
        
        
        #region Protected Methods
        
        protected virtual void AnimateUpdate(float value)
        {
            AnimationManager.Instance.PlayNumberUpdate(
                textElement, CurrentValue, value, () => SetNumber(value)
            );
        }
        
        protected virtual void SetNumber(float value)
        {
            CurrentValue = Util.Clamp(value, minValue, maxValue);
            OnUpdate.Invoke();
        }
        
        #endregion
        
    }
}
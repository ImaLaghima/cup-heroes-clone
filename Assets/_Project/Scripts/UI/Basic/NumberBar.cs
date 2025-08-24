// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Animation;
using CupHeroesClone.Common;
using UnityEngine;
using UnityEngine.UI;

namespace CupHeroesClone.UI.Basic
{
    [DisallowMultipleComponent]
    public class NumberBar : NumberPanel
    {
        #region Fields

        [Header("NumberBar")]
        [SerializeField] protected Direction fillDirection = Direction.Right;
        [SerializeField] protected Image fillImage;

        #endregion
        
        
        #region Public Methods

        public override void Init()
        {
            base.Init();
            UpdateFillDirection();
        }
        
        #endregion
        
        
        #region Protected Methods
        
        protected override void AnimateUpdate(float value)
        {
            AnimationManager.Instance.PlayNumberBarUpdate(
                textElement,
                fillImage,
                CurrentValue,
                value,
                minValue,
                maxValue,
                () => SetNumber(value)
            );
        }
        
        protected virtual void UpdateFillDirection()
        {
            if (fillImage)
            {
                switch (fillDirection)
                {
                    case Direction.Up:
                        fillImage.fillMethod = Image.FillMethod.Vertical;
                        fillImage.fillOrigin = 1;
                        break;
                    case Direction.Down:
                        fillImage.fillMethod = Image.FillMethod.Vertical;
                        fillImage.fillOrigin = 0;
                        break;
                    case Direction.Left:
                        fillImage.fillMethod = Image.FillMethod.Horizontal;
                        fillImage.fillOrigin = 1;
                        break;
                    case Direction.Right:
                        fillImage.fillMethod = Image.FillMethod.Horizontal;
                        fillImage.fillOrigin = 0;
                        break;
                }
            }
        }
        
        #endregion
    }
}

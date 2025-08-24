// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System;
using CupHeroesClone.Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CupHeroesClone.Animation
{
    [DisallowMultipleComponent]
    public class AnimationManager : MonoBehaviour
    {
        public static AnimationManager Instance { get; private set; }
        
        #region Fields
        
        [Header("Press")]
        [SerializeField] private float pressScaleMultiplier = 0.92f;
        [SerializeField] private float pressDuration = 0.2f;
        
        [Header("Release")]
        [SerializeField] private float releaseDuration = 0.2f;
        
        [Header("Number Update")]
        [SerializeField] private float numberUpdateDuration = 0.5f;
        
        [Header("Number Bar Update")]
        [SerializeField] private float numberBarUpdateDuration = 0.5f;

        [Header("Melee Attack")] 
        [SerializeField] private float lungeDistance = 0.5f;
        [SerializeField] private float lungeDuration = 0.2f;
        
        [Header("DamageText")]
        [SerializeField] private float pingScaleMultiplier = 2f;
        [SerializeField] private float pingDuration = 0.2f;
        
        #endregion
        
        
        #region MonoBehavior

        private void Awake()
        {
            EnsureSingleton();
            Init();
        }
        
        #endregion
        
        
        #region UI API
        
        public void PlayPressOn(Transform targetTransform)
        {
            Vector3 pressScale = targetTransform.localScale * pressScaleMultiplier;
            Tween tween = Shrink(targetTransform, pressScale, pressDuration);
            tween.SetId(AnimationFlag.UI | AnimationFlag.Interactive);
            tween.Play();
        }
        
        public void PlayReleaseOn(Transform targetTransform)
        {
            Vector3 growScale = Vector3.one;
            Tween tween = Grow(targetTransform, growScale, releaseDuration);
            tween.SetId(AnimationFlag.UI | AnimationFlag.Interactive);
            tween.Play();
        }

        public void PlayNumberUpdate(
            TextMeshProUGUI textElement,
            float oldValue,
            float newValue,
            Action onComplete = null
        )
        {
            Tween tween = DOTween.To(
                () => oldValue,
                value =>
                {
                    oldValue = value;
                    textElement.text = Util.StringFromNumber(value);
                },
                newValue,
                numberUpdateDuration
            );
            tween.SetId(AnimationFlag.UI | AnimationFlag.Text);
            tween.OnComplete(() => onComplete?.Invoke());
            tween.Play();
        }
        
        public void PlayNumberBarUpdate(
            TextMeshProUGUI tmp,
            Image fillImage,
            float startValue,
            float endValue,
            float minValue,
            float maxValue,
            Action onComplete = null
        )
        {
            // TODO: fix overlapping of animations - bar's fill flicks on high AttackSpeed
            Tween tween = DOTween.To(
                () => startValue, 
                value => 
                {
                    startValue = value;
                    tmp.text = Util.StringFromNumber(value);
                    float normalizedValue = Util.Clamp01(value, minValue, maxValue);
                    fillImage.fillAmount = normalizedValue;
                }, 
                endValue, 
                numberBarUpdateDuration
            );
            tween.SetId(AnimationFlag.UI | AnimationFlag.Text);
            tween.OnComplete(() => onComplete?.Invoke());
            tween.Play();
        }
        
        #endregion
        
        
        #region Gameplay API

        public void PlayMeleeLunge(Transform targetTransform, Action onComplete = null)
        {
            Vector3 originalPosition = targetTransform.position;
            Vector3 lungePosition = originalPosition + targetTransform.right * -lungeDistance;
            
            Sequence lungeSequence = DOTween.Sequence();
            lungeSequence
                .Append(targetTransform.DOMove(lungePosition, lungeDuration / 2)
                    .SetEase(Ease.OutQuad)
                )
                .Append(targetTransform.DOMove(originalPosition, lungeDuration / 2)
                    .SetEase(Ease.InQuad)
                );

            lungeSequence.SetId(AnimationFlag.Gameplay | AnimationFlag.Attack | AnimationFlag.Enemy);
            lungeSequence.OnComplete(() => onComplete?.Invoke());
            lungeSequence.Play();
        }

        public void PlayDamageText(Transform targetTransform, Action onComplete = null)
        {
            Vector3 maxPingScale = targetTransform.localScale * pingScaleMultiplier;
            Vector3 originalScale = targetTransform.localScale;
            
            Tween tween = Grow(targetTransform, maxPingScale, pingDuration);
            tween.SetId(AnimationFlag.UI | AnimationFlag.Text);
            tween.OnComplete(() =>
            {
                targetTransform.localScale = originalScale;
                onComplete?.Invoke();
            });
            
            tween.Play();
        }
        
        #endregion
        
        
        #region Basic Animations

        private Tween Shrink(Transform targetTransform, Vector3 shrinkScale, float shrinkDuration)
        {
            Tween tween = targetTransform.DOScale(shrinkScale, shrinkDuration);
            return tween;
        }
        
        private Tween Grow(Transform targetTransform, Vector3 growScale, float growDuration)
        {
            Tween tween = targetTransform.DOScale(growScale, growDuration);
            return tween;
        }
        
        private Tween Slide(Transform targetTransform, Vector3 slidePosition, float slideDuration)
        {
            Tween tween = targetTransform.DOMove(slidePosition, slideDuration);
            return tween;
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
        
        private void Init()
        {
            DOTween.Init(true, false, LogBehaviour.ErrorsOnly);
            DOTween.SetTweensCapacity(200, 50);
            DOTween.defaultAutoPlay = AutoPlay.None;
        }
        
        #endregion
    }
}
// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Animation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CupHeroesClone.UI.Basic
{
    [DisallowMultipleComponent]
    public class Interactive : Demonstrative
    {
        #region Fields

        [Header("Interactive")]
        [SerializeField] protected Button buttonComponent;
        [SerializeField] protected ButtonExtension buttonExtension;
        [Space]
        [SerializeField] protected bool isInteractionEnabled = true;
        [SerializeField] protected UserAction detectUserActions = UserAction.None;
        [SerializeField] protected UserAction animateUserActions = UserAction.None;
        [SerializeField] protected InteractiveEvent autoInvokeEvents = InteractiveEvent.None;

        #endregion


        #region Events

        public readonly UnityEvent OnClick = new UnityEvent();
        public readonly UnityEvent OnPress = new UnityEvent();
        public readonly UnityEvent OnRelease = new UnityEvent();
        public readonly UnityEvent OnEnter = new UnityEvent();
        public readonly UnityEvent OnLeave = new UnityEvent();

        #endregion


        #region MonoBehaviour

        protected virtual void OnDisable()
        {
            SetListenToEvents(false);
        }

        #endregion


        #region Public Methods

        public override void Init()
        {
            base.Init();
            SetListenToEvents(true);
        }

        #endregion


        #region Event Handlers

        protected virtual void HandleClick()
        {
            if (!isInteractionEnabled)
                return;

            if (animateUserActions.HasFlag(UserAction.Click))
                AnimateClick();

            if (autoInvokeEvents.HasFlag(InteractiveEvent.OnClick))
                OnClick.Invoke();
        }

        protected virtual void HandleEnter()
        {
            if (!isInteractionEnabled)
                return;

            if (animateUserActions.HasFlag(UserAction.Enter))
                AnimateEnter();

            if (autoInvokeEvents.HasFlag(InteractiveEvent.OnEnter))
                OnEnter.Invoke();
        }

        protected virtual void HandleLeave()
        {
            if (!isInteractionEnabled)
                return;

            if (animateUserActions.HasFlag(UserAction.Leave))
                AnimateLeave();

            if (autoInvokeEvents.HasFlag(InteractiveEvent.OnLeave))
                OnLeave.Invoke();
        }

        protected virtual void HandlePress()
        {
            if (!isInteractionEnabled)
                return;

            if (animateUserActions.HasFlag(UserAction.Press))
                AnimatePress();

            if (autoInvokeEvents.HasFlag(InteractiveEvent.OnPress))
                OnPress.Invoke();
        }

        protected virtual void HandleRelease()
        {
            if (!isInteractionEnabled)
                return;

            if (animateUserActions.HasFlag(UserAction.Release))
                AnimateRelease();

            if (autoInvokeEvents.HasFlag(InteractiveEvent.OnRelease))
                OnRelease.Invoke();
        }

        #endregion


        #region Animation Methods

        protected virtual void AnimateClick()
        {
            //
        }

        protected virtual void AnimateEnter()
        {
            //
        }

        protected virtual void AnimateLeave()
        {
            //
        }

        protected virtual void AnimatePress()
        {
            AnimationManager.Instance.PlayPressOn(buttonComponent.gameObject.transform);
        }

        protected virtual void AnimateRelease()
        {
            AnimationManager.Instance.PlayReleaseOn(buttonComponent.gameObject.transform);
        }

        #endregion


        #region Private Methods

        private void SetListenToEvents(bool value)
        {
            if (value)
            {
                if (detectUserActions.HasFlag(UserAction.Click))
                    buttonComponent.onClick.AddListener(HandleClick);

                if (detectUserActions.HasFlag(UserAction.Press))
                    buttonExtension.onEnter.AddListener(HandleEnter);

                if (detectUserActions.HasFlag(UserAction.Release))
                    buttonExtension.onLeave.AddListener(HandleLeave);

                if (animateUserActions.HasFlag(UserAction.Press))
                    buttonExtension.onPress.AddListener(HandlePress);

                if (animateUserActions.HasFlag(UserAction.Release))
                    buttonExtension.onRelease.AddListener(HandleRelease);
            }
            else
            {
                buttonComponent.onClick.RemoveListener(HandleClick);
                buttonExtension.onEnter.RemoveListener(HandleEnter);
                buttonExtension.onLeave.RemoveListener(HandleLeave);
                buttonExtension.onPress.RemoveListener(HandlePress);
                buttonExtension.onRelease.RemoveListener(HandleRelease);
            }
        }

        #endregion
    }
}
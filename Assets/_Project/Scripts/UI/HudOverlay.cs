// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.UI.Basic;
using CupHeroesClone.UI.Components;
using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone.UI
{
    [DisallowMultipleComponent]
    public class HudOverlay : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private PauseButton pauseButton;
        [SerializeField] private BalancePanel balancePanel;
        
        [Header("Hero Stats")]
        [SerializeField] private ResourcePanel healthPanel;
        [SerializeField] private ResourcePanel attackDamage;
        [SerializeField] private ResourcePanel attackSpeed;
        
        #endregion
        
        
        public readonly UnityEvent OnGamePause = new UnityEvent();

        private void OnEnable()
        {
            pauseButton?.onGamePause.AddListener(HandlePauseClick);
        }

        private void OnDisable()
        {
            pauseButton?.onGamePause.RemoveListener(HandlePauseClick);
        }

        private void HandlePauseClick()
        {
            OnGamePause.Invoke();
        }
    }
}

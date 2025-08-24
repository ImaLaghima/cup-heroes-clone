// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Gameplay.User;
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
        [SerializeField] private ResourcePanel maxHealthPanel;
        [SerializeField] private ResourcePanel attackDamagePanel;
        [SerializeField] private ResourcePanel attackSpeedPanel;
        
        #endregion
        
        
        #region Events
        
        public readonly UnityEvent OnGamePause = new UnityEvent();
        
        #endregion

        
        #region MonoBehavior
        
        //
        
        #endregion
        
        
        #region Public Methods

        public void Init()
        {
            pauseButton?.Init();
            pauseButton?.onGamePause.AddListener(HandlePauseClick);
            
            Player.Instance.OnBalanceChange.AddListener(HandlePlayerBalanceChange);
        }

        public void Clear()
        {
            pauseButton?.onGamePause.RemoveAllListeners();
            
            Player.Instance.OnBalanceChange.RemoveListener(HandlePlayerBalanceChange);
        }

        public void UpdateNumbers()
        {
            balancePanel.UpdateNumber(Player.Instance.MoneyBalance);
            maxHealthPanel.UpdateNumber(Player.Instance.MaxHealth);
            attackDamagePanel.UpdateNumber(Player.Instance.AttackDamage);
            attackSpeedPanel.UpdateNumber(Player.Instance.AttackSpeed);
        }
        
        #endregion
        
        
        #region Private Methods

        private void HandlePauseClick()
        {
            OnGamePause.Invoke();
        }

        private void HandlePlayerBalanceChange(float newBalance)
        {
            balancePanel.UpdateNumber(newBalance);
        }
        
        private void HandlePlayerMaxHealthChange(float newMaxBalance)
        {
            maxHealthPanel.UpdateNumber(newMaxBalance);
        }
        
        private void HandlePlayerAttackDamageChange(float newAttackDamage)
        {
            attackDamagePanel.UpdateNumber(newAttackDamage);
        }
        
        private void HandlePlayerAttackSpeedChange(float newAttackSpeed)
        {
            attackSpeedPanel.UpdateNumber(newAttackSpeed);
        }
        
        #endregion
    }
}

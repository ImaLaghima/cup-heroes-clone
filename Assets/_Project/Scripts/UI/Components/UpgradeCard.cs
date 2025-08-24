// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Gameplay.Basic;
using CupHeroesClone.Gameplay.User;
using CupHeroesClone.UI.Basic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CupHeroesClone.UI.Components
{
    public class UpgradeCard : NumberPanel
    {
        #region Fields
        
        [Header("Upgrade Card")]
        [SerializeField] protected Image icon;
        [SerializeField] protected UpgradeTarget upgradeTarget;
        [SerializeField] protected CustomButton buyUpgradeButton;
        
        protected float upgradeValue;
        protected float upgradeCost;
        
        #endregion
        
        
        #region Events
        
        public readonly UnityEvent<UpgradeTarget, float> OnUpgradeBuy = new UnityEvent<UpgradeTarget, float>();
        
        #endregion
        
        
        #region Public Methods

        public void Init(float value, float cost)
        {
            base.Init();
            upgradeValue = value;
            upgradeCost = cost;
            
            SetTextColor(Color.lawnGreen);
            if (upgradeCost <= Player.Instance.MoneyBalance)
            {
                buyUpgradeButton.SetTextColor(Color.red);
                buyUpgradeButton.Deactivate();
                return;
            }
            
            buyUpgradeButton.OnClick.AddListener(HandleBuyUpgradeClick);
        }

        public void Clear()
        {
            buyUpgradeButton?.OnClick.RemoveAllListeners();
        }
        
        #endregion
        
        
        #region Private Methods

        private void HandleBuyUpgradeClick()
        {
            OnUpgradeBuy.Invoke(upgradeTarget, upgradeValue);
        }
        
        #endregion
    }
}
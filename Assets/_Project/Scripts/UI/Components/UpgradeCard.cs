// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Common;
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
        
        public readonly UnityEvent OnPurchaseUpgrade = new UnityEvent();
        
        #endregion
        
        
        #region Public Methods

        public void Init(float value, float cost)
        {
            base.Init();
            buyUpgradeButton.Init();
            
            upgradeValue = value;
            UpdateNumber(upgradeValue);
            
            upgradeCost = cost;
            buyUpgradeButton.SetText(Util.StringFromNumber(upgradeCost));
            
            SetTextColor(Color.lawnGreen);
            if (upgradeCost > Player.Instance.MoneyBalance)
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
            OnPurchaseUpgrade.RemoveAllListeners();
            upgradeValue = 0;
            upgradeCost = 0;
        }
        
        #endregion
        
        
        #region Private Methods

        private void HandleBuyUpgradeClick()
        {
            // OnPurchaseUpgrade.Invoke(upgradeTarget, upgradeValue, upgradeCost);
            OnPurchaseUpgrade.Invoke();
        }
        
        #endregion
    }
}
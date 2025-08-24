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
        [SerializeField] protected CustomButton purchaseUpgradeButton;
        
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
            purchaseUpgradeButton.Init();
            
            upgradeValue = value;
            SetTextColor(Color.lawnGreen);
            UpdateNumber(upgradeValue);
            
            upgradeCost = cost;
            purchaseUpgradeButton.SetText(Util.StringFromNumber(upgradeCost));
            
            if (upgradeCost > Player.Instance.MoneyBalance)
            {
                purchaseUpgradeButton.SetTextColor(Color.red);
                purchaseUpgradeButton.Deactivate();
                return;
            }
            
            purchaseUpgradeButton.OnClick.AddListener(HandleBuyUpgradeClick);
        }

        public void Clear()
        {
            purchaseUpgradeButton.Activate();
            purchaseUpgradeButton.SetTextColor(Color.white);
            purchaseUpgradeButton?.OnClick.RemoveAllListeners();
            OnPurchaseUpgrade.RemoveAllListeners();
            upgradeValue = 0;
            upgradeCost = 0;
        }
        
        public override void UpdateNumber(float value)
        {
            SetText("+" + Util.StringFromNumber(value));
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
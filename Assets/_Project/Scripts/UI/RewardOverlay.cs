// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Gameplay;
using CupHeroesClone.Gameplay.Basic;
using CupHeroesClone.UI.Basic;
using CupHeroesClone.UI.Components;
using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone.UI
{
    [DisallowMultipleComponent]
    public class RewardOverlay : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject maxHealthUpgradeCard;
        [SerializeField] private GameObject attackDamageUpgradeCard;
        [SerializeField] private GameObject attackSpeedUpgradeCard;
        [Space]
        [SerializeField] private CustomButton skipButton;
        
        private UpgradeCard _maxHealthUpgradeCard;
        private UpgradeCard _attackDamageUpgradeCard;
        private UpgradeCard _attackSpeedUpgradeCard;

        #endregion


        #region Events

        public readonly UnityEvent OnRewardComplete = new UnityEvent();

        #endregion


        #region MonoBehavior

        //

        #endregion


        #region Public Methods

        public void Init()
        {
            SetupMaxHealthCard();
            SetupAttackDamageCard();
            SetupAttackSpeedCard();
            
            skipButton.Init();
            skipButton.OnClick.AddListener(() =>
            {
                OnRewardComplete.Invoke();
            });
        }

        public void Clear()
        {
            OnRewardComplete.RemoveAllListeners();
            skipButton.OnClick.RemoveAllListeners();
            _maxHealthUpgradeCard.Clear();
            _attackDamageUpgradeCard.Clear();
            _attackSpeedUpgradeCard.Clear();
        }

        #endregion
        
        
        #region Private Methods

        private void SetupMaxHealthCard()
        {
            float maxHealthUpgradeValue = Random.Range(5, 15);
            float maxHealthUpgradeCost = Random.Range(6, 20);
            _maxHealthUpgradeCard = maxHealthUpgradeCard.GetComponent<UpgradeCard>();
            _maxHealthUpgradeCard.Init(maxHealthUpgradeValue, maxHealthUpgradeCost);
            _maxHealthUpgradeCard.OnPurchaseUpgrade.AddListener(() =>
            {
                GameManager.Instance.PurchaseUpgrade(
                    UpgradeTarget.MaxHealth,
                    maxHealthUpgradeValue,
                    maxHealthUpgradeCost
                );
                OnRewardComplete.Invoke();
            });
        }

        private void SetupAttackDamageCard()
        {
            float attackDamageUpgradeValue = Random.Range(2, 10);
            float attackDamageUpgradeCost = Random.Range(8, 18);
            _attackDamageUpgradeCard = attackDamageUpgradeCard.GetComponent<UpgradeCard>();
            _attackDamageUpgradeCard.Init(attackDamageUpgradeValue, attackDamageUpgradeCost);
            _attackDamageUpgradeCard.OnPurchaseUpgrade.AddListener(() =>
            {
                GameManager.Instance.PurchaseUpgrade(
                    UpgradeTarget.AttackDamage,
                    attackDamageUpgradeValue,
                    attackDamageUpgradeCost
                );
                OnRewardComplete.Invoke();
            });
        }

        private void SetupAttackSpeedCard()
        {
            float attackSpeedUpgradeValue = Random.Range(1, 1);
            float attackSpeedUpgradeCost = Random.Range(30, 50);
            _attackSpeedUpgradeCard = attackSpeedUpgradeCard.GetComponent<UpgradeCard>();
            _attackSpeedUpgradeCard.Init(attackSpeedUpgradeValue, attackSpeedUpgradeCost);
            _attackSpeedUpgradeCard.OnPurchaseUpgrade.AddListener(() =>
            {
                GameManager.Instance.PurchaseUpgrade(
                    UpgradeTarget.AttackSpeed,
                    attackSpeedUpgradeValue,
                    attackSpeedUpgradeCost
                );
                OnRewardComplete.Invoke();
            });
        }
        
        #endregion
    }
}

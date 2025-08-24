// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Animation;
using CupHeroesClone.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone.UI.Components
{
    [DisallowMultipleComponent]
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textElement;
        
        
        public readonly UnityEvent OnAnimationFinish = new UnityEvent();

        
        #region Public Methods
        
        public void Init(float damage, Vector3 worldPosition)
        {
            Vector2 screenPosition = Util.WorldToScreenPosition(worldPosition);
            transform.position = screenPosition;
            
            string damageText = Util.StringFromNumber(damage);
            textElement.text = damageText;
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            AnimationManager.Instance.PlayDamageText(transform, () =>
            {
                OnAnimationFinish.Invoke();
            });
        }

        public void Clear()
        {
            gameObject.SetActive(false);
            OnAnimationFinish.RemoveAllListeners();
        }
        
        #endregion
    }
}
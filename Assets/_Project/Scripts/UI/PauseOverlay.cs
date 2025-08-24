// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Gameplay;
using CupHeroesClone.UI.Basic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CupHeroesClone.UI
{
    [DisallowMultipleComponent]
    public class PauseOverlay : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private Button buttonComponent;
        [SerializeField] private TextMeshProUGUI textComponent;
        
        #endregion

        public readonly UnityEvent OnGameContinue = new UnityEvent();
        
        
        #region Public Methods

        public void Init()
        {
            buttonComponent?.onClick.AddListener(HandleContinueClick);
        }

        public void Clear()
        {
            buttonComponent?.onClick.RemoveAllListeners();
        }
        
        #endregion
        
        
        #region Private Methods

        private void HandleContinueClick()
        {
            OnGameContinue.Invoke();
        }
        
        #endregion
    }
}

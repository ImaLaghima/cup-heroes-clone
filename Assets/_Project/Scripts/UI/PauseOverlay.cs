// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CupHeroesClone.UI
{
    [DisallowMultipleComponent]
    public class PauseOverlay : MonoBehaviour
    {
        [SerializeField] private Button buttonComponent;
        [SerializeField] private TextMeshProUGUI textComponent;

        public readonly UnityEvent OnGameContinue = new UnityEvent();

        private void OnEnable()
        {
            buttonComponent?.onClick.AddListener(HandleContinueClick);
        }
        
        private void OnDisable()
        {
            buttonComponent?.onClick.RemoveListener(HandleContinueClick);
        }

        private void HandleContinueClick()
        {
            OnGameContinue.Invoke();
        }
    }
}

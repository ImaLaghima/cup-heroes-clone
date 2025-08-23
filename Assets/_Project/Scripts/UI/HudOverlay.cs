// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone.UI
{
    [DisallowMultipleComponent]
    public class HudOverlay : MonoBehaviour
    {
        [SerializeField] private PauseButton pauseButton;
        
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

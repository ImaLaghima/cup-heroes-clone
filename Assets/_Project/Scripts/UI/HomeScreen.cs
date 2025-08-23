// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.UI.Components;
using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone.UI
{
    [DisallowMultipleComponent]
    public class HomeScreen : MonoBehaviour
    {
        [SerializeField] private StartGameButton startGameButton;
        
        public readonly UnityEvent OnGameStart = new UnityEvent();


        private void OnEnable()
        {
            startGameButton?.onGameStart.AddListener(HandleStartGameClick);
        }

        private void OnDisable()
        {
            startGameButton?.onGameStart.RemoveListener(HandleStartGameClick);
        }
        

        private void HandleStartGameClick()
        {
            OnGameStart.Invoke();
        }
    }
}



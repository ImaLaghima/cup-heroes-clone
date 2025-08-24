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


        #region MonoBehavior
        
        //
        
        #endregion
        
        
        #region Public Methods

        public void Init()
        {
            startGameButton?.Init();
            startGameButton?.onGameStart.AddListener(HandleStartGameClick);
        }

        public void Clear()
        {
            startGameButton?.onGameStart.RemoveAllListeners();
        }
        
        #endregion
        

        #region Private Methods
        
        private void HandleStartGameClick()
        {
            OnGameStart.Invoke();
        }
        
        #endregion
    }
}



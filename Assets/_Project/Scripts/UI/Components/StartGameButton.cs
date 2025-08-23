// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.UI.Basic;
using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone.UI.Components
{
    [DisallowMultipleComponent]
    public class StartGameButton : CustomButton
    {
        public UnityEvent onGameStart = new UnityEvent();
        
        public override void Init()
        {
            base.Init();
            //
        }

        protected override void HandleClick()
        {
            base.HandleClick();
            onGameStart.Invoke();
            Debug.Log("Game Start");
        }
    }
}

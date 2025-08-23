// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.UI.Basic;
using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone
{
    [DisallowMultipleComponent]
    public class PauseButton : CustomButton
    {
        public UnityEvent onGamePause = new UnityEvent();

        protected override void HandleClick()
        {
            base.HandleClick();
            onGamePause.Invoke();
        }
    }
}

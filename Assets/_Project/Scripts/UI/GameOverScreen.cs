// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Common;
using CupHeroesClone.UI.Basic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CupHeroesClone.UI
{
    [DisallowMultipleComponent]
    public class GameOverScreen : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private TextMeshProUGUI waveSurvivedNumber;
        [SerializeField] private CustomButton restartButton;
        
        #endregion
        
        
        #region Events
        
        public readonly UnityEvent OnRestartGame = new UnityEvent();
        
        #endregion
        
        
        #region Public Methods

        public void Init(int waveSurvived)
        {
            waveSurvivedNumber.text = Util.StringFromNumber(waveSurvived);
            restartButton.Init();
            restartButton.OnClick.AddListener(() =>
            {
                OnRestartGame.Invoke();
            });
        }

        public void Clear()
        {
            OnRestartGame.RemoveAllListeners();
            restartButton.OnClick.RemoveAllListeners();
        }
        
        #endregion
    }
}

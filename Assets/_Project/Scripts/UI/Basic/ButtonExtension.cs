// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CupHeroesClone.UI.Basic
{
    [DisallowMultipleComponent]
    public class ButtonExtension :
        MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        #region Events
        
        public UnityEvent onEnter = new UnityEvent();
        public UnityEvent onPress = new UnityEvent();
        public UnityEvent onRelease = new UnityEvent();
        public UnityEvent onLeave = new UnityEvent();
        
        #endregion
        
        
        #region Public Methods
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            onEnter.Invoke();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            onPress.Invoke();
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            onRelease.Invoke();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            onLeave.Invoke();
        }
        
        #endregion
    }
}
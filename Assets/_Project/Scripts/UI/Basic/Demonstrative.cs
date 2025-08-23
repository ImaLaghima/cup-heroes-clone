// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CupHeroesClone.UI.Basic
{
    [DisallowMultipleComponent]
    public class Demonstrative : MonoBehaviour
    {
        [Header("Demonstrative")]
        [SerializeField] protected Image backgroundImage;
        [SerializeField] protected TextMeshProUGUI textElement;

        public virtual void Init()
        {
            //
        }
    }
}
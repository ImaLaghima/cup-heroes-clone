// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using UnityEngine;
using UnityEngine.UI;

namespace CupHeroesClone.UI.Basic
{
    [DisallowMultipleComponent]
    public class ResourcePanel : NumberPanel
    {
        [Header("Resource Panel")]
        [SerializeField] private Image iconImage;
    }
}
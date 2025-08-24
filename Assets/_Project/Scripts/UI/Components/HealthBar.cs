// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Common;
using CupHeroesClone.UI.Basic;
using UnityEngine;

namespace CupHeroesClone.UI.Components
{
    public class HealthBar : NumberBar
    {
        public void Init(float min, float max)
        {
            base.Init();
            minValue = min;
            maxValue = max;
        }
        
        public void UpdatePosition(Vector3 worldAnchor)
        {
            transform.position = Util.WorldToScreenPosition(worldAnchor);
        }
    }
}
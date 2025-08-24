// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System;

namespace CupHeroesClone.Animation
{
    [Flags]
    public enum AnimationFlag
    {
        None = 0,
        
        UI = 1 << 0,
        Interactive = 1 << 1,
        Text = 1 << 2,
        
        Gameplay = 1 << 16,
        Attack = 1 << 17,
        Enemy = 1 << 18,
    }
}
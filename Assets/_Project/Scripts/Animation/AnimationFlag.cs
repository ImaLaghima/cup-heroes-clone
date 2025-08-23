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
    }
}
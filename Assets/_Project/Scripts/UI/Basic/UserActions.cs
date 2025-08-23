// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System;

namespace CupHeroesClone.UI.Basic
{
    [Flags]
    public enum UserAction : byte
    {
        None = 0,
        Click = 1 << 0,
        Press = 1 << 1,
        Release = 1 << 2,
        Enter = 1 << 3,
        Leave = 1 << 4,
    }
}
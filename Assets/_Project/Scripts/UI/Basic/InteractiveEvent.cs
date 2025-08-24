// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System;

namespace CupHeroesClone.UI.Basic
{
    [Flags]
    public enum InteractiveEvent : byte
    {
        None = 0,
        OnClick = 1 << 0,
        OnPress = 1 << 1,
        OnRelease = 1 << 2,
        OnEnter = 1 << 3,
        OnLeave = 1 << 4,
    }
}
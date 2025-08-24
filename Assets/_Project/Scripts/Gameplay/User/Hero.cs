// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Gameplay.Basic;
using UnityEngine;

namespace CupHeroesClone.Gameplay.User
{
    public class Hero : CombatUnit
    {
        #region  Fields

        [Header("Hero")]
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected GameObject projectileSpawnPoint;
        
        private HeroState _state;

        #endregion
        
        
        #region Public Methods

        public void StartCombat()
        {
            _state = HeroState.Fight;
        }
        
        public void StopCombat()
        {
            _state = HeroState.Idle;
        }
        
        public void RunForward()
        {
            _state = HeroState.Run;
        }
        
        #endregion
        
        
        #region Private Region
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Hero collide with " + other.name + "; Tag = " + other.tag);
            
        }
        
        #endregion
    }
}
// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System.Collections.Generic;
using UnityEngine;

namespace CupHeroesClone
{
    [DisallowMultipleComponent]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        
        #region Fields
        
        [SerializeField] private List<AudioSource> backgroundMusic;
        [SerializeField] private List<AudioSource> gameOverMusic;
        
        private int _lastMelodyIndex;
        private AudioSource _currentMelody;
        
        #endregion


        #region MonoBehavior
        
        private void Awake()
        {
            EnsureSingleton();
        }
        
        #endregion


        #region Public Methods
        
        public void PlayNextMelody()
        {
            _currentMelody?.Stop();
            _currentMelody = GetNextSource();
            _currentMelody.Play();
        }

        public void PlayGameOver()
        {
            gameOverMusic[Random.Range(0, gameOverMusic.Count)].Play();
            _currentMelody?.Stop();
        }
        
        #endregion
        

        #region Private Methods
        
        private void EnsureSingleton()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private AudioSource GetNextSource()
        {
            int nextMelodyIndex = Random.Range(0, backgroundMusic.Count);
            while (nextMelodyIndex == _lastMelodyIndex)
                nextMelodyIndex = Random.Range(0, backgroundMusic.Count);
            
            _lastMelodyIndex = nextMelodyIndex;
            return backgroundMusic[nextMelodyIndex];
        }
        
        #endregion
    }
}

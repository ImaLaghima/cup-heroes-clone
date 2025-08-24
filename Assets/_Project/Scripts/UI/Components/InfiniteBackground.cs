// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System.Collections;
using UnityEngine;

namespace CupHeroesClone.UI.Components
{
    [DisallowMultipleComponent]
    public class InfiniteBackground : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private Transform leftPart;
        [SerializeField] private Transform rightPart;
        [SerializeField] private float speed = 2f;
        
        private float _tileWidth;
        private float _slideDistance;
        private Transform _nextToSwap;
        private Coroutine _scrollCoroutine;
        
        #endregion


        #region Public Methods

        public void Init()
        {
            _tileWidth = rightPart.GetComponent<SpriteRenderer>().bounds.size.x;
            _slideDistance = 0;
            _nextToSwap = leftPart;
        }

        public void StartScrolling()
        {
            _scrollCoroutine = StartCoroutine(Scroll());
        }

        public void StopScrolling()
        {
            StopCoroutine(_scrollCoroutine);
        }
        
        #endregion
        
        
        #region Coroutines

        private IEnumerator Scroll()
        {
            while (true)
            {
                Vector3 slide = Vector3.left * (speed * Time.deltaTime);
                leftPart.localPosition += slide;
                rightPart.localPosition += slide;

                _slideDistance += Mathf.Abs(slide.x);
                if (_slideDistance >= _tileWidth)
                {
                    _slideDistance = 0;
                    
                    _nextToSwap.position = new Vector3(
                        _nextToSwap.position.x + _tileWidth * 2,
                        _nextToSwap.position.y,
                        _nextToSwap.position.z
                    );
                    
                    if (_nextToSwap == leftPart)
                        _nextToSwap = rightPart;
                    else
                        _nextToSwap = leftPart;
                }
                
                yield return null;
            }
        }
        
        #endregion
    }
}
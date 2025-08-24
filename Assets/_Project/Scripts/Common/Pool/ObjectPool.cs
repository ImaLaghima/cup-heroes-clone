// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using System.Collections.Generic;
using UnityEngine;

namespace CupHeroesClone.Common.Pool
{
    public class ObjectPool : MonoBehaviour

    {
        #region Fields
        
        [SerializeField] private const int DefaultPoolSize = 10;
        private Queue<GameObject> _pool;
        private GameObject _prefab;
        
        #endregion
        
        
        #region Public Methods
        
        public void Init(GameObject prefab, int initialPoolSize = DefaultPoolSize)
        {
            _prefab = prefab;
            _pool = new Queue<GameObject>();
            for (int i = 0; i < initialPoolSize; i++)
                CreateNewInstance();
        }

        public GameObject Borrow()
        {
            if (_pool.Count == 0)
                CreateNewInstance();
            
            return _pool.Dequeue();
        }

        public void Return(GameObject instance)
        {
            _pool.Enqueue(instance);
        }
        
        #endregion
        
        
        #region Private Methods

        private void CreateNewInstance()
        {
            GameObject instance = Instantiate(_prefab, transform);
            instance.SetActive(false);
            _pool.Enqueue(instance);
        }
        
        #endregion
    }
}
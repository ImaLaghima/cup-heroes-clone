// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

using CupHeroesClone.Common.Pool;
using CupHeroesClone.UI.Components;
using UnityEngine;

namespace CupHeroesClone.UI
{
    [DisallowMultipleComponent]
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        
        #region Fields
        
        [Header("Damage Overlay")]
        [SerializeField] private GameObject damageOverlay;
        [SerializeField] private GameObject damageTextPrefab;
        
        [Header("HealthBar Overlay")]
        [SerializeField] private GameObject healthBarOverlay;
        [SerializeField] private GameObject healthBarPrefab;

        private ObjectPool _healthBarPool;
        private ObjectPool _damageTextPool;
        
        #endregion
        
        
        #region MonoBehavior
        
        private void Awake()
        {
            EnsureSingleton();
        }
        
        #endregion
        
        
        #region Public Methods

        public void Init()
        {
            CreateDamageTextPool();
            CreateHealtBarPool();
        }

        public GameObject GetHealthBarObject()
        {
            return _healthBarPool.Borrow();
        }

        public void ReturnHealthBarObject(GameObject healthBarObj)
        {
            healthBarObj.SetActive(false);
            _healthBarPool.Return(healthBarObj);
        }

        public void ShowDamage(float damage, Vector3 position)
        {
            GameObject damageTextObj = _damageTextPool.Borrow();
            DamageText damageText = damageTextObj.GetComponent<DamageText>();
            
            damageText.Init(damage, position);
            damageText.OnAnimationFinish.AddListener(() =>
            {
                damageText.Clear();
                _damageTextPool.Return(damageTextObj);
            });
            
            damageText.Show();
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

        private void CreateDamageTextPool()
        {
            GameObject go = new GameObject("DamageTextPool");
            go.transform.SetParent(damageOverlay.transform, false);
            _damageTextPool = go.AddComponent<ObjectPool>();
            _damageTextPool.Init(damageTextPrefab);
        }

        private void CreateHealtBarPool()
        {
            GameObject go = new GameObject("HealthBarPool");
            go.transform.SetParent(healthBarOverlay.transform, false);
            _healthBarPool = go.AddComponent<ObjectPool>();
            _healthBarPool.Init(healthBarPrefab);
        }
        
        #endregion
    }
}

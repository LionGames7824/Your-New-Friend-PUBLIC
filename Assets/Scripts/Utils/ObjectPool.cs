using System;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private uint _initPoolSize;
        [SerializeField] private GameObject _objectToPool;
        [SerializeField] private Transform _container;

        private Stack<GameObject> _pooledObjects = new();
        private List<GameObject> _activeObjects = new();

        public void Initialize(GameObject objectToPool, uint initPoolSize)
        {
            _objectToPool = objectToPool;
            _initPoolSize = initPoolSize;

            SetupPool();
        }

        private void SetupPool()
        {
            for (int i = 0; i < _initPoolSize; i++)
            {
                GameObject instance = Instantiate(_objectToPool, _container);
                instance.gameObject.SetActive(false);
                _pooledObjects.Push(instance);
            }
        }

        public GameObject GetFromPool()
        {
            GameObject instance = _pooledObjects.Count > 0
                ? _pooledObjects.Pop()
                : Instantiate(_objectToPool, _container);

            instance.SetActive(true);
            instance.transform.SetAsLastSibling(); 
            _activeObjects.Add(instance);
            return instance;
        }

        public void ReturnToPool(GameObject obj)
        {
            if (obj == null)
                return;

            if (_activeObjects.Contains(obj))
            {
                _activeObjects.Remove(obj);
                obj.SetActive(false);

                if (!_pooledObjects.Contains(obj))
                {
                    _pooledObjects.Push(obj);
                }
            }
        }

        public void ReturnAllPooledObjects(Action<GameObject> resetAction)
        {
            for (int i = _activeObjects.Count - 1; i >= 0; --i)
            {
                var obj = _activeObjects[i];
                if (obj != null)
                {
                    resetAction?.Invoke(obj);
                    obj.SetActive(false);
                    if (!_pooledObjects.Contains(obj))
                        _pooledObjects.Push(obj);
                }
                _activeObjects.RemoveAt(i);
            }
        }
    }
}

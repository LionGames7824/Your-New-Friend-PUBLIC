using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
            if (_pooledObjects.Count == 0)
            {
                GameObject newInstance = Instantiate(_objectToPool);
                return newInstance;
            }

            GameObject nextInstance = _pooledObjects.Pop();
            nextInstance.gameObject.SetActive(true);
            _activeObjects.Add(nextInstance);
            return nextInstance;
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
                var activePooledObj = _activeObjects[i];
                if (activePooledObj != null)
                {
                    resetAction?.Invoke(activePooledObj);
                    activePooledObj.SetActive(false);

                    if (!_pooledObjects.Contains(activePooledObj))
                    {
                        _pooledObjects.Push(activePooledObj);
                    }
                }
                _activeObjects.Remove(activePooledObj);
            }
        }
    }
}

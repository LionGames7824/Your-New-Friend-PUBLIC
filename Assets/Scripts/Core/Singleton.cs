
using UnityEngine;

namespace YourNewFriend
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        FindOrCreateNewInstance();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            SetInstanceAndRemoveDuplicates();
        }

        private static void FindOrCreateNewInstance()
        {
            _instance = FindObjectOfType<T>();

            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                _instance = obj.AddComponent<T>();
            }
        }

        private void SetInstanceAndRemoveDuplicates()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}

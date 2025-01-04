using UnityEngine;

namespace YourNewFriend
{
    public static class PersistenceDataHelper
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Your New Friend/Wipe Data")]
        public static void WipeDataMenu()
        {
            PlayerPrefs.DeleteAll();
        }
#endif

        public static bool TryPersistData(string key, object data)
        {
            if (data == null || string.IsNullOrEmpty(key))
            {
                return false;
            }

            string json = JsonUtility.ToJson(data);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
            return true;
        }

        public static bool TryLoadPersistedData<T>(string key, out T data)
        {
            if (string.IsNullOrEmpty(key))
            {
                data = default;
                return false;
            }

            string json = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(json))
            {
                data = default;
                return false;
            }

            data = JsonUtility.FromJson<T>(json);
            return data != null;
        }
    }
}

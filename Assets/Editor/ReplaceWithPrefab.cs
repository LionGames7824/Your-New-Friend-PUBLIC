using UnityEngine;
using UnityEditor;

namespace YourNewFriend.Editor
{
    public class ReplaceWithPrefab : ScriptableWizard
    {
        public GameObject prefab;

        [MenuItem("Tools/Replace Selection With Prefab")]
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<ReplaceWithPrefab>("Replace Selection with Prefab", "Replace");
        }

        void OnWizardCreate()
        {
            if (prefab == null) return;

            GameObject[] selection = Selection.gameObjects;

            foreach (GameObject go in selection)
            {
                GameObject newGo = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                newGo.transform.SetParent(go.transform.parent);
                newGo.transform.position = go.transform.position;
                newGo.transform.rotation = go.transform.rotation;
                newGo.transform.localScale = go.transform.localScale;

                Undo.RegisterCreatedObjectUndo(newGo, "Replace With Prefab");
                Undo.DestroyObjectImmediate(go);
            }
        }
    }
}
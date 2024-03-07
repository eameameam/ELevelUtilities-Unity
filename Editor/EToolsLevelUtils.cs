using UnityEditor;
using UnityEngine;

namespace ETools
{
    public class EToolsLevelUtils : EditorWindow
    {
        private float minScale = 0.9f;
        private float maxScale = 1.1f;
        private bool scaleX = true;
        private bool scaleY = true;
        private bool scaleZ = true;

        [MenuItem("Escripts/Level Utilities")]
        public static void ShowWindow()
        {
            GetWindow<EToolsLevelUtils>("ETools Level Utilities");
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label("Random Rotator", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Rotate X"))
            {
                RotateSelectedObjects(Vector3.right);
            }

            if (GUILayout.Button("Rotate Y"))
            {
                RotateSelectedObjects(Vector3.up);
            }

            if (GUILayout.Button("Rotate Z"))
            {
                RotateSelectedObjects(Vector3.forward);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.BeginVertical("box");
            GUILayout.Label("Random Scaler", EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Min Scale: " + minScale.ToString("F2") + ", Max Scale: " + maxScale.ToString("F2"));

            EditorGUILayout.MinMaxSlider(ref minScale, ref maxScale, 0.1f, 10.0f);

            GUILayout.BeginHorizontal();
            scaleX = GUILayout.Toggle(scaleX, "Scale X");
            scaleY = GUILayout.Toggle(scaleY, "Scale Y");
            scaleZ = GUILayout.Toggle(scaleZ, "Scale Z");
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Randomize Scale"))
            {
                RandomizeSelectedObjectsScale();
            }
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.BeginVertical("box");
            GUILayout.Label("Lighting Data Management", EditorStyles.boldLabel);
            if (GUILayout.Button("Clear GI Cache and Baked Data"))
            {
                ClearLightingData();
            }
            GUILayout.EndVertical();
        }

        private void RotateSelectedObjects(Vector3 axis)
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj.transform, "Random Rotate");
                float randomAngle = Random.Range(0f, 360f);
                obj.transform.Rotate(axis, randomAngle, Space.Self);
            }
        }

        private void RandomizeSelectedObjectsScale()
        {

            foreach (GameObject obj in Selection.gameObjects)
            {
                float randomScaleFactor = Random.Range(minScale, maxScale);

                Undo.RecordObject(obj.transform, "Random Scale");

                Vector3 randomScale = new Vector3(
                    scaleX ? randomScaleFactor : obj.transform.localScale.x,
                    scaleY ? randomScaleFactor : obj.transform.localScale.y,
                    scaleZ ? randomScaleFactor : obj.transform.localScale.z
                );

                obj.transform.localScale = randomScale;
            }
        }

        private static void ClearLightingData()
        {
            if (Lightmapping.lightingDataAsset != null)
            {
                Lightmapping.Clear();
                Lightmapping.ClearDiskCache();
                Debug.Log("Cleared GI Cache and Baked Data");
            }
            else
            {
                Debug.Log("No lighting data found to clear.");
            }
        }
    }
}


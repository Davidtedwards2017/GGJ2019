using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _DataPersistance
{
    [CustomEditor(typeof(ProgressionController))]
    public class ProgressionControllerInEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (ProgressionController)target;
            if (GUILayout.Button("Save"))
            {
                script.SaveProgress();
            }

            if (GUILayout.Button("Load"))
            {
                script.LoadProgress();
            }

            if (GUILayout.Button("Clear"))
            {
                script.Clear();
            }
        }
    }
}
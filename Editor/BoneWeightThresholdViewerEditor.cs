using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RainyReignGames
{
    [CustomEditor(typeof(BoneWeightThresholdViewer))]
    public class BoneWeightThresholdViewerEditor : Editor
    {
        public override void OnInspectorGUI()
        {           
            if(SceneView.lastActiveSceneView != null && SceneView.lastActiveSceneView.drawGizmos == false)
            {
                EditorGUILayout.HelpBox("Gizmos are not enabled in SceneView.  You will be unable to see the vertex gizmos!", MessageType.Warning);
            }

            base.OnInspectorGUI();
        }
    }
}

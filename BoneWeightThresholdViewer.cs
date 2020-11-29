///
/// Created by Kenamis
///

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RainyReignGames
{
    public class BoneWeightThresholdViewer : MonoBehaviour
    {
        [Range(0.001f, 0.01f), Tooltip("Size of the Gizmos cubes displayed for each vertex.")]
        public float size = 0.01f;
        [Range(0, 0.999f), Tooltip("Bone Weight Threshold")]
        public float threshold;
        public SkinnedMeshRenderer skinnedMeshRenderer;
        public Transform boneToSplit;

        [SerializeField, HideInInspector]
        private SkinnedMeshRenderer previousRenderer;
        [SerializeField, HideInInspector]
        private Transform previousBone;

        [SerializeField, HideInInspector]
        private Vector3[] vertices;
        [SerializeField, HideInInspector]
        private BoneWeight[] boneWeights;
        [SerializeField, HideInInspector]
        private bool[] boneMask;

        private Vector3 gizmoSize;

        private void OnValidate()
        {
            gizmoSize = new Vector3(size, size, size);

            if (skinnedMeshRenderer == null)
            {
                skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            }

            if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
            {
                if (skinnedMeshRenderer != previousRenderer)
                {
                    UpdateMesh();
                }

                if(boneToSplit != previousBone)
                {
                    UpdateBoneData();
                }
            }
        }

        void UpdateMesh()
        {
            Mesh bakedMesh = new Mesh();
            skinnedMeshRenderer.BakeMesh(bakedMesh);

            vertices = bakedMesh.vertices;
            boneWeights = skinnedMeshRenderer.sharedMesh.boneWeights;

            previousRenderer = skinnedMeshRenderer;
        }

        void UpdateBoneData()
        {
            List<Transform> bonesToSplit = new List<Transform>();
            if (boneToSplit != null)
            {
                boneToSplit.GetComponentsInChildren<Transform>(bonesToSplit);
            }

            Transform[] bones = skinnedMeshRenderer.bones;
            boneMask = new bool[bones.Length];
            for (int i = 0; i < bones.Length; i++)
            {
                if (bonesToSplit.Contains(bones[i]))
                {
                    boneMask[i] = true;
                }
            }
            
            previousBone = boneToSplit;
        }

        void OnDrawGizmos()
        {
            if(vertices != null)
            {
                if (boneToSplit != null)
                {
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        if (IsPartOf(boneWeights[i], boneMask, threshold))
                        {
                            Gizmos.color = Color.red;
                        }
                        else
                        {
                            Gizmos.color = Color.black;
                        }

                        Gizmos.DrawCube(transform.TransformPoint(vertices[i]), gizmoSize);
                    }
                }
                else
                {
                    Gizmos.color = Color.black;
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        Gizmos.DrawCube(transform.TransformPoint(vertices[i]), gizmoSize);
                    }
                }
            }
        }

        private static bool IsPartOf(BoneWeight b, bool[] mask, float threshold)
        {
            float weight = 0;

            if (mask[b.boneIndex0]) weight += b.weight0;
            if (mask[b.boneIndex1]) weight += b.weight1;
            if (mask[b.boneIndex2]) weight += b.weight2;
            if (mask[b.boneIndex3]) weight += b.weight3;

            return (weight > threshold);
        }
    }
}
#endif
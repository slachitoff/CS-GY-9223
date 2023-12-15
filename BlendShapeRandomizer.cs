using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlendShapeGroup
{
    public int startIndex;
    public int endIndex;
    public float groupMean;
    public float groupStandardDeviation;
}

public class BlendShapeRandomizer : MonoBehaviour
{
    [SerializeField] private List<BlendShapeGroup> blendShapeGroups = new List<BlendShapeGroup>();

    void Start()
    {
        RandomizeBlendShapes();
        ScaleColliders();
    }

    void RandomizeBlendShapes()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        if (skinnedMeshRenderer != null)
        {
            foreach (BlendShapeGroup group in blendShapeGroups)
            {
                for (int i = group.startIndex; i <= group.endIndex; i++)
                {
                    float randomBlendShapeValue = GetRandomNormalDistribution(group.groupMean, group.groupStandardDeviation);
                    skinnedMeshRenderer.SetBlendShapeWeight(i, randomBlendShapeValue);
                }
            }
        }
    }

    void ScaleColliders()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        if (skinnedMeshRenderer != null)
        {
            int blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;

            Colliding[] collidersWithColl = skinnedMeshRenderer.GetComponentsInChildren<Colliding>();

            foreach (Colliding colliderWithColl in collidersWithColl)
            {
                int adjustedIndex = colliderWithColl.index - 1;

                if (adjustedIndex >= 0 && adjustedIndex < blendShapeCount)
                {
                    float blendShapeValue = skinnedMeshRenderer.GetBlendShapeWeight(adjustedIndex);

                    float colliderScale = 1.0f - (blendShapeValue / 100.0f);

                    Vector3 colliderSize = colliderWithColl.boxCollider.size;
                    colliderSize.x *= colliderScale; 
                    colliderSize.y *= colliderScale; 
                    colliderWithColl.SetColliderSize(colliderSize);
                }
            }
        }
    }

    float GetRandomNormalDistribution(float mean, float standardDeviation)
    {
        float u1 = 1.0f - UnityEngine.Random.Range(0f, 1f);
        float u2 = 1.0f - UnityEngine.Random.Range(0f, 1f);
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + standardDeviation * randStdNormal;
    }
}

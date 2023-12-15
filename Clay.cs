/* Importantly, the Clay component in its entirety has been wholly adapted from the Wood script found here: 
https://github.com/herbou/Unity_Pottery/blob/master/Assets/Scripts/Wood.cs, 
 where the original functionality of the Wood script found in the linked Github repository has been divided between the 
 Clay component shown below, and a separate Revolve component. Apart from changing some variable names, I had no contribution to the development of this component.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clay : MonoBehaviour
{
    public Revolve revolve;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    public void Collision(int keyIndex, float forceValue)
    {
        float colliderHeight = 0.02f;

        if (revolve != null && revolve.isRotating)
        {
            float newWeight = skinnedMeshRenderer.GetBlendShapeWeight(keyIndex) + forceValue * (100f / colliderHeight);
            skinnedMeshRenderer.SetBlendShapeWeight(keyIndex, newWeight);
        }
    }
}

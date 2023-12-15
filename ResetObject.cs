using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour
{
    public Transform[] objectsToReset;
    private Vector3[] originalPositions;

    private void Start()
    {
        if (objectsToReset != null)
        {
            originalPositions = new Vector3[objectsToReset.Length];

            for (int i = 0; i < objectsToReset.Length; i++)
            {
                if (objectsToReset[i] != null)
                {
                    originalPositions[i] = objectsToReset[i].position;
                }
            }
        }
    }

    public void ResetObjects()
    {
        if (originalPositions != null)
        {
            for (int i = 0; i < objectsToReset.Length; i++)
            {
                if (objectsToReset[i] != null)
                {
                    Rigidbody rb = objectsToReset[i].GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                    objectsToReset[i].position = originalPositions[i];
                    rb.isKinematic = false;
                }
            }
        }
    }
}
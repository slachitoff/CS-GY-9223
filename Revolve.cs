/* The Revolve component, although heavily modified, was adapted from the Wood script found here: 
https://github.com/herbou/Unity_Pottery/blob/master/Assets/Scripts/Wood.cs, 
 where the original functionality of the Wood script found in the linked Github repository has been divided 
 between the Revolve component shown below, and a separate Clay component. My original contributions to this script amount to 
toggling an on/off condition, and incorporating an acceleration and deceleration period, which turned out to be a non-trivial
challenge.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RevolvingChild
{
    public Transform childTransform;
    public Vector3 rotationVector;
}

public class Revolve : MonoBehaviour
{
    [SerializeField] private List<RevolvingChild> revolvingChildren;
    [SerializeField] private float maxRotationDuration;
    [SerializeField] private float accelerationDuration;
    [SerializeField] private float decelerationDuration;

    public bool isRotating = false;
    private Coroutine rotationCoroutine;

    public void ToggleRotation()
    {
        if (isRotating)
        {
            StopRotation();
        }
        else
        {
            StartRotation();
        }
    }

    private void StartRotation()
    {
        isRotating = true;
        rotationCoroutine = StartCoroutine(RotateChildren());
    }

    private IEnumerator RotateChildren()
    {
        float timer = 0f;
        float currentRotationSpeed = 0f;

        while (isRotating)
        {
            if (timer < accelerationDuration)
            {
                currentRotationSpeed = Mathf.Lerp(0f, 1f, timer / accelerationDuration);
            }
            else
            {
                currentRotationSpeed = 1f;
            }

            foreach (var child in revolvingChildren)
            {
                child.childTransform.Rotate(child.rotationVector * Time.deltaTime * currentRotationSpeed, Space.Self);
            }

            yield return null;
            timer += Time.deltaTime;
        }
    }

    private void StopRotation()
    {
        isRotating = false;
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = StartCoroutine(DecelerateRotation());
        }
    }

    private IEnumerator DecelerateRotation()
    {
        float timer = 0f;

        while (timer < decelerationDuration)
        {
            float decelerationFactor = Mathf.Lerp(1f, 0f, timer / decelerationDuration);

            foreach (var child in revolvingChildren)
            {
                child.childTransform.Rotate(child.rotationVector * Time.deltaTime * decelerationFactor, Space.Self);
            }

            yield return null;
            timer += Time.deltaTime;
        }
    }
}
 
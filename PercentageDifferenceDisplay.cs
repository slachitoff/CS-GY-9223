using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PercentageDifferenceDisplay : MonoBehaviour
{
    [SerializeField] private GameObject object1;
    [SerializeField] private GameObject object2;
    public TextMeshProUGUI percentageText;
    private Coroutine fadeCoroutine;

    public void CalculateAndDisplayPercentage()
    {
        if (object1 != null && object2 != null)
        {
            float percentageDifference = CalculateBoxColliderVolumeDifference(object1, object2);

            if (percentageText != null)
            {

                int roundedPercentage = Mathf.RoundToInt(percentageDifference);

                percentageText.text = $"Volume Difference: \n{roundedPercentage}%";

                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }

                fadeCoroutine = StartCoroutine(FadeInOutCoroutine());
            }

        }

    }

    private float CalculateBoxColliderVolumeDifference(GameObject obj1, GameObject obj2)
    {
        BoxCollider[] colliders1 = obj1.GetComponentsInChildren<BoxCollider>();
        BoxCollider[] colliders2 = obj2.GetComponentsInChildren<BoxCollider>();

        float initialVolume = CalculateTotalVolume(colliders1);
        float newVolume = CalculateTotalVolume(colliders2);

        float volumeDifference = initialVolume - newVolume;

        float percentageDifference = (volumeDifference / initialVolume) * 100.0f;

        percentageDifference = Mathf.Clamp(percentageDifference, -100.0f, 100.0f);

        return percentageDifference;
    }

    private float CalculateTotalVolume(BoxCollider[] colliders)
    {
        float totalVolume = 0.0f;

        foreach (var collider in colliders)
        {
            Vector3 colliderSize = collider.size;
            float colliderVolume = colliderSize.x * colliderSize.y * colliderSize.z;
            totalVolume += colliderVolume;
        }

        return totalVolume * 10000;
    }

    private System.Collections.IEnumerator FadeInOutCoroutine()
    {
        float fadeDuration = 1f; 
        float displayDuration = 5f; 

        Color startColor = percentageText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            percentageText.color = Color.Lerp(endColor, startColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        percentageText.color = startColor;

        yield return new WaitForSeconds(displayDuration);

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            percentageText.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        percentageText.color = endColor;

        percentageText.text = "";
        percentageText.color = startColor; 
    }
}

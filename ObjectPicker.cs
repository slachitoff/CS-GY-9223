/* Although a great deal of the script has been modified to fulfill a variety of functions, 
the script utilizes some of the logic found in the Gaze Timer Interaction tutorial shown here:
https://www.youtube.com/watch?v=zdNBZsJdg9c. I did not used the GVR SDK, 
so the script had to be adjusted accordingly. My original contributions include the interactions 
with game objects to pick up objects, drop objects, reset objects, 
activate the rotation of the pottery wheel, and display the comparison "Score"*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPicker : MonoBehaviour
{
    private GameObject heldObject;
    private Transform originalParent;
    private Vector3 originalObjectPosition;
    private bool isHoldingObject;
    public float raycastDistance = 5f;
    public float desiredDistance = 2f;
    public Image countDownIndicator;
    public float totalTime = 2.0f;
    private float countDownTimer;

    private bool isCooldownActive = false;
    public float cooldownTime = 2.0f;
    private float cooldownTimer;

    private string targetObjectTag = "Drop Off";

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        bool hitInteractable = false;

        if (!isHoldingObject && !isCooldownActive)
        {
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                GameObject hoveredObject = hit.collider.gameObject;
                HeldObject heldObjectScript = hoveredObject.GetComponent<HeldObject>();
                PlaySounds soundPlayer = hoveredObject.GetComponent<PlaySounds>();
                Revolve revolveScript = hoveredObject.GetComponent<Revolve>();
                PercentageDifferenceDisplay percentageDisplay = hoveredObject.GetComponent<PercentageDifferenceDisplay>();
                ResetObject resetObjectScript = hoveredObject.GetComponent<ResetObject>();

                if (heldObjectScript != null || soundPlayer != null || revolveScript != null || percentageDisplay != null || resetObjectScript != null)
                {
                    hitInteractable = true;

                    countDownTimer += Time.deltaTime;
                    countDownIndicator.fillAmount = countDownTimer / totalTime;

                    if (countDownTimer > totalTime)
                    {
                        if (heldObjectScript != null)
                            PickUpObject(heldObjectScript);
                        if (soundPlayer != null)
                        {
                            soundPlayer.ClickSound();
                            soundPlayer.ToggleLoopSound();
                        }
                        if (revolveScript != null)
                            revolveScript.ToggleRotation();
                        if (percentageDisplay != null)
                            percentageDisplay.CalculateAndDisplayPercentage();
                        if (resetObjectScript != null)
                            resetObjectScript.ResetObjects();

                        isCooldownActive = true;
                        cooldownTimer = 0;

                        countDownTimer = 0;
                        countDownIndicator.fillAmount = 0;
                    }
                }
            }
        }
        else if (heldObject && !isCooldownActive)
        {
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                GameObject hoveredObject = hit.collider.gameObject;

                if (hoveredObject.CompareTag(targetObjectTag))
                {
                    hitInteractable = true;
                    countDownTimer += Time.deltaTime;
                    countDownIndicator.fillAmount = countDownTimer / totalTime;

                    if (countDownTimer > totalTime)
                    {
                        ReleaseObject();
                        isCooldownActive = true;
                        cooldownTimer = 0;

                        countDownTimer = 0;
                        countDownIndicator.fillAmount = 0;
                    }
                }
            }
        }

        if (!hitInteractable)
        {
            countDownTimer = 0;
            countDownIndicator.fillAmount = 0;
        }


        if (isCooldownActive)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= cooldownTime)
            {
                isCooldownActive = false;
                cooldownTimer = 0; 
            }
        }

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            Vector3 directionToHitPoint = hit.point - transform.position;
            Vector3 desiredPosition = transform.position + directionToHitPoint.normalized * desiredDistance;

            if (heldObject != null)
            {
                heldObject.transform.position = desiredPosition;

                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
    }

    private void PickUpObject(HeldObject objToPickUp)
    {
        if (objToPickUp == null)
        {
            return;
        }

        heldObject = objToPickUp.gameObject;
        originalObjectPosition = heldObject.transform.position;
        isHoldingObject = true;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        objToPickUp.SetIsHeld(true);

        originalParent = heldObject.transform.parent;
        heldObject.transform.parent = transform;
        heldObject.transform.localPosition = Vector3.zero;
    }

    private void ReleaseObject()
    {
        if (isHoldingObject)
        {
            if (heldObject != null)
            {
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = true;

                    rb.constraints = RigidbodyConstraints.None;
                }

                Collider coll = heldObject.GetComponent<Collider>();
                if (coll != null)
                {
                    coll.material = null;  
                }

                HeldObject heldObjectComponent = heldObject.GetComponent<HeldObject>();
                if (heldObjectComponent != null)
                {
                    heldObjectComponent.SetIsHeld(false);
                }

                heldObject.transform.parent = originalParent;
                heldObject = null;
                isHoldingObject = false;
            }
        }
    }
}

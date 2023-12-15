/*Although modified to incorporate sound cues, set object orientation upon pickup, 
and have the position of the held object track the main camera movement,
the basis for the component HeldObject was adapted from a Unity script found here:
https://github.com/herbou/Unity_Pottery/blob/master/Assets/Scripts/Knife.cs */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObject : MonoBehaviour
{
    [SerializeField] private float forceValue;
    [SerializeField] private Clay clay;
    [SerializeField] private AudioClip dropSound; 
    private AudioSource audioSource; 

    private Rigidbody objectRigidBody;
    private Vector3 movementVector;
    public Quaternion desiredRotation = Quaternion.Euler(0f, 0f, 0f);
    private bool isHeld = false; 
    private Camera mainCamera;

    private void Start()
    {
        objectRigidBody = GetComponent<Rigidbody>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {

        if (isHeld)
        {
            Vector3 forward = mainCamera.transform.forward;
            movementVector = forward.normalized * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {    

        if (isHeld)
        {
            objectRigidBody.MovePosition(objectRigidBody.position + movementVector);
            transform.rotation = desiredRotation;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Colliding coll = collision.collider.GetComponent<Colliding>();
        if (coll != null)
        {
            coll.HitCollider(forceValue);
            clay.Collision(coll.index, forceValue);
        }
    }

    public void SetIsHeld(bool held, float volume = .2f)
    {
        isHeld = held;

        if (!isHeld && dropSound != null)
        {
            audioSource.volume = volume; 
            audioSource.PlayOneShot(dropSound);
        }
    }
}

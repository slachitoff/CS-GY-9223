/* The Colliding component has been adapted from the Coll script found here: 
https://github.com/herbou/Unity_Pottery/blob/master/Assets/Scripts/Coll.cs.
My original contributions include incorporating sound cues when collisions occur, enabling/disabling collisions based
on the condition of the Revolve component, and functionality with the "BlendShapeRandomizer" component 
 */

using UnityEngine;

public class Colliding : MonoBehaviour
{
    public int index;
    public AudioClip collisionSound;
    private AudioSource audioSource;
    public Revolve revolve;  

    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;

    public BoxCollider boxCollider { get; private set; }

    private bool isColliding = false;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = collisionSound;
        audioSource.volume = volume;
        audioSource.loop = true;  
        index = transform.GetSiblingIndex();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (revolve != null && revolve.isRotating)
        {
            if (!isColliding)
            {
                isColliding = true;
                audioSource.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isColliding)
        {
            isColliding = false;
            audioSource.Stop();
        }
    }

    public void HitCollider(float damage)
    {
        if (isColliding)
        {
            if (boxCollider.size.y - damage > 0.0f)
            {
                SetColliderSize(new Vector3(boxCollider.size.x - damage, boxCollider.size.y - damage, boxCollider.size.z));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetColliderSize(Vector3 newSize)
    {
        boxCollider.size = newSize;
    }
}

using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private Rigidbody spinnerRigidbody;
    [SerializeField] private bool anticlockwise;
    [SerializeField] private float speed = 1;
    
    public void FixedUpdate()
    {
        if (anticlockwise) 
            spinnerRigidbody.angularVelocity = new Vector3(0, -speed, 0);
        else
            spinnerRigidbody.angularVelocity = new Vector3(0, speed, 0);
    }
}

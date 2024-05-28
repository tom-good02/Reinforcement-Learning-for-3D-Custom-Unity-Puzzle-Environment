using System;
using UnityEngine;

public class VideoCameraController : MonoBehaviour
{
    // Class purely used to film the submission video
    
    [SerializeField] private Transform startingPosition;
    [SerializeField] private Transform endingPosition;
    [SerializeField] private Transform mainCamera;
    private bool m_MoveCamera;

    public void Awake()
    {
        mainCamera.position = startingPosition.position;
        Time.timeScale = 0;
    }

    public void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     m_MoveCamera = true;
        // }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Time.timeScale = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale = 1;
        }
        
        var movementSpeed = 12f;
        
        if (m_MoveCamera)
        {
            var targetPosition = endingPosition.position;
            var currentPosition = transform.position;
            var direction = (targetPosition - currentPosition).normalized;

            // Move the camera at a constant speed towards the target position
            transform.position += direction * (movementSpeed * Time.deltaTime);

            var distance = Vector3.Distance(currentPosition, targetPosition);

            if (distance < 0.1f)
            {
                m_MoveCamera = false;
                transform.position = targetPosition;
            }
        }
    }
}

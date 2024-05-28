using UnityEngine;

public class Door : MonoBehaviour
{
    private bool m_Open;
    private Vector3 m_StartPosition;

    public void Awake()
    {
        m_StartPosition = transform.position;
    }

    public void OpenDoor()
    {
        m_Open = true;
    }
    
    public void CloseDoor()
    {
        m_Open = false;
    }

    public void Update()
    {
        if (m_Open && transform.position.y < m_StartPosition.y + 7f)
        {
            // Raise the door
            transform.Translate(Vector3.up * (5f * Time.deltaTime));
        }
        else if (!m_Open && transform.position.y > m_StartPosition.y)
        {
            // Lower the door
            transform.Translate(Vector3.down * (3f * Time.deltaTime));
        }
    }
    
    public void ResetDoor()
    {
        m_Open = false;
        transform.position = m_StartPosition;
    }
}

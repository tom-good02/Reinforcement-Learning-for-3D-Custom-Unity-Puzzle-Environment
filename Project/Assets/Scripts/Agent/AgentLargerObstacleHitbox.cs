using UnityEngine;

public class AgentLargerObstacleHitbox : MonoBehaviour
{
    [SerializeField] private EscapeAgent escapeAgent;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            escapeAgent.AddReward(-0.1f, "Obstacle hit with larger hitbox");
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            escapeAgent.AddReward(-0.0005f, "OnStay Obstacle hit with larger hitbox");
        }
    }
}

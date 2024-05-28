using UnityEngine;

public class AgentLargerWallAndFloorHitbox : MonoBehaviour
{
    [SerializeField] private EscapeAgent escapeAgent;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Floor"))
        {
            escapeAgent.AddReward(-0.1f, "Wall or Floor hit with larger hitbox");
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Floor"))
        {
            escapeAgent.AddReward(-0.001f, "OnStay Wall or Floor hit with larger hitbox");
        }
    }
}

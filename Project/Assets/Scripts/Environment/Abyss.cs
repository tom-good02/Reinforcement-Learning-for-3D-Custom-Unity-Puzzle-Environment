using UnityEngine;

public class Abyss : MonoBehaviour
{
    [SerializeField] private EnvironmentManager environmentManager;    
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            environmentManager.Abyss();
        }
    }
}

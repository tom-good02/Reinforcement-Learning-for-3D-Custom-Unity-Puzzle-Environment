using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool m_Pressed;
    private const float SinkAmount = 0.2f;
    [SerializeField] private RoomController roomController;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material unpressedMaterial;
    [SerializeField] private Material pressedMaterial;
    [SerializeField] private BoxCollider largerHitBox;
    [SerializeField] private BoxCollider plateHitBox;
    [SerializeField] private Transform pressurePlate;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent") && !m_Pressed)
        {
            m_Pressed = true;
            meshRenderer.material = pressedMaterial;
            
            largerHitBox.enabled = false;
            plateHitBox.enabled = false;
            roomController.PlatePressed();
        }
    }
    
    public void FixedUpdate()
    {
        if (m_Pressed && pressurePlate.localPosition.y > plateHitBox.center.y - SinkAmount)
        {
            // Sink the plate down by sinkAmount on the y-axis
            pressurePlate.Translate(Vector3.down * (2f * Time.fixedDeltaTime));
        }
    }
    
    public void ResetPlate()
    {
        m_Pressed = false;
        meshRenderer.material = unpressedMaterial;
        
        largerHitBox.enabled = true;
        plateHitBox.enabled = true;

        pressurePlate.localPosition = plateHitBox.center;
    }
}

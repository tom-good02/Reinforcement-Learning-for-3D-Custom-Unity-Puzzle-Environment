using UnityEngine;

public class DoorPressurePlate : MonoBehaviour
{
    [SerializeField] private RoomController roomController;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Agent"))
        {
            roomController.DoorPlatePressed();
        }
    }
}

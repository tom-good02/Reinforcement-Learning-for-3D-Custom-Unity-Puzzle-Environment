using UnityEngine;

public class Room1Controller : RoomController
{
    [SerializeField] private bool randomPressurePlateSpawnPoint;
    [SerializeField] private GameObject pressurePlate;
    [SerializeField] private GameObject smallPressurePlatePillar;
    
    public override void ResetRoom()
    {
        base.ResetRoom();
        if (randomPressurePlateSpawnPoint)
        {
            var pressurePlatePosition = pressurePlate.transform.position;
            var smallPressurePlatePillarPosition = smallPressurePlatePillar.transform.position;

            if (Random.Range(0, 2) == 1)
            {
                pressurePlate.transform.position = new Vector3(Random.Range(15f, 30f), pressurePlatePosition.y, Random.Range(15f, 35f));
                smallPressurePlatePillar.transform.position = new Vector3(Random.Range(40f, 55f), smallPressurePlatePillarPosition.y, Random.Range(15f, 35f));
            }
            else
            {
                pressurePlate.transform.position = new Vector3(Random.Range(40f, 55f), pressurePlatePosition.y, Random.Range(15f, 35f));
                smallPressurePlatePillar.transform.position = new Vector3(Random.Range(15f, 30f), smallPressurePlatePillarPosition.y, Random.Range(15f, 35f));
            }
        }
    }
}

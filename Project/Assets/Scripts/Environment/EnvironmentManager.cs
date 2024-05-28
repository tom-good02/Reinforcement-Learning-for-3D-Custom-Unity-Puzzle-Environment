using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private List<RoomController> roomControllers;
    [SerializeField] private bool mainRoom;
    public bool MainRoom
    {
        get => mainRoom;
        set => mainRoom = value;
    }
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private EscapeAgent escapeAgent;
    public EscapeAgent EscapeAgent
    {
        get => escapeAgent;
        set => escapeAgent = value;
    }
    
    private bool m_MoveCamera;
    private int m_CurrentRoomNumber;

    public void OnValidate()
    {
        EscapeAgent = escapeAgent;
    }

    public void Awake()
    {
        if (mainRoom)
        {
            mainCamera.transform.position = roomControllers[m_CurrentRoomNumber].CameraPosition.position;
            mainCamera.transform.rotation = roomControllers[m_CurrentRoomNumber].CameraPosition.rotation;   
        }
        roomControllers[m_CurrentRoomNumber].ActiveRoom = true;
        roomControllers[m_CurrentRoomNumber].DoorPlate.SetActive(true);
    }
    
    private void NextRoom()
    {
        // Get possible new room number from Academy
        // var academyRoomNumber = (int)Academy.Instance.EnvironmentParameters.GetWithDefault("room_number", 0);
        if (roomControllers[m_CurrentRoomNumber].temp == false)
            return;
        
        var academyRoomNumber = m_CurrentRoomNumber + 1;
        
        if (academyRoomNumber >= roomControllers.Count)
            academyRoomNumber = roomControllers.Count - 1;
        if (m_CurrentRoomNumber != academyRoomNumber)
        {
            roomControllers[m_CurrentRoomNumber].ActiveRoom = false;
            roomControllers[m_CurrentRoomNumber].DoorPlate.SetActive(false);
            m_CurrentRoomNumber = academyRoomNumber;
            roomControllers[m_CurrentRoomNumber].DoorPlate.SetActive(true);
            roomControllers[m_CurrentRoomNumber].ActiveRoom = true;
            if (mainRoom)
                m_MoveCamera = true;
        }
    }
    
    public Vector3 GetSpawnPointPosition()
    {
        var position = roomControllers[m_CurrentRoomNumber].SpawnPoint.position;
        position.x += Random.Range(-5f, 5f);
        position.z += Random.Range(-12.5f, 12.5f);
        
        return position;
    }
    
    public Quaternion GetSpawnPointRotation()
    {
        // Random rotation
        var rotation = roomControllers[m_CurrentRoomNumber].SpawnPoint.rotation.eulerAngles;
        rotation.y = Random.Range(0f, 360f);
        
        return Quaternion.Euler(rotation);
    }
    
    private Transform GetCameraPosition()
    {
        return roomControllers[m_CurrentRoomNumber].CameraPosition;
    }
    
    public void Update()
    {
        var movementSpeed = 2.0f;

        if (m_MoveCamera)
        {
            var targetPosition = GetCameraPosition().position;
            var transform1 = mainCamera.transform;
            var position = transform1.position;
            var direction = targetPosition - position;

            position += direction * (movementSpeed * Time.deltaTime);
            transform1.position = position;

            var distance = Vector3.Distance(position, targetPosition);
            
            if (distance < 0.1f)
            {
                m_MoveCamera = false;
                mainCamera.transform.position = targetPosition;
            }
        }
    }
    
    public void ResetCurrentRoom()
    {
        // Reset the current room the agent is in
        roomControllers[m_CurrentRoomNumber].ResetRoom();
        // Check if we need to go to the next room
        NextRoom();
    }
    
    public void Abyss()
    {
        escapeAgent.AddReward(-0.5f, "Fell into the abyss");
        escapeAgent.EndEpisode();
    }
}

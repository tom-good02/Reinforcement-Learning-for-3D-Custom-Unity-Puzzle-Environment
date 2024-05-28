using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Tooltip("Time limit to complete the room in physics steps")]
    [SerializeField] private int timeLimit;

    private int TimeElapsedThisAttempt
    {
        set;
        get;
    }
    
    [SerializeField] private Door door;
    [SerializeField] private GameObject doorPlate;
    public GameObject DoorPlate
    {
        private set => doorPlate = value;
        get => doorPlate;
    }
    [SerializeField] private List<PressurePlate> pressurePlates;
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] private List<Light> lights;
    [SerializeField] private Color defaultLightColour = new(0.9528302f, 0.8737386f, 0.6876557f, 1f);
    [SerializeField] private Color greenLightColour = new(0f, 1f, 0f, 1f);
    [SerializeField] private Transform spawnPoint;
    public Transform SpawnPoint
    {
        private set => spawnPoint = value;
        get => spawnPoint;
    }
    
    public bool ActiveRoom { get; set; }
    
    [SerializeField] private Transform cameraPosition;
    public Transform CameraPosition
    {
        private set => cameraPosition = value;
        get => cameraPosition;
    }

    private int m_NumberOfPressurePlatesPressed;
    
    private void OnValidate()
    {
        CameraPosition = cameraPosition;
        SpawnPoint = spawnPoint;
    }

    public void FixedUpdate()
    {
        if (!ActiveRoom)
            return;
        
        TimeElapsedThisAttempt++;
        
        if (timeLimit == 0)
            return;
        
        if (TimeElapsedThisAttempt >= timeLimit)
        {
            environmentManager.EscapeAgent.EndEpisode();
        }
    }

    public void PlatePressed()
    {
        m_NumberOfPressurePlatesPressed++;
        // Every second that has passed remove 0.005 reward

        var temp = Mathf.Floor(TimeElapsedThisAttempt / 50f);
        var reward = 1f - (0.005f * temp);
        // if (environmentManager.MainRoom)
        //     print(reward);
            
        environmentManager.EscapeAgent.AddReward(reward, "Pressure Plate Pressed");
        if (m_NumberOfPressurePlatesPressed == pressurePlates.Count)
        {
            door.GetComponent<Door>().OpenDoor();
            foreach (var spotlight in lights)
            {
                spotlight.color = greenLightColour;
            }
        }
    }

    public bool temp;
    
    public void DoorPlatePressed()
    {
        temp = true;
        environmentManager.EscapeAgent.AddReward(2f, "Door Plate Pressed");
        environmentManager.EscapeAgent.EndEpisode();
    }

    public virtual void ResetRoom()
    {
        m_NumberOfPressurePlatesPressed = 0;
        TimeElapsedThisAttempt = 0;
        door.CloseDoor();
        
        foreach (var pressurePlate in pressurePlates)
        {
            pressurePlate.ResetPlate();
        }
        
        foreach (var spotlight in lights)
        {
            spotlight.color = defaultLightColour;
        }
    }
}

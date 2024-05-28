using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EscapeAgent : Agent
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] private Rigidbody agentRigidbody;
    [SerializeField] private BoxCollider agentCollider;
    private const float MoveSpeed = 8f;
    private const float RotationSpeed = 3f;
    private const float JumpForce = 11f;
    private bool m_Grounded;
    private int m_StuckTimer;
    private int m_groundedTimer;
    
    public override void OnEpisodeBegin()
    {
        // Called when the environment is reset by Academy
        agentRigidbody.velocity = Vector3.zero;
        agentRigidbody.angularVelocity = Vector3.zero;

        m_StuckTimer = 0;
        m_Grounded = false;
        
        environmentManager.ResetCurrentRoom();

        transform.position = environmentManager.GetSpawnPointPosition();
        transform.rotation = environmentManager.GetSpawnPointRotation();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get the discrete actions and move the agent accordingly
        var discreteActions = actions.DiscreteActions;
        MoveAgent(discreteActions);
    }

    private void MoveAgent(ActionSegment<int> actionSegment)
    {
        // branch 1: forward/backward
        var forwardAxis = actionSegment[0];
        // branch 2: rotate left/right
        var rotateAxis = actionSegment[1];
        // branch 3: jump
        var jumpAxis = actionSegment[2];
        
        if (m_Grounded)
        {
            // Calculate the move direction
            switch (forwardAxis)
            {
                case 0:
                    agentRigidbody.velocity = Vector3.zero;
                    break;
                case 1:
                    agentRigidbody.velocity = transform.forward * MoveSpeed;
                    break;
                case 2:
                    agentRigidbody.velocity = -transform.forward * MoveSpeed;
                    break;
            }

            // Calculate the rotation
            switch (rotateAxis)
            {
                case 0:
                    agentRigidbody.angularVelocity = Vector3.zero;
                    break;
                case 1:
                    agentRigidbody.angularVelocity = new Vector3(0, -RotationSpeed, 0);
                    break;
                case 2:
                    agentRigidbody.angularVelocity = new Vector3(0, RotationSpeed, 0);
                    break;
            }
        }

        // Jump
        if (jumpAxis == 1 && m_Grounded)
        {
            AddReward(-0.02f, "Jumped");
            agentRigidbody.velocity = new Vector3(agentRigidbody.velocity.x, JumpForce, agentRigidbody.velocity.z);
        }
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Used to test the agent with keyboard input
        var discreteActionsOut = actionsOut.DiscreteActions;

        // Forwards and backwards (W and S)
        discreteActionsOut[0] = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? 2 : 0;
        // Rotate left and right (A and D)
        discreteActionsOut[1] = Input.GetKey(KeyCode.A) ? 1 : Input.GetKey(KeyCode.D) ? 2 : 0;
        // Jump (Space)
        discreteActionsOut[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void Start()
    {
        agentCollider = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        // Check if the agent is grounded
        var wasGrounded = m_Grounded;
        m_Grounded = IsGrounded();
        
        // Stops the agent from tipping themselves over when landing from a jump
        if (!wasGrounded && m_Grounded)
        {
            if (agentRigidbody.velocity.magnitude > 0.1f || agentRigidbody.angularVelocity.magnitude > 0.1f)
            {
                m_Grounded = false;
            }
        }
        
        // If the agent is grounded, set the x and z rotation to 0
        if (m_Grounded)
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
        // Apply gravity if the agent is not grounded
        agentRigidbody.useGravity = !m_Grounded;
    }

    private bool IsGrounded()
    {
        var correctWayUp = Vector3.Dot(transform.up, Vector3.up) > 0.99f;
        
        var point = transform.TransformPoint(agentCollider.center);
        var isGrounded = Physics.Raycast(point, Vector3.down, out var hitInfo, (agentCollider.size.y * 0.5f) + 0.001f, groundLayer);
        
        // Draw ray in debug mode
        // Debug.DrawRay(point, new Vector3(0, -1.501f, 0), Color.red);
        
        // if the agent is not grounded, the velocity is zero, the angular velocity is zero and these conditions have been happening for two seconds, end the episode
        if ((!isGrounded || !correctWayUp) && 
            agentRigidbody.velocity.magnitude < 0.01f && 
            agentRigidbody.angularVelocity.magnitude < 0.01f)
        {
            m_StuckTimer++;
            if (m_StuckTimer >= 50)
            {
                EndEpisode();
            }
        }
        else
        {
            m_StuckTimer = 0;
        }
        
        return isGrounded && correctWayUp;
    }
}

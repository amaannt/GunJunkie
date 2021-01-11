using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    public float agentSpeed = 2.0f;
    public GameObject player;
    public override void OnEpisodeBegin()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        Debug.Log(gameObject.name + " following player");
    }
    float stepCount = 0.0f;
    bool isFollowingPlayer = true;
    public void BotAIActivityOff() {
        isFollowingPlayer = false;
    }
    public void BotAIActivityOn()
    {
        isFollowingPlayer = true;
    }
    //this controls actions 
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float moveSpeed = agentSpeed;
        if (isFollowingPlayer)
        {
            transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
        }
        try {
            transform.LookAt(player.transform);
        }
        catch
        {
            Debug.Log("Error Looking at player");
        }
        

        stepCount = StepCount;
    }

    //this is where the agent collects information
    [SerializeField] private Transform targetTransform;
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(player.transform.position);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
        

    private void OnTriggerEnter(Collider other)
    {
       /* if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(1f );

            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        } 
        if(other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }*/
    }
}

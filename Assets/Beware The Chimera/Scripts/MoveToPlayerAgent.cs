/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToPlayerAgent : Agent
{
    [SerializeField]
    private Transform target;

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(-33.9f, 165.6f, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(target.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log(actions.DiscreteActions[0]);
        //Debug.Log(actions.ContinuousActions[0]);

        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        float moveSpeed = 4f;

        transform.position += new Vector3(moveX, moveY, 0) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetReward(1f);
            EndEpisode();
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
}
*/

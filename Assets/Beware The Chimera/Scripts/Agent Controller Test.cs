using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentControllerTest : Agent
{
    [SerializeField]
    private Transform target;

    /*
    [SerializeField]
    private Transform env;

    [SerializeField]
    private SpriteRenderer backgroundSpriteRenderer;
    */

    private Animator anim;

    private Vector2 moveDirection;

    private Vector2 lastMove;

    private Rigidbody2D rb;

    public override void Initialize()
    {
        base.Initialize();
        anim = GetComponentInChildren<Animator>();
    }
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(0f, 0f), Random.Range(0f, 0f));
        target.localPosition = new Vector3(Random.Range(0f, 0f), Random.Range(-10f, 0f));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        float moveSpeed = 7f;

        moveDirection = new Vector2(moveX, moveY).normalized;
        transform.localPosition += new Vector3(moveX, moveY) * Time.deltaTime * moveSpeed;

        if (!Mathf.Approximately(moveX, 0.0f) || !Mathf.Approximately(moveY, 0.0f))
        {
            lastMove.Set(moveX, moveY);
            moveDirection.Set(moveX, moveY);
            moveDirection.Normalize();
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        anim.SetFloat("Move X", lastMove.x);
        anim.SetFloat("Move Y", lastMove.y);
        anim.SetFloat("Speed", lastMove.magnitude);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector2)transform.position);
        sensor.AddObservation((Vector2) target.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Target target))
        {
            AddReward(50f);
            //backgroundSpriteRenderer.color = Color.green;
            EndEpisode();
        }
        else if (collision.TryGetComponent(out Wall wall) || collision.TryGetComponent(out Obstacle obstacle))
        {
            AddReward(-10f);
            //backgroundSpriteRenderer.color = Color.red;
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
}

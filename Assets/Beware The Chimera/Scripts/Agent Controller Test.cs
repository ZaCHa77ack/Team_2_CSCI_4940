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

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform bulletOrigin;

    // A work in progress. The agent will eventually be able to fire projectiles at the player.
    [SerializeField]
    private float shootCooldown = 2.0f;
    private float shootTimer = 0.0f;

    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private int currentHealth;
    public FloatingHealthBar healthBar;

    // Initialize the agent with its animations, health, and healthbar.
    public override void Initialize()
    {
        base.Initialize();
        anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // At the beginning of each episode, spawn the agent and target at preset locations.
    // These locations may be altered via the Random.Range() function.
    public override void OnEpisodeBegin()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        transform.localPosition = new Vector3(Random.Range(-23f, -23f), Random.Range(11.5f, 11.5f));
        //target.localPosition = new Vector3(Random.Range(19.5f, 19.5f), Random.Range(-2f, -2f));
        target.localPosition = new Vector3(Random.Range(2f, 2f), Random.Range(10f, 10f));
    }

    // Upon receiving a new action, use the local position to move in the corresponding direction.
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        float moveSpeed = 3f;

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

    // Collect observations regarding both the agent's current position and the target's position.
    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation((Vector2) transform.position);
        sensor.AddObservation((Vector2) target.position);
    }

    // Upon colliding with either the target or a wall, reward or penalize the agent.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player target) || collision.TryGetComponent(out PlayerController player))
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

    // Define the continuous actions of the agent, in this case they are horizontal and vertical movement.
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    // This is a work in progress.
    private void Shoot()
    {
        Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle agent's death, such as ending the episode
        EndEpisode();
    }
}

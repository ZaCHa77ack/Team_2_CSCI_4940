using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnAttack;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float chaseDistanceThreshold = 3;
    private float attackDistanceThreshold = 0.8f;

    [SerializeField]
    private float attackDelay = 1;
    private float passedTime = 1;

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            OnMovementInput?.Invoke(Vector2.zero);
            return;
        }

        float distance = Vector2.Distance(player.position, transform.position);
        if (distance < chaseDistanceThreshold)
        {
            OnPointerInput?.Invoke(player.position);
            if (distance <= attackDistanceThreshold)
            {
                //attack behavior
                OnMovementInput?.Invoke(Vector2.zero);
                if (passedTime >= attackDelay)
                {
                    passedTime = 0;
                    OnAttack?.Invoke();
                }
            }
            else
            {
                // Chasing Player
                Vector2 direction = player.position - transform.position;
                OnMovementInput?.Invoke(direction.normalized);
            }
        }
        if (passedTime < attackDelay)
        {
            passedTime += Time.deltaTime;
        }
    }
}

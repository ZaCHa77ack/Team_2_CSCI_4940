using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject tagTarget;

    public float enterSpeed = 1f;

    public SceneAsset sceneToLoad;

    public GameObject fadeAnimation;

    public Canvas canvas;

    private Animator transitionAnimator;

    
    void Start()
    {
        Debug.Log("Level Transition Start");

        canvas = FindObjectOfType<Canvas>();

        if (sceneToLoad == null)
        {
            Debug.LogWarning(name + " has no sceneToLoad set");
        }

        if (fadeAnimation == null)
        {
            Debug.LogWarning(name + " has no fadeAnimation set for the transition");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transitionAnimator != null)
        {
            if (transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                Debug.Log("Transition animation is done");

                SceneManager.LoadScene(sceneToLoad.name, LoadSceneMode.Single);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.isInvulnerable = true;
            }

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;

            Vector2 entranceDirection = (transform.position - rb.transform.position).normalized;

            rb.velocity = entranceDirection * enterSpeed;

            transitionAnimator = Instantiate(fadeAnimation, canvas.transform).GetComponent<Animator>();
        }
    }
}

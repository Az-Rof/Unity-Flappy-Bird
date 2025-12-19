using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float jumpPower = 10f;

    _GUIscript _GUIscript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _GUIscript = GameObject.FindFirstObjectByType<_GUIscript>();
    }

    void Update()
    {
        movement();
        controlPlayedGame();
    }


    void FixedUpdate()
    {

    }

    #region Controller
    void movement()
    {
        if ((Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Mouse0)) && !_GUIscript.isPaused)
        {
            AudioManager.Instance.PlaySFX("Jump");
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero; // Reset velocity to prevent stacking forces
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            transform.rotation = Quaternion.Euler(0, 0, 10); // Set rotation to 10 degrees immediately
        }
        else
        {
            rb.gravityScale = 1;
            if (rb.gravityScale == 1)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -30), 20 * Time.deltaTime); // Slowly rotate towards -30 degrees
            }
        }
    }

    void controlPlayedGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_GUIscript.isPaused)
        {
            _GUIscript.pauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _GUIscript.isPaused)
        {
            _GUIscript.resumeGame();
        }
    }


    #endregion

    // Scoring
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Score")
        {
            _GUIscript.score++;
            AudioManager.Instance.PlaySFX("GetPoint");
        }
    }
}
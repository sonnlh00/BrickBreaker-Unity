using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D ballBody;
    private Paddle paddle;
    private Vector3 ballToPaddle;
    private bool gameStarted;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }
   
    void Start()
    {
        paddle = GameObject.FindObjectOfType<Paddle>();
        ballBody = GetComponent<Rigidbody2D>();
        gameStarted = false;
        
    }
    public void AddForce(Vector2 force)
    {
        ballBody.AddForce(force, ForceMode2D.Impulse);
    }
    // Update is called once per frame
    
    public void CheckVelocity() { 
        // Prevent ball from rolling in the same directon forever
        if (ballBody.velocity.x == 0)
        {
            ballBody.velocity = new Vector2(Random.Range(1, 3), ballBody.velocity.y);
        }
        else if (ballBody.velocity.y == 0)
        {
            ballBody.velocity = new Vector2(ballBody.velocity.x, Random.Range(1, 3));
        }
    }
}

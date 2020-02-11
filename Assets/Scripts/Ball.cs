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
    void Start()
    {
        paddle = GameObject.FindObjectOfType<Paddle>();
        ballBody = GetComponent<Rigidbody2D>();
        gameStarted = false;
    }
    public bool IsServed()
    {
        return gameStarted;
    }
    // Update is called once per frame
    void ServeBall()
    {
        // Move ball with paddle when ball's not been served yet
        ballToPaddle = gameObject.transform.position;
        ballToPaddle.x += paddle.transform.position.x - gameObject.transform.position.x;
        gameObject.transform.position = ballToPaddle;
        // Serve ball on left mouse clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Destroy(GameObject.Find("Instruction Text"));
            ballBody.AddForce(new Vector2(0.5f, 9), ForceMode2D.Impulse);
            gameStarted = true;
        }
    }
    void Update()
    {
        if (!gameStarted)
        {
            ServeBall();
        }
        else
        {
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
}

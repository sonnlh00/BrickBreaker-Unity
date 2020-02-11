using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    private Ball Ball;
    private float mouseXPos;
    private AudioManager audioManager;
    // Variable for automated play testing
    private float ballPos;
    [SerializeField]
    private bool autoPlay;
    // Start is called before the first frame update
    void Start()
    {
        Ball = FindObjectOfType<Ball>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only play sound of collision between ball and paddle when the ball has already been served
        if (Ball.IsServed())
            audioManager.PlayUnbreakableHitAudio();
    }
    // Update is called once per frame
    private void MoveWithMouse()
    {
        // Main gameplay
        // Move paddle with mouse

        // Calculate relative position on x axis of mouse -7.88 <= mouseXPos <= 7.88
        mouseXPos = Input.mousePosition.x / Screen.width * 15.76f - 7.88f;
        Vector3 paddlePos = gameObject.transform.position;
        paddlePos.x = Mathf.Clamp(mouseXPos, -7.88f, 7.88f);
        gameObject.transform.position = paddlePos;
    }
    private void AutomatedPlay()
    {
        // Automated play testing

        ballPos = Ball.transform.position.x;
        Vector3 paddlePos = gameObject.transform.position;
        paddlePos.x = Mathf.Clamp(ballPos, -7.88f, 7.88f);
        gameObject.transform.position = paddlePos;
    }
    void Update()
    {
       
        if (autoPlay)
            AutomatedPlay();
        else
            MoveWithMouse();
    }
}

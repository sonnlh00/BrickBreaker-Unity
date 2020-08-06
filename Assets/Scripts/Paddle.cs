using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    private Ball Ball;
    private AudioManager audioManager;
    private GameManager gameManager;
    private float mouseXPos;
    private float ballPos;
    [SerializeField]
    private bool autoPlay;
    private float zDistance, leftCorner, rightCorner;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Ball = FindObjectOfType<Ball>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        // Restrict paddle position
        zDistance = transform.position.z - Camera.main.transform.position.z;
        // 
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        leftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zDistance)).x + sprite.bounds.size.x/2;
        rightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, zDistance)).x - sprite.bounds.size.x/2;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only play sound of collision between ball and paddle when the ball has already been served
        if (gameManager.IsBallServed())
            audioManager.PlayUnbreakableHitAudio();
    }
    // Update is called once per frame
    private void MoveWithMouse()
    {
        // Main gameplay
        // Move paddle with mouse
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zDistance;
        mouseXPos = Camera.main.ScreenToWorldPoint(mousePos).x;
        Vector3 paddlePos = gameObject.transform.position;
        paddlePos.x = Mathf.Clamp(mouseXPos, leftCorner, rightCorner);
        gameObject.transform.position = paddlePos;
    }
    private void AutomatedPlay()
    {
        // Automated play testing

        ballPos = Ball.transform.position.x;
        Vector3 paddlePos = gameObject.transform.position;
        paddlePos.x = Mathf.Clamp(ballPos, leftCorner, rightCorner);
        gameObject.transform.position = paddlePos;
    }
    void Update()
    {
        if (autoPlay)
            AutomatedPlay();
        else if (gameManager.IsBallServed())
            MoveWithMouse();
    }
}

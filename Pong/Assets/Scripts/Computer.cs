using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float topBounds = 8.3f;
    public float bottomBounds = -8.3f;
    public Vector2 startingPosition = new Vector2(13.0F, 0.0F);

    private Ball ball;
    private Vector2 ballPos;

    [SerializeField] private Game game;

    void Start()
    {
        transform.localPosition = startingPosition;
    }

    void Update()
    {
        if (game.gameState == Game.GameState.Playing)
        {
            Move();
        }      
    }

    void Move()
    {
        if (!ball)
        {
            ball = FindObjectOfType<Ball>();
        }

        if (ball.ballDirection == Vector2.right)
        {
            ballPos = ball.transform.localPosition;

            if (transform.localPosition.y > bottomBounds && ballPos.y < transform.localPosition.y)
            {
                transform.localPosition += new Vector3(0, - moveSpeed * Time.deltaTime , 0);
            }

            if (transform.localPosition.y < topBounds && ballPos.y > transform.localPosition.y)
            {
                transform.localPosition += new Vector3(0, + moveSpeed * Time.deltaTime, 0);
            }
        }
    }
}

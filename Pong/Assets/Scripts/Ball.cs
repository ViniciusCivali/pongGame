using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public float moveSpeed = 12.0f;

    public Vector2 ballDirection = Vector2.left;

    public float topBounds = 9.4f;
    public float bottomBounds = -9.4f;

    public int speedIncreaseInterval = 20;
    public float speedIncreaseBy = 1.0f;
    private float speedIncreaseTimer = 0;
     
    private float playerPaddleHeight, playerPaddleWidth, computerPaddleHeight, computerPaddleWidth, ballWidth, ballHeight,
    playerPaddleMaxX, playerPaddleMinX, playerPaddleMaxY, playerPaddleMinY,
    computerPaddleMaxX, computerPaddleMinX, computerPaddleMaxY, computerPaddleMinY;
                
    private GameObject paddlePlayer, paddleComputer;

    private float bounceAngle;

    private float vx, vy;

    private float maxAngle = 45.0f;

    private bool collideWithPlayer, collideWithComputer, collidedWithWall;

    private Game game;

    private bool assignedpoint;

    void Start()
    {
        game = FindObjectOfType<Game>();

        if(moveSpeed < 0)
        {
            moveSpeed = -1 * moveSpeed;
        }

        paddlePlayer = GameObject.Find("player_paddle");
        paddleComputer = GameObject.Find("computer_paddle");

        SpriteRenderer spriteRenderer = paddlePlayer.GetComponent<SpriteRenderer>();


        playerPaddleHeight = spriteRenderer.bounds.size.y;
        playerPaddleWidth = spriteRenderer.bounds.size.x;

        spriteRenderer = paddleComputer.GetComponent<SpriteRenderer>();

        computerPaddleHeight = spriteRenderer.bounds.size.y;
        computerPaddleWidth = spriteRenderer.bounds.size.x;

        spriteRenderer = GetComponent<SpriteRenderer>();

        ballHeight = spriteRenderer.bounds.size.y;
        ballWidth = spriteRenderer.bounds.size.x;

        playerPaddleMaxX = paddlePlayer.transform.localPosition.x + playerPaddleWidth / 2;
        playerPaddleMinX = paddlePlayer.transform.localPosition.x - playerPaddleWidth / 2;

        computerPaddleMaxX = paddleComputer.transform.localPosition.x - computerPaddleWidth / 2;
        computerPaddleMinX = paddleComputer.transform.localPosition.x + computerPaddleWidth / 2;

        bounceAngle = GetRandomBounceAngle();

        vx = moveSpeed * Mathf.Cos(bounceAngle);
        vy = moveSpeed * -Mathf.Sin(bounceAngle);
    }

    void Update()
    {
        if (game.gameState != Game.GameState.Paused)
        {
            Move();

            UpdateSpeedIncrease();
        }
    }

    void UpdateSpeedIncrease()
    {
        if (speedIncreaseTimer >= speedIncreaseInterval)
        {
            speedIncreaseTimer = 0;

            if (moveSpeed > 0)
            {
                moveSpeed += speedIncreaseBy;
            }
            else
            {
                moveSpeed -= speedIncreaseBy;
            }
        }
        else
        {
            speedIncreaseTimer += Time.deltaTime;
        }
    }

    bool CheckCollision()
    {
   
        playerPaddleMaxY = paddlePlayer.transform.localPosition.y + playerPaddleHeight / 2;
        playerPaddleMinY = paddlePlayer.transform.localPosition.y - playerPaddleHeight / 2;

        computerPaddleMaxY = paddleComputer.transform.localPosition.y + computerPaddleHeight / 2;
        computerPaddleMinY = paddleComputer.transform.localPosition.y - computerPaddleHeight / 2;

        if (((transform.localPosition.x - ballWidth / 2) < playerPaddleMaxX) && ((transform.localPosition.x + ballWidth / 2) > playerPaddleMinX))
        {

            if (((transform.localPosition.y - ballHeight / 2) < playerPaddleMaxY) &&
                ((transform.localPosition.y + ballHeight / 2) > playerPaddleMinY))
            {

                Direction();
                collideWithPlayer = true;
                transform.localPosition = new Vector3(playerPaddleMaxX + 0.01f + ballWidth / 2, transform.localPosition.y, 0);
                return true;

            }
            else if (!assignedpoint)
            {
                assignedpoint = true;
                game.ComputerPoint();
            }               
        }
        else if (((transform.localPosition.x + ballWidth / 2) > computerPaddleMaxX) && ((transform.localPosition.x - ballWidth / 2) < computerPaddleMinX))
        {

            if (((transform.localPosition.y - ballHeight / 2) < computerPaddleMaxY) &&
                ((transform.localPosition.y + ballHeight / 2) > computerPaddleMinY))
            {

                Direction();
                collideWithComputer = true;
                transform.localPosition = new Vector3(computerPaddleMaxX - 0.01f - ballWidth / 2, transform.localPosition.y, 0);
                return true;

            }
            else if (!assignedpoint)
            {
                assignedpoint = true;
                game.PlayerPoint();
            }
        }
        else if(transform.localPosition.y > topBounds)
        {
            transform.localPosition = new Vector3 (transform.localPosition.x, topBounds, 0);
            collidedWithWall = true;
            return true;
        }
        else if (transform.localPosition.y < bottomBounds)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, bottomBounds, 0);
            collidedWithWall = true;
            return true;
        }

        return false;

    }

    void Direction()
    {
        ballDirection *= -1;
    }

    void Move()
    {

        if (!CheckCollision())
        {
            vx = moveSpeed * Mathf.Cos(bounceAngle);

            if (moveSpeed > 0)
            {
                vy = moveSpeed * -Mathf.Sin(bounceAngle);
            }
            else
            {
                vy = moveSpeed * Mathf.Sin(bounceAngle);
            }

            transform.localPosition += new Vector3(ballDirection.x * vx * Time.deltaTime, vy * Time.deltaTime, 0);
        }
        else
        {
            if (moveSpeed < 0)
            {
                moveSpeed = -1 * moveSpeed;
            }

            if (collideWithPlayer == true)
            {

                collideWithPlayer = false;
                float relativeIntersectY = paddlePlayer.transform.localPosition.y - transform.localPosition.y;
                float normalizedRelativeIntersectionY = (relativeIntersectY / (playerPaddleHeight / 2));

                bounceAngle = normalizedRelativeIntersectionY * (maxAngle * Mathf.Deg2Rad);

            }
            else if (collideWithComputer == true)
            {

                collideWithComputer = false;
                float relativeIntersectY = paddleComputer.transform.localPosition.y - transform.localPosition.y;
                float normalizedRelativeIntersectionY = (relativeIntersectY / (computerPaddleHeight / 2));

                bounceAngle = normalizedRelativeIntersectionY * (maxAngle * Mathf.Deg2Rad);

            }
            else if (collidedWithWall == true)
            {

                collidedWithWall = false;

                bounceAngle *= -1; 

            }

        }
    }

    float GetRandomBounceAngle(float minDegrees = 160f, float maxDegrees = 260f)
    {
        float minRad = minDegrees * Mathf.PI / 180;
        float maxRad = maxDegrees * Mathf.PI / 180;

        return Random.Range(minRad, maxRad);
    }
}

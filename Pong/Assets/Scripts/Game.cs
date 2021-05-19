using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private GameObject ball;

    private int computerScore;
    private int playerScore;

    [SerializeField] private GameObject paddleComputer;
    [SerializeField] private GameObject paddlePlayer;
    [SerializeField] private Hud hud;

    public int winningScore = 2;

    public enum GameState
    {
        Playing,
        GameOver,
        Paused,
        Launched
    }

    public GameState gameState = GameState.Launched;

    void Start()
    {
        hud.playAgain.text = ("PRESS SPACEBAR TO PLAY");
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckScore()
    {
        if (computerScore >= winningScore || playerScore >= winningScore)
        {
            if (playerScore >= winningScore && computerScore < (playerScore - 1))
            {
                PlayerWins();
            }
            else if (computerScore >= winningScore && playerScore < (computerScore - 1))
            {
                ComputerWins();
            }
        }
    }

    private void CheckInput()
    {
        if (gameState == GameState.Paused || gameState == GameState.Playing)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                PausedResumeGame();
            }
        }
        else if (gameState == GameState.Launched || gameState == GameState.GameOver)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                StartGame();
            }
        }
    }

    private void PlayerWins()
    {
        hud.winPlayer.enabled = true;
        GameOver();
    }

    private void ComputerWins()
    {
        hud.winComputer.enabled = true;
        GameOver();
    }

    public void ComputerPoint()
    {
        computerScore++;
        hud.computerScore.text = computerScore.ToString();
        CheckScore();
        NextRound();
    }

    public void PlayerPoint()
    {
        playerScore++;
        hud.playerScore.text = playerScore.ToString();
        CheckScore();
        NextRound();
    }

    private void StartGame()
    {
        playerScore = 0;
        computerScore = 0;

        hud.playerScore.text = ("0");
        hud.computerScore.text = ("0");
        hud.winPlayer.enabled = false;
        hud.winComputer.enabled = false;
        hud.playAgain.enabled = false;

        gameState = GameState.Playing;

        paddleComputer.transform.localPosition = Vector2.right * paddleComputer.transform.localPosition.x;
        paddlePlayer.transform.localPosition = new Vector3(paddlePlayer.transform.localPosition.x, 0, 0);

        SpawnBall();
    }

    private void GameOver()
    {
        Destroy(ball.gameObject);
        hud.playAgain.text = "PRESS SPACEBAR TO PLAY AGAIN";
        hud.playAgain.enabled = true;
        gameState = GameState.GameOver;
    }

    private void SpawnBall()
    {
        ball = Instantiate(Resources.Load<GameObject>("Prefabs/ball"));
        ball.transform.localPosition = new Vector3(12, 0, 0);
    }

    private void PausedResumeGame()
    {
        if (gameState == GameState.Paused)
        {
            gameState = GameState.Playing;
            hud.playAgain.enabled = false;
        }
        else
        {
            gameState = GameState.Paused;
            hud.playAgain.text = ("GAME IS PAUSED PRESS SPACE TO CONTINUE");
            hud.playAgain.enabled = true;
        }
    }

    private void NextRound()
    {
        if (gameState == GameState.Playing)
        {
            paddleComputer.transform.localPosition = new Vector3(paddleComputer.transform.localPosition.x, 0, 0);

            GameObject.Destroy(ball.gameObject);

            SpawnBall();
        }
    }





}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 8.0f;
    public float topBounds = 8.3f;
    public float bottomBounds = -8.3f;
    public Vector2 startingPosition = new Vector2(-13.0f, 0.0f);

    [Space]
    [Header("Game")]
    [SerializeField] private Game game;

    void Start()
    {
        transform.localPosition = startingPosition;
    }

    void Update()
    {
        if (game.gameState == Game.GameState.Playing)
        {
            CheckUserInput();
        }
        
    }

    void CheckUserInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {

            if (transform.localPosition.y >= topBounds)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, topBounds, 0);
            }
            else
            {
                transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
            }

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {

            if (transform.localPosition.y <= bottomBounds)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, bottomBounds, 0);
            }
            else
            {
                transform.localPosition += Vector3.down * moveSpeed * Time.deltaTime;
            }

        }
    }
}
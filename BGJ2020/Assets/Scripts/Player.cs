﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horzSpeed;
    private string previousHit;
    private Animator animator;
    private GameManager gameManager;


    [Header("Speed Settings")]
    [SerializeField]
    private float vertSpeed;
    [SerializeField]
    private float rewindSpeed;
    [SerializeField]
    private float fastFowardSpeed;
    [SerializeField]
    private float speedScaler;

    [Header("Collision Settings")]
    [SerializeField]
    private float boxWidth;
    [SerializeField]
    private float boxHeight;
    [SerializeField]
    private GameObject explosionEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        horzSpeed = fastFowardSpeed;
    }

    private void Update()
    {
        if(gameManager.gameHasEnded == false)
        {
            rb.velocity = new Vector2(horzSpeed, Input.GetAxisRaw("Vertical") * vertSpeed);

            HandleCollisions();
        }
    }

    private void HandleCollisions()
    {
        Collider2D col = Physics2D.OverlapBox(gameObject.transform.position, new Vector3(boxWidth, boxHeight, 0f), 0f);

        if (col)
        {
            switch (col.tag)
            {
                case "Rewind":
                    RewindFastForward(true, "Rewind");
                    break;
                case "FastForward":
                    RewindFastForward(false, "FastForward");
                    break;
                case "Obstacle":
                    HitObstacle();
                    break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(boxWidth, boxHeight, 0f));
    }

    private void HitObstacle()
    {
        gameManager.EndGame(false);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    private void RewindFastForward(bool rewind, string type)
    {
        if(type != previousHit)
        {
            rewindSpeed = IncreaseSpeed(rewindSpeed, speedScaler);
            fastFowardSpeed = IncreaseSpeed(fastFowardSpeed, speedScaler);

            if (rewind == true)
            {
                horzSpeed = rewindSpeed;
                previousHit = type;
                animator.Play("Player_RW");
                gameManager.particles.textureSheetAnimation.SetSprite(0, gameManager.rw);
                gameManager.ChangeCameraColor();
            }
            else
            {
                horzSpeed = fastFowardSpeed;
                previousHit = type;
                animator.Play("Player_FF");
                gameManager.particles.textureSheetAnimation.SetSprite(0, gameManager.ff);
                gameManager.ChangeCameraColor();
            }
            gameManager.fasterAnimator.SetTrigger("Faster");
            StartCoroutine(gameManager.WriteText(.01f, "FASTER", gameManager.fasterText));
        }
    }
    
    private float IncreaseSpeed(float currentSpeed, float percentage)
    {
        float increaseAmount = currentSpeed * percentage;
        return currentSpeed + increaseAmount;
    }
}

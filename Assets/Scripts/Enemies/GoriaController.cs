﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoriaController : MonoBehaviour
{
    public float speed;
    public float projectileSpeed;
    public int moveSeed;

    public float moveTime;
    float timer;
    public float attackTime;
    float attackTimer;

    private int direction;
    private System.Random rand;
    private int lastDirection;
    private bool isStopped;
    private bool launched;

    Rigidbody2D rigidbody2d;
    Animator animator;

    Vector2 lookDirection = new Vector2(0, 0);

    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random(moveSeed);
        timer = moveTime;
        attackTimer = attackTime;
        direction = 2;
        lastDirection = direction;
        isStopped = false;
        launched = false;

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped)
        {
            moveWithAI();
        }
        else if (!launched)
        {
            Launch();
            launched = true;
        }
        else
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0)
            {
                isStopped = false;
                attackTimer = attackTime;
                timer = 0;
                launched = false;
            }
        }
        
    }

    public void moveWithAI()
    {
        changeDirection();
        Vector2 position = rigidbody2d.position;
        //just walking around, stop when attacking
        switch (direction)
        {
            case -2:
                position = moveUp(position);
                break;
            case -1:
                position = moveLeft(position);
                break;
            case 1:
                position = moveRight(position);
                break;
            case 2:
                position = moveDown(position);
                break;
            case 0:
                //dont move & thus attack
                isStopped = true;
                break;
        }
        rigidbody2d.position = position;
    }

    void changeDirection()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = rand.Next(-3, 3);
            timer = moveTime;
        }
    }

    Vector2 moveUp(Vector2 position)
    {
        position.y += speed * Time.deltaTime;
        animator.SetFloat("Look Y", 1);
        animator.SetFloat("Look X", 0);
        lastDirection = direction;
        lookDirection.x = 0;
        lookDirection.y = 1;
        return position;
    }

    Vector2 moveDown(Vector2 position)
    {
        position.y -= speed * Time.deltaTime;
        animator.SetFloat("Look Y", -1);
        animator.SetFloat("Look X", 0);
        lastDirection = direction;
        lookDirection.x = 0;
        lookDirection.y = -1;
        return position;
    }

    Vector2 moveRight(Vector2 position)
    {
        position.x += speed * Time.deltaTime;
        animator.SetFloat("Look Y", 0);
        animator.SetFloat("Look X", 1);
        lastDirection = direction;
        lookDirection.x = 1;
        lookDirection.y = 0;
        return position;
    }

    Vector2 moveLeft(Vector2 position)
    {
        position.x -= speed * Time.deltaTime;
        animator.SetFloat("Look Y", 0);
        animator.SetFloat("Look X", -1);
        lastDirection = direction;
        lookDirection.x = -1;
        lookDirection.y = 0;
        return position;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        LinkController controller = other.gameObject.GetComponent<LinkController>();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }

    void Launch()
    {
        GameObject projectile = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.2f, Quaternion.identity);

        BoomerangProjectileController boomerangProjectile = projectile.GetComponent<BoomerangProjectileController>();
        boomerangProjectile.Launch(lookDirection, projectileSpeed);
    }
}

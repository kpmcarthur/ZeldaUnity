using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallmasterController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;

    Vector2 linkPosition;

    public float speed;
    public int moveSeed;

    private int direction;
    private System.Random rand;
    public float moveTime;
    float timer;
    private int grabbed = 0;

    void Start()
    {
        rand = new System.Random(moveSeed);

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbed == 1)
        {
            Vector2 position = transform.position;
            moveDown(position);
            transform.position = position;
        } else
            moveWithAI();
        
    }

    void moveWithAI()
    {
        changeDirection();
        Vector2 position = transform.position;
        switch (direction)
        {
            case -1:
                position = moveUp(position);
                break;
            case 0:
                position = moveRight(position);
                break;
            case 1:
                position = moveLeft(position);
                break;
            case 2:
                position = moveRight(position);
                break;
        }
        transform.position = position;
    }

    void changeDirection()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = rand.Next(-1, 2);
            timer = moveTime;
        }

    }

    Vector2 moveUp(Vector2 position)
    {
        position.y += speed * Time.deltaTime;
        return position;
    }

    Vector2 moveRight(Vector2 position)
    {
        position.x += speed * Time.deltaTime;
        return position;
    }

    Vector2 moveLeft(Vector2 position)
    {
        position.x -= speed * Time.deltaTime;
        return position;
    }

    Vector2 moveDown(Vector2 position)
    {
        position.y -= speed * Time.deltaTime;
        return position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        LinkController controller = other.gameObject.GetComponent<LinkController>();

        if (controller != null)
        {
            grabbed = 1;
            Vector2 position = transform.position;
            controller.transform.position = transform.position;            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed;
    public Transform target;
    public float acceleration;
    public float minDistance;
    public GameObject projectile;
    public float timeBetweenShots = 1;
    private Rigidbody2D rb;
    private float nextShotTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    private void FixedUpdate()
    {
        if (Time.time > nextShotTime)
        {

            Instantiate(projectile, transform.position, Quaternion.identity);
            nextShotTime = Time.time + timeBetweenShots;
        }
        if (Vector2.Distance(transform.position, target.position) < minDistance)
        {

            transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);

        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}

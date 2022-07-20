using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool wait = false;
    public float speed;
    public GameObject target;
    public float acceleration;
    public float minDistance;
    public GameObject projectile;
    public float timeBetweenShots = 1;
    public Rigidbody2D targetRb;
    private Rigidbody2D rb;
    private float nextShotTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetRb = GetComponent<Rigidbody2D>();

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
         if(!wait)EnemyLogic();
        
    }
    private async void EnemyLogic()
    {
        

        if (Vector2.Distance(transform.position, target.transform.position) < minDistance)
        {
            var startTime = Time.time + 1;
            Vector2 toTarget = (target.transform.position - transform.position).normalized;
            
            while (Time.time < startTime)
            {
                
                wait = true;
                rb.velocity = toTarget * -speed/2;
                await Task.Yield();
            }
           
            wait=false;
        }
        else
        {
            Vector2 toTarget = (target.transform.position - transform.position).normalized;
            rb.velocity = toTarget * speed;


        }
    }
}

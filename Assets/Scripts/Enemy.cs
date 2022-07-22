using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    public static Enemy instance;
    public int health = 20;
    public bool isHit = false;
    private bool nearGround;
    public bool wait = false;
    [SerializeField] private float speed;
    private GameObject target;
    [SerializeField] private float acceleration;
    [SerializeField] private float minDistance;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float timeBetweenShots = 1;
    private Rigidbody2D rb;
    private float nextShotTime;
    [SerializeField] private float _groundRaycastLength;
    [SerializeField] private Vector3 _groundRaycastOffset;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        CheckCollisions();
        Physics2D.IgnoreLayerCollision(11,11);

        if (Time.time > nextShotTime)
        {
            var toTarget = target.transform.position - transform.position;
            Instantiate(projectile, target.transform.position, transform.rotation );
            nextShotTime = Time.time + timeBetweenShots;
        }
         if(!wait && !isHit)StartCoroutine(EnemyLogic());

    }
    public IEnumerator EnemyLogic()
    {
        

        if (Vector2.Distance(transform.position, target.transform.position) < minDistance || nearGround )
        {
            var startTime = Time.time + 1;
            Vector2 toTarget = (target.transform.position - transform.position).normalized;
            while (Time.time < startTime && !isHit)
            {
                
                    wait = true;
                    if (transform.position.y < target.transform.position.y) rb.velocity = Vector2.up * speed / 2;
                    else rb.velocity = toTarget * -speed / 2;
                    yield return null;
                
                
                
            }
            wait = false;
            yield return new WaitForSeconds(.3f);
            isHit = false;
            
        }
        else
        {
            Vector2 toTarget = (target.transform.position - transform.position).normalized;
            rb.velocity = toTarget *speed;


        }
        
    }
  
    private void CheckCollisions()
    {
        nearGround = Physics2D.Raycast(transform.position + _groundRaycastOffset, Vector2.down, _groundRaycastLength, groundMask) ||
                    Physics2D.Raycast(transform.position - _groundRaycastOffset, Vector2.down, _groundRaycastLength, groundMask);
       

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + _groundRaycastOffset, transform.position + _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - _groundRaycastOffset, transform.position - _groundRaycastOffset + Vector3.down * _groundRaycastLength);

        //Wall Check

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Vector2 toTarget;
    Vector3 targetPosition;
    private Rigidbody2D rb;
    public float speed;
    private Animator anim;
    private bool isActive = true;


    // Start is called before the first frame update

    void Start()
    {
        
        targetPosition = FindObjectOfType<Player>().transform.position;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        toTarget = targetPosition - transform.position;


    }

    // Update is called once per frame
    void Update()
    {
        
        anim.SetBool("isActive", isActive);
        
    }
    private void FixedUpdate()
    {
        rb.AddForce(toTarget, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Debug.Log("should be destroyed");
    }
    private void SeekAndDestroy()
    {
     
        
    }
}

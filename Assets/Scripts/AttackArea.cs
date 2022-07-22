using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private float _pushValue = 5f;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<Rigidbody2D>() !=null)
        {
            Rigidbody2D eb = collider.GetComponent<Rigidbody2D>();
            if (Player.instance._facingRight) {
                eb.velocity = new Vector2(0f, 0f);
                eb.AddForce(Vector2.right * _pushValue, ForceMode2D.Impulse);
            } 
            else
            {
                eb.velocity = new Vector2(0f, 0f);
                eb.AddForce(Vector2.left * _pushValue, ForceMode2D.Impulse);
            }
        }
        if (collider.CompareTag("Enemy")){
            Vector2 toTarget = (collider.transform.position - transform.position).normalized;
            Rigidbody2D eb = collider.GetComponent<Rigidbody2D>();
            collider.GetComponent<Enemy>().isHit = true;
            eb.velocity = new Vector2(0f, 0f);
            eb.AddForce(toTarget * _pushValue*2, ForceMode2D.Impulse);
            collider.GetComponent<Health>().health -= 10;
            



        }
    }
}

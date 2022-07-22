using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0) StartCoroutine(DestroyGameObject());
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    private IEnumerator DestroyGameObject()
    {
         yield return new WaitForSeconds(.3f);
         Destroy(gameObject);
        
    }
}

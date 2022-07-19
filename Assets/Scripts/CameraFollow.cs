using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float _followSpeed = 8f;
    public float _yOffset = 1f;
    public float _zOffset = -3f;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x,_yOffset, _zOffset);
        transform.position = Vector3.Slerp(transform.position,newPos, _followSpeed *Time.deltaTime);
    }
}

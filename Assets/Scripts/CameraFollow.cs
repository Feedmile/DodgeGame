using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float _followSpeed = 8f;
    public float _yOffset = 1f;
    public float _zOffset = -3f;
    [SerializeField] private GameObject playerPrefab;
    // Start is called before the first frame update
    private void Awake()
    {
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(playerPrefab.transform.position.x, playerPrefab.transform.position.y +_yOffset, _zOffset - playerPrefab.transform.position.y/2);
        transform.position = Vector3.Slerp(transform.position,newPos, _followSpeed *Time.deltaTime);
    }
}

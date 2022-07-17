using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummScript : MonoBehaviour
{
    private Player _prefab;
    private Rigidbody2D _rb;
    private bool _onWall;
    private bool _onRightWall;
    private bool _onGround;
    private float _bounceValue = 7f;
    [SerializeField] private Vector3 _groundRaycastOffset;
    [SerializeField] private float _groundRaycastLength;
    [SerializeField] private float _wallRaycastLength;


    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {

        CheckCollisions();
        Physics2D.IgnoreLayerCollision(6,7);
        if (_onGround && _onWall && !_onRightWall)
        {
            _rb.velocity = new Vector2(0f, 0f);
            _rb.AddForce(Vector2.right * _bounceValue, ForceMode2D.Impulse);
        }
        if (_onGround && _onRightWall)
        {
            _rb.velocity = new Vector2(0f, 0f);
            _rb.AddForce(Vector2.left * _bounceValue, ForceMode2D.Impulse);
        }
    }
    
        private void CheckCollisions()
    {
        _onGround = Physics2D.Raycast(transform.position + _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer) ||
                    Physics2D.Raycast(transform.position - _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer);
        _onWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer) ||
                  Physics2D.Raycast(transform.position, Vector2.left, _wallRaycastLength, _wallLayer);
        _onRightWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + _groundRaycastOffset, transform.position + _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - _groundRaycastOffset, transform.position - _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        //Wall Check
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _wallRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * _wallRaycastLength);
    }
}

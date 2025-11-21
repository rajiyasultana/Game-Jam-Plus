using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    private Rigidbody _rb;
    private float _distToGround;
    private bool _jumpRequest; 

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _distToGround = GetComponent<Collider>().bounds.extents.y;
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
            _jumpRequest = true;
    }
    
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed;

        // Use MovePosition instead of velocity
        Vector3 targetPos = _rb.position + movement * Time.fixedDeltaTime;
        _rb.MovePosition(targetPos);

        if (_jumpRequest)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _jumpRequest = false;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _distToGround + 0.1f);
    }
}
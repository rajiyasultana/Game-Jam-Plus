using UnityEngine;
using System.Collections.Generic; // Required for Queue

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public CameraFollow gameCamera; 
    public Animator characterAnimator; 

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 15f; 
    public float jumpForce = 8f;

    [Header("Respawn Settings")]
    public float fallThreshold = -5f; // The Y level where player dies
    public float rewindTime = 2f;     // How many seconds to go back

    private Rigidbody _rb;
    private float _distToGround;
    private bool _jumpRequest; 
    private bool _isCameraActive = false;

    // RESPAWN VARIABLES
    private Queue<Vector3> _positionHistory = new Queue<Vector3>();
    private Vector3 _respawnPosition;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _distToGround = GetComponent<Collider>().bounds.extents.y;
        
        // Initialize respawn position to current start position to avoid errors
        _respawnPosition = transform.position;

        if (MessageManager.Instance != null)
            MessageManager.Instance.ShowMessage("Welcome to the game!\n\n" + "<b>Now you will understand the PAIN behind FUN -_- <b>", 5f);
    }
    
    void Update()
    {
        if (GameManager.Instance == null) return;

        // --- FALL CHECK (RESPAWN LOGIC) ---
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }

        // --- JUMP LOGIC ---
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            if (GameManager.Instance.HasAbility(AbilityType.Jump))
            {
                _jumpRequest = true;
                if (characterAnimator != null && characterAnimator.isActiveAndEnabled)
                {
                    characterAnimator.SetTrigger("Jump");
                }
            }
        }
        
        // --- CAMERA LOGIC ---
        if (!_isCameraActive && GameManager.Instance.HasAbility(AbilityType.Camera))
        {
            if (gameCamera != null)
            {
                gameCamera.StartFollowing(transform);
                _isCameraActive = true; 
            }
        }
    }
    
    void FixedUpdate()
    {
        // --- 1. RECORD HISTORY FOR RESPAWN ---
        TrackPositionHistory();

        // --- MOVEMENT & PHYSICS ---
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputVector = new Vector3(horizontalInput, 0f, verticalInput);
        
        Vector3 movement = inputVector * moveSpeed;
        Vector3 targetPos = _rb.position + movement * Time.fixedDeltaTime;
        _rb.MovePosition(targetPos);

        if (inputVector.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputVector);
            Quaternion nextRotation = Quaternion.Slerp(_rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            _rb.MoveRotation(nextRotation);
        }

        if (characterAnimator != null && characterAnimator.isActiveAndEnabled)
        {
            float currentSpeed = inputVector.magnitude; 
            characterAnimator.SetFloat("Speed", currentSpeed);
        }
        
        if (_jumpRequest)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _jumpRequest = false;
        }
    }

    // --- NEW HELPER METHODS ---

    void TrackPositionHistory()
    {
        // Only record history if we are safely on the ground (optional, but feels better)
        // If you want to rewind even if you were in the air, remove the IsGrounded() check.
        if (IsGrounded()) 
        {
            _positionHistory.Enqueue(transform.position);
        }

        // Calculate how many frames represent 2 seconds
        // Example: 2.0s / 0.02s per frame = 100 frames
        int maxFrames = Mathf.RoundToInt(rewindTime / Time.fixedDeltaTime);

        // If we have stored more frames than needed, remove the oldest one
        // and save it as our "safe respawn point"
        if (_positionHistory.Count > maxFrames)
        {
            _respawnPosition = _positionHistory.Dequeue();
        }
    }

    void Respawn()
    {
        Debug.Log("Fell off map! Rewinding 2 seconds...");

        // 1. Move player to the old position
        transform.position = _respawnPosition;

        // 2. KILL VELOCITY (Important! Otherwise you keep falling at high speed)
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // 3. Clear history so we don't accidentally respawn back into the void immediately
        _positionHistory.Clear();
        
        // Reset respawn point to current spot so we don't glitch
        _respawnPosition = transform.position;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _distToGround + 0.5f);
    }
}
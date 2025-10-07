using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxAccelerationValue;
    [SerializeField] private float maxRotationSpeed;
    
    private bool _isAccelerating;
    private float _stickX;
    
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (_isAccelerating) _rigidbody.AddForce(transform.forward * maxAccelerationValue);
        if (_isAccelerating) transform.Translate(maxAccelerationValue * Time.deltaTime * Vector3.forward);
        
        transform.Rotate(transform.up, _stickX * maxRotationSpeed * Time.deltaTime);
        
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isAccelerating = true;
        }

        if (context.canceled)
        {
            _isAccelerating = false;
        }
    }
    
    public void OnStick(InputAction.CallbackContext context)
    {
        _stickX = context.ReadValue<float>();
    }
}

using System;
using UnityEditor;
using UnityEngine;

public class MyCarPhysics : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxBackwardSpeed;
    [SerializeField] private float accelerationFactor;
    [SerializeField] private float rotationSpeedFactor;
    [SerializeField] private float noDriftFactor;
    [SerializeField] private float driftFactor;
    
    private float _accelerationInput;
    private float _rotationInput;
    private float _angle;
    private bool _isDrifting;
    private float _currentDriftFactor;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyEngineForce();

        AlignToGround();
        
        KillOrthogonalVelocity();
        
        ApplySteering();
    }

    void ApplyEngineForce()
    {
        Vector2 velocity2d = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.z);
        Vector2 forward2d = new Vector2(transform.forward.x, transform.forward.z);
        Vector2 forwardVelocity = forward2d * Vector2.Dot(velocity2d, forward2d);
        
        float speed = forwardVelocity.magnitude;
        float direction = Vector2.Dot(forward2d, forwardVelocity);
        
        if ((direction > 0 && speed < maxSpeed) || (direction < 0 && speed < maxBackwardSpeed)) 
            _rb.AddForce(_accelerationInput * accelerationFactor * transform.forward, ForceMode.VelocityChange);
    }

    void ApplySteering()
    {
        Vector2 velocity2d = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.z);
        Vector2 forward2d = new Vector2(transform.forward.x, transform.forward.z);
        Vector2 forwardVelocity = forward2d * Vector2.Dot(velocity2d, forward2d);
        
        float direction = Vector2.Dot(forward2d, forwardVelocity);
        
        float sign = Math.Sign(direction);
        
        // update angle only if the object is moving 
        if (_rb.linearVelocity.magnitude >= 1) 
            _angle -= _rotationInput * sign * rotationSpeedFactor;
        else if (_rb.linearVelocity.magnitude >= 0.6)
            _angle -= _rotationInput * sign * rotationSpeedFactor * _rb.linearVelocity.magnitude;
  
        
        Quaternion rotation = Quaternion.AngleAxis(_angle, Vector3.up);
        _rb.MoveRotation(rotation);
    }

    void AlignToGround()
    {
        // Get the ground 
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.5f))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            _rb.MoveRotation(targetRotation);
        }
    }
    
    void KillOrthogonalVelocity()
    {
        float upVelocity = _rb.linearVelocity.y;
        
        Vector2 velocity2d = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.z);
        Vector2 forward2d = new Vector2(transform.forward.x, transform.forward.z);
        Vector2 right2d = new Vector2(transform.right.x, transform.right.z);
        
        Vector2 forwardVelocity = forward2d * Vector2.Dot(velocity2d, forward2d);
        Vector2 rightVelocity = right2d * Vector2.Dot(velocity2d, right2d);
        
        Vector2 finalVelocity2d = forwardVelocity + (driftFactor * rightVelocity);
        
        _rb.linearVelocity = new Vector3(finalVelocity2d.x, upVelocity, finalVelocity2d.y);
    }

    public void SetAccelerationValue(float accelerationInput)
    {
        _accelerationInput = accelerationInput;
    }

    public void SetRotationValue(float rotationInput)
    {
        _rotationInput = rotationInput;
    }

    public void SetIsDrifting(bool isDrifting)
    {
        _isDrifting =  isDrifting;
        
        _currentDriftFactor = _isDrifting ? driftFactor : noDriftFactor;
    }
}

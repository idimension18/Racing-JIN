using System;
using UnityEngine;

public class MyCarPhysics : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxBackwardSpeed;
    [SerializeField] private float accelerationFactor;
    [SerializeField] private float rotationSpeedFactor;
    [SerializeField] private float noDriftFactor;
    [SerializeField] private float driftFactor;
    [SerializeField] private float driftBreakFactor;
    [SerializeField] private float boostTime;
    [SerializeField] private float boostAccelerationFactor;
    [SerializeField] private float boostMaxSpeed;
    [SerializeField] private float grassFactor;
    [SerializeField] private float grassMaxSpeed;
    
    private float _accelerationInput;
    private float _rotationInput;
    private float _angle;
    private bool _isDrifting;
    private float _currentDriftFactor;
    private bool _isBoost;
    private float _boostTimer;
    private bool _isOnGrass;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _angle = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyBoostForce();
        
        ApplyEngineForce();
        
        AlignToGround();
        
        KillOrthogonalVelocity();
        
        ApplySteering();
        
        ApplyDriftBreak();

        ApplyGrassForce();
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
        
        float direction = Math.Sign(Vector2.Dot(forward2d, forwardVelocity));
        
        // update angle only if the object is moving 
        if (_rb.linearVelocity.magnitude >= 1) 
            _angle -= _rotationInput * direction * rotationSpeedFactor;
        
        else if (_rb.linearVelocity.magnitude >= 0.6) 
            _angle -= _rotationInput * direction * rotationSpeedFactor * _rb.linearVelocity.magnitude;
        
        _rb.MoveRotation(Quaternion.AngleAxis(_angle, Vector3.up));
    }

    
    /**
     * Turn the car so That all wheels touch the ground
     */
    void AlignToGround()
    {
        // Get the ground 
        Ray ray = new Ray(transform.position, -transform.up);
        if (!Physics.Raycast(ray, out RaycastHit hit, 0.5f)) return;
        
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        _rb.MoveRotation(targetRotation);
    }
    
    /**
     * Make the car less slippery by changing the Orthogonal Velocity.
     */
    void KillOrthogonalVelocity()
    {
        float upVelocity = _rb.linearVelocity.y;
        
        Vector2 velocity2d = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.z);
        Vector2 forward2d = new Vector2(transform.forward.x, transform.forward.z);
        Vector2 right2d = new Vector2(transform.right.x, transform.right.z);
        
        Vector2 forwardVelocity = forward2d * Vector2.Dot(velocity2d, forward2d);
        Vector2 rightVelocity = right2d * Vector2.Dot(velocity2d, right2d);
        
        Vector2 finalVelocity2d = forwardVelocity + (_currentDriftFactor * rightVelocity);
        
        _rb.linearVelocity = new Vector3(finalVelocity2d.x, upVelocity, finalVelocity2d.y);
    }

    private void ApplyDriftBreak()
    {
        if (!_isDrifting) return;
        
        Vector2 velocity2d = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.z);
        Vector2 forward2d = new Vector2(transform.forward.x, transform.forward.z);
        Vector2 forwardVelocity = forward2d * Vector2.Dot(velocity2d, forward2d);
        
        float speed = forwardVelocity.magnitude;
        float direction = Math.Sign(Vector2.Dot(forward2d, forwardVelocity));
        
        if (speed > 0)
            _rb.AddForce(driftBreakFactor * (-direction) * transform.forward, ForceMode.VelocityChange);
    }

    private void ApplyBoostForce()
    {
        if (!_isBoost) return;

        if (_boostTimer > 0)
        {
            Vector2 velocity2d = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.z);
            Vector2 forward2d = new Vector2(transform.forward.x, transform.forward.z);
            Vector2 forwardVelocity = forward2d * Vector2.Dot(velocity2d, forward2d);
        
            float speed = forwardVelocity.magnitude;
        
            if (speed < boostMaxSpeed) 
                _rb.AddForce(boostAccelerationFactor * transform.forward, ForceMode.VelocityChange);
            
            _boostTimer -= Time.fixedDeltaTime;
        }
        else
        {
            _isBoost = false;
        }
    }

    private void ApplyGrassForce()
    {
        if (!_isOnGrass) return;
        
        Vector2 velocity2d = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.z);
        Vector2 forward2d = new Vector2(transform.forward.x, transform.forward.z);
        Vector2 forwardVelocity = forward2d * Vector2.Dot(velocity2d, forward2d);
        
        float speed = forwardVelocity.magnitude;
        float direction = Math.Sign(Vector2.Dot(forward2d, forwardVelocity));

        if (speed < grassMaxSpeed) return;
        
        _rb.AddForce(grassFactor * (-direction) * transform.forward, ForceMode.VelocityChange);
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
        _isDrifting = isDrifting;
        
        _currentDriftFactor = _isDrifting ? driftFactor : noDriftFactor;
    }

    public void TriggerBoost()
    {
        _isBoost = true;
        _boostTimer = boostTime;
    }

    public void SetOnGrass(bool isOnGrass)
    {
        _isOnGrass = isOnGrass;
    }
    
}

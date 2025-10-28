using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MyCarPhysics _carPhysics;
    private Rigidbody _rigidbody;

    void Awake()
    {
        _carPhysics = GetComponent<MyCarPhysics>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnBreak(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _carPhysics.SetAccelerationValue(-0.5f);
        }

        if (context.canceled)
        {
            _carPhysics.SetAccelerationValue(0);
        }
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _carPhysics.SetAccelerationValue(1);
        }

        if (context.canceled)
        {
            _carPhysics.SetAccelerationValue(0);
        }
    }
    
    public void OnStick(InputAction.CallbackContext context)
    {
        _carPhysics.SetRotationValue(-context.ReadValue<float>());
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        
    }

    public void OnDrift(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _carPhysics.SetIsDrifting(true);
        }

        if (context.canceled)
        {
            _carPhysics.SetIsDrifting(false);
        }
    }
}

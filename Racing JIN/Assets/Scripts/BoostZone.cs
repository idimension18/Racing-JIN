using UnityEngine;

public class BoostZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        MyCarPhysics car = other.GetComponentInParent<MyCarPhysics>();
        if (!car) return;
        
        car.TriggerBoost();
    }
}

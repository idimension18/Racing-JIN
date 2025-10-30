using UnityEngine;

public class GrassZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        MyCarPhysics car = other.GetComponentInParent<MyCarPhysics>();
        if (car==null) return;
        
        car.SetOnGrass(true);
    }
    
    private void OnTriggerStay(Collider other)
    {
        MyCarPhysics car = other.GetComponentInParent<MyCarPhysics>();
        if (!car) return;
        
        car.SetOnGrass(true);
    }

    private void OnTriggerExit(Collider other)
    {
        MyCarPhysics car = other.GetComponentInParent<MyCarPhysics>();
        if (!car) return;
        
        car.SetOnGrass(false);
    }
}

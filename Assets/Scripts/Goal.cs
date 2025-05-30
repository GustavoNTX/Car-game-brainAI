using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Car"))
        {
            CarController car = other.GetComponent<CarController>();
            if (car != null)
            {
                car.ReachGoal();
                Debug.Log("Carro alcançou o objetivo!");
            }
        }
    }
}

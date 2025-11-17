using UnityEngine;

public class ZonaProhibida : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador ha ingresado a la zona prohibida. Game Over.");
        }
    }
}
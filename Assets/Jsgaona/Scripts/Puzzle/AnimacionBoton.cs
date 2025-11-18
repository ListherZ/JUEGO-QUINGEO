using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    [Header("Settings")]
    public float pressAmount =10f;      // Qué tanto baja el botón
    public float pressSpeed = 2f;         // Qué tan rápido se mueve
    public bool isPressed = false;

    private Vector3 originalPos;
    private Vector3 pressedPos;

    void Start()
    {
        originalPos = transform.localPosition;
        pressedPos = originalPos - new Vector3(0, pressAmount, 0);
    }

    void Update()
    {
        if (isPressed)
        {
            // Mover hacia abajo
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                pressedPos,
                Time.deltaTime * pressSpeed
            );
        }
        else
        {
            // Regresar hacia arriba
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                originalPos,
                Time.deltaTime * pressSpeed
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPressed = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPressed = false;
    }
}

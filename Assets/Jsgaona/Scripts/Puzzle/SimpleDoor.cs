using UnityEngine;

public class SimpleDoor : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float openSpeed = 2f;
    public bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    [SerializeField] private AudioSource doorSound;
    void Start()
    {
        closedRotation = Quaternion.Euler(0, 90, 0);   // Rotación inicial
        openRotation = Quaternion.Euler(0, 180, 0);    // Rotación final

        transform.localRotation = closedRotation;
    }
    private bool YaReprodujo = false;
    void Update()
    {
        if (isOpen)
        {
            if (!YaReprodujo)
            {
                doorSound.Play();
                YaReprodujo = true;
            }
            // Mover hacia la rotación abierta
          
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                openRotation,
                Time.deltaTime * openSpeed
            );
        }
        else
        {
            // Regresar a la rotación cerrada (opcional)
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                closedRotation,
                Time.deltaTime * openSpeed
            );
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()   // opcional
    {
        isOpen = false;
    }
}

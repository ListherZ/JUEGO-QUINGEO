using UnityEngine;

public class RotatingTile : MonoBehaviour
{
    [Header("Rotación")]
    public float rotateSpeed = 10f;

    [Tooltip("0 = 0°, 1 = 90°, 2 = 180°, 3 = 270°")]
    public int rotationStep = 0;            // Rotación ACTUAL
    public int correctRotationStep = 0;     // Rotación que RESUELVE el puzzle

    private Quaternion targetRot;
    private Renderer rend;

    [Header("Feedback Visual")]
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    [Header("Sonidos")]
    public AudioClip correctSFX;
    public AudioClip wrongSFX;
    private AudioSource audioSource;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        audioSource = gameObject.AddComponent<AudioSource>();

        // Asegura que la rotación visual coincide con rotationStep
        targetRot = Quaternion.Euler(0, rotationStep * 90f, 0);
        transform.localRotation = targetRot;

        // Color inicial
        rend.material.color = IsCorrect() ? correctColor : wrongColor;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * rotateSpeed);
    }

    public void RotateTile()
    {
        // Girar 90°
        rotationStep = (rotationStep + 1) % 4;
        targetRot = Quaternion.Euler(0, rotationStep * 90f, 0);

        // Si está correcta
        if (IsCorrect())
        {
            //emission

            rend.material.color = correctColor;
            // Emisión
            if (rend.material.HasProperty("_EmissionColor"))
            {
                rend.material.SetColor("_EmissionColor", correctColor * 1.5f);
                rend.material.EnableKeyword("_EMISSION");
            }
            if (correctSFX != null)
                audioSource.PlayOneShot(correctSFX, 0.5f);
        }
        else
        {
            rend.material.color = wrongColor;
            if (rend.material.HasProperty("_EmissionColor"))
            {
                rend.material.SetColor("_EmissionColor", wrongColor * 1.5f);
                rend.material.EnableKeyword("_EMISSION");
            }
            if (wrongSFX != null)
                audioSource.PlayOneShot(wrongSFX, 0.3f);
        }
    }

    public bool IsCorrect()
    {
        return rotationStep == correctRotationStep;
    }
}

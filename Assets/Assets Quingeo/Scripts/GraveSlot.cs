using UnityEngine;

public class GraveSlot : MonoBehaviour
{
    public bool isValidGrave = true;

    [Header("Colocación")]
    public Transform placePoint;
    public bool HasFlower { get; private set; }

    [Header("Visual")]
    [SerializeField] private Renderer graveRenderer;
    [SerializeField] private float aimBoost = 2.0f;     // cuando apuntas
    [SerializeField] private float guideBoost = 1.3f;   // cuando llevas flor (guía)

    private Material mat;
    private Color baseEmission;

    private void Awake()
    {
        if (!graveRenderer) graveRenderer = GetComponentInChildren<Renderer>();
        if (graveRenderer)
        {
            mat = graveRenderer.material;
            if (mat.HasProperty("_EmissionColor"))
                baseEmission = mat.GetColor("_EmissionColor");
        }
    }

    public void SetAimHighlight(bool on)
    {
        if (mat == null || !mat.HasProperty("_EmissionColor")) return;
        mat.SetColor("_EmissionColor", on ? baseEmission * aimBoost : baseEmission);
    }

    public void SetGuideHighlight(bool on)
    {
        if (!isValidGrave || HasFlower) on = false;
        if (mat == null || !mat.HasProperty("_EmissionColor")) return;

        // Guía suave: si está activo y no está apuntado, boost ligero.
        if (on)
            mat.SetColor("_EmissionColor", baseEmission * guideBoost);
        else
            mat.SetColor("_EmissionColor", baseEmission);
    }

    public bool TryPlaceFlower(GameObject placedFlowerPrefab)
    {
        if (!isValidGrave) return false;
        if (HasFlower) return false;
        if (placePoint == null) return false;

        GameObject placedFlower = Instantiate(placedFlowerPrefab, placePoint.position, placePoint.rotation, placePoint);
        DisableFlowerInteraction(placedFlower);
        HasFlower = true;

        // Al quedar ocupada, baja su guía
        SetGuideHighlight(false);
        SetAimHighlight(false);

        return true;
    }

    private static void DisableFlowerInteraction(GameObject placedFlower)
    {
        if (placedFlower == null) return;

        foreach (var pickup in placedFlower.GetComponentsInChildren<FlowerPickup>(true))
        {
            Object.Destroy(pickup);
        }

        foreach (var col in placedFlower.GetComponentsInChildren<Collider>(true))
        {
            col.enabled = false;
        }
    }
}

using UnityEngine;

public class FlowerPickup : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private GameObject idleVfx; // partícula sutil
    [SerializeField] private Renderer[] renderers;

    private Material[] mats;
    private Color[] baseEmission;

    public bool IsPicked { get; private set; }

    private void Awake()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>();

        mats = new Material[renderers.Length];
        baseEmission = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            mats[i] = renderers[i].material;
            if (mats[i].HasProperty("_EmissionColor"))
                baseEmission[i] = mats[i].GetColor("_EmissionColor");
        }
    }

    public void SetHighlighted(bool on)
    {
        for (int i = 0; i < mats.Length; i++)
        {
            if (!mats[i].HasProperty("_EmissionColor")) continue;
            mats[i].SetColor("_EmissionColor", on ? baseEmission[i] * 2.0f : baseEmission[i]);
        }
    }

    public void SetPicked(bool picked)
    {
        IsPicked = picked;

        if (idleVfx) idleVfx.SetActive(!picked);

        var col = GetComponent<Collider>();
        if (col) col.enabled = !picked;

        var rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = picked;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        gameObject.SetActive(!picked); // se oculta del mundo al recoger
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractorMobile : MonoBehaviour
{
    [Header("Refs")]
    public Camera playerCamera;
    public Transform carryPoint; // Empty delante del player
    public CemeteryMinigameManager gameManager;
    public MinigameUI ui;

    [Header("Raycast")]
    public float interactDistance = 3f;
    public LayerMask interactMask;

    private FlowerPickup targetFlower;
    private GraveSlot targetGrave;

    private FlowerPickup carriedFlower;
    private GameObject carriedVisual;

    private FlowerPickup lastFlowerHL;
    private GraveSlot lastGraveAimHL;

    private void Start()
    {
        ui.interactButton.onClick.RemoveAllListeners();
        ui.interactButton.onClick.AddListener(OnInteractPressed);

        ui.SetCarry(false);
        ui.SetInteract(false, "");
        ui.SetInstruction("Busca una flor 🌸");
    }

    private void Update()
    {
        ScanTargets();
        UpdateGuidanceAndUI();
        UpdateValidGravesGuide();
    }

    private void ScanTargets()
    {
        targetFlower = null;
        targetGrave = null;

        // Limpia highlights previos
        if (lastFlowerHL != null) lastFlowerHL.SetHighlighted(false);
        if (lastGraveAimHL != null) lastGraveAimHL.SetAimHighlight(false);

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask))
        {
            targetFlower = hit.collider.GetComponentInParent<FlowerPickup>();
            targetGrave = hit.collider.GetComponentInParent<GraveSlot>();
        }

        // Highlight actual
        if (targetFlower != null && !targetFlower.IsPicked)
        {
            targetFlower.SetHighlighted(true);
            lastFlowerHL = targetFlower;
        }
        else lastFlowerHL = null;

        if (targetGrave != null)
        {
            targetGrave.SetAimHighlight(true);
            lastGraveAimHL = targetGrave;
        }
        else lastGraveAimHL = null;
    }

    private void UpdateGuidanceAndUI()
    {
        ui.SetCarry(carriedFlower != null);

        // Estado 1: no llevo flor
        if (carriedFlower == null)
        {
            if (targetFlower != null && !targetFlower.IsPicked)
            {
                ui.SetInstruction("Toca el botón para recoger la flor 🌸");
                ui.SetInteract(true, "Recoger flor");
            }
            else
            {
                ui.SetInstruction("Busca una flor 🌸 (brilla ligeramente)");
                ui.SetInteract(false, "");
            }
            return;
        }

        // Estado 2: llevo flor
        if (targetGrave == null)
        {
            ui.SetInstruction("Ve a una tumba correcta (color marcado) y coloca la flor 🌼");
            ui.SetInteract(false, "");
            return;
        }

        // Estado 3: llevo flor y apunto a tumba
        if (!targetGrave.isValidGrave)
        {
            ui.SetInstruction("Esa tumba no es válida ❌ Busca una tumba correcta");
            ui.SetInteract(true, "Tumba incorrecta");
            return;
        }

        if (targetGrave.HasFlower)
        {
            ui.SetInstruction("Esa tumba ya tiene flor ⚠️ Busca otra");
            ui.SetInteract(true, "Ocupada");
            return;
        }

        ui.SetInstruction("Toca para colocar la flor ✅");
        ui.SetInteract(true, "Colocar flor");
    }

    private void UpdateValidGravesGuide()
    {
        // Guía visual: si llevo flor, resalta todas las tumbas válidas no ocupadas suavemente
        // Para no buscar cada frame en grande, lo hacemos simple: buscar una vez y cachear si quieres.
        // En cementerio pequeño, esto va bien.

        if (carriedFlower == null)
        {
            // Apaga guía
            var gravesOff = FindObjectsOfType<GraveSlot>();
            foreach (var g in gravesOff) g.SetGuideHighlight(false);
            return;
        }

        var graves = FindObjectsOfType<GraveSlot>();
        foreach (var g in graves)
            g.SetGuideHighlight(true);
    }

    private void OnInteractPressed()
    {
        // Recoger
        if (carriedFlower == null && targetFlower != null && !targetFlower.IsPicked)
        {
            PickupFlower(targetFlower);
            return;
        }

        // Colocar
        if (carriedFlower != null && targetGrave != null)
        {
            TryPlace(targetGrave);
            return;
        }
    }

    private void PickupFlower(FlowerPickup flower)
    {
        carriedFlower = flower;
        carriedFlower.SetPicked(true);

        if (carryPoint != null && gameManager.flowerPlacedPrefab != null)
        {
            carriedVisual = Instantiate(gameManager.flowerPlacedPrefab, carryPoint);
            carriedVisual.transform.localPosition = Vector3.zero;
            carriedVisual.transform.localRotation = Quaternion.identity;
        }

        ui.Toast("🌸 Flor recogida");
    }

    private void TryPlace(GraveSlot grave)
    {
        if (!grave.isValidGrave)
        {
            ui.Toast("❌ Tumba incorrecta");
            return;
        }
        if (grave.HasFlower)
        {
            ui.Toast("⚠️ Ya tiene flor");
            return;
        }

        if (grave.TryPlaceFlower(gameManager.flowerPlacedPrefab))
        {
            carriedFlower = null;
            if (carriedVisual) Destroy(carriedVisual);

            ui.Toast("✅ Flor colocada");
            gameManager.NotifyFlowerPlaced();
        }
        else
        {
            ui.Toast("⚠️ No se pudo colocar");
        }
    }
}

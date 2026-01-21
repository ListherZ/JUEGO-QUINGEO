using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractorMobile : MonoBehaviour
{
    [Header("Refs")]
    public Camera playerCamera;
    public CinemachineCamera virtualCamera;
    public Transform carryPoint; // Empty delante del player
    public CemeteryMinigameManager gameManager;
    public MinigameUI ui;

    [Header("Raycast")]
    public float interactDistance = 3f;
    public LayerMask interactMask;

    [Header("Guidance")]
    [SerializeField] private bool guideAllValidGraves = false;

    private FlowerPickup targetFlower;
    private GraveSlot targetGrave;

    private FlowerPickup carriedFlower;
    private GameObject carriedVisual;

    private FlowerPickup lastFlowerHL;
    private GraveSlot lastGraveAimHL;

    private GraveSlot[] allGraves;
    private Transform rayOrigin;

    private void Start()
    {
        if (virtualCamera != null)
        {
            rayOrigin = virtualCamera.transform;
        }
        else
        {
            if (playerCamera == null && Camera.main != null)
            {
                playerCamera = Camera.main;
            }

            rayOrigin = playerCamera != null ? playerCamera.transform : transform;
        }

        allGraves = FindObjectsByType<GraveSlot>(FindObjectsSortMode.None);

        if (ui != null && ui.interactButton != null)
        {
            ui.interactButton.onClick.RemoveAllListeners();
            ui.interactButton.onClick.AddListener(OnInteractPressed);
        }

        if (ui != null)
        {
            ui.SetCarry(false);
            ui.SetInteract(false, "");
            ui.SetInstruction("Busca una flor");
        }
    }

    private void Update()
    {
        if (gameManager != null && gameManager.IsCompleted)
        {
            if (ui != null)
            {
                ui.SetInteract(false, "");
            }
            return;
        }

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

        Transform origin = rayOrigin != null ? rayOrigin : transform;
        Ray ray = new Ray(origin.position, origin.forward);
        int mask = interactMask.value == 0 ? ~0 : interactMask.value;
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, mask, QueryTriggerInteraction.Collide))
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
        if (ui == null) return;

        ui.SetCarry(carriedFlower != null);

        // Estado 1: no llevo flor
        if (carriedFlower == null)
        {
            if (targetFlower != null && !targetFlower.IsPicked)
            {
                ui.SetInstruction("Toca el botón para recoger la flor");
                ui.SetInteract(true, "Recoger flor");
            }
            else
            {
                ui.SetInstruction("Busca una flor");
                ui.SetInteract(false, "");
            }
            return;
        }

        // Estado 2: llevo flor
        if (targetGrave == null)
        {
            ui.SetInstruction("Ve a una tumba correcta y coloca la flor");
            ui.SetInteract(false, "");
            return;
        }

        // Estado 3: llevo flor y apunto a tumba
        if (!targetGrave.isValidGrave)
        {
            ui.SetInstruction("Esa tumba no es válida. Busca una tumba correcta");
            ui.SetInteract(true, "Tumba incorrecta");
            return;
        }

        if (targetGrave.HasFlower)
        {
            ui.SetInstruction("Esa tumba ya tiene flor. Busca otra");
            ui.SetInteract(true, "Ocupada");
            return;
        }

        ui.SetInstruction("Toca para colocar la flor");
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
            if (allGraves == null) return;
            foreach (var g in allGraves) g.SetGuideHighlight(false);
            return;
        }

        if (allGraves == null) return;

        if (!guideAllValidGraves)
        {
            foreach (var g in allGraves) g.SetGuideHighlight(false);
            if (targetGrave != null) targetGrave.SetGuideHighlight(true);
            return;
        }

        foreach (var g in allGraves)
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

        if (ui != null) ui.Toast("Flor recogida");
    }

    private void TryPlace(GraveSlot grave)
    {
        if (!grave.isValidGrave)
        {
            if (ui != null) ui.Toast("Tumba incorrecta");
            return;
        }
        if (grave.HasFlower)
        {
            if (ui != null) ui.Toast("Ya tiene flor");
            return;
        }

        if (grave.TryPlaceFlower(gameManager.flowerPlacedPrefab))
        {
            carriedFlower = null;
            if (carriedVisual) Destroy(carriedVisual);

            if (ui != null) ui.Toast("Flor colocada");
            gameManager.NotifyFlowerPlaced();
        }
        else
        {
            if (ui != null) ui.Toast("No se pudo colocar");
        }
    }
}

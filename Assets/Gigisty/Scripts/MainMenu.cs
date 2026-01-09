using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuCanvasSwitcher : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject levelSelectCanvas;

    [Header("Botones principales")]
    [SerializeField] private Button playButton;   // Jugar
    [SerializeField] private Button backButton;   // Volver al menú

    [Header("Botones de niveles y escenas")]
    [SerializeField] private LevelButtonData[] levelButtons;

    [Header("Inicio")]
    [SerializeField] private bool startInMainMenu = true;

    private void Awake()
    {
        ValidateReferences();
        SetupMainButtons();
        SetupLevelButtons();

        if (startInMainMenu)
            ShowMainMenu();
        else
            ShowLevelSelect();
    }

    // ==============================
    // CONFIGURACIÓN
    // ==============================

    private void SetupMainButtons()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(ShowLevelSelect);
        }

        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(ShowMainMenu);
        }
    }

    private void SetupLevelButtons()
    {
        if (levelButtons == null || levelButtons.Length == 0)
        {
            Debug.LogWarning("[MenuCanvasSwitcher] No hay niveles configurados.");
            return;
        }

        foreach (var level in levelButtons)
        {
            if (level.button == null || string.IsNullOrEmpty(level.sceneName))
            {
                Debug.LogWarning("[MenuCanvasSwitcher] Nivel mal configurado.");
                continue;
            }

            level.button.onClick.RemoveAllListeners();
            level.button.onClick.AddListener(() => LoadLevel(level.sceneName));
        }
    }

    // ==============================
    // ACCIONES
    // ==============================

    public void ShowLevelSelect()
    {
        mainMenuCanvas.SetActive(false);
        levelSelectCanvas.SetActive(true);
    }

    public void ShowMainMenu()
    {
        levelSelectCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    private void LoadLevel(string sceneName)
    {
        Debug.Log($"[MenuCanvasSwitcher] Cargando escena: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    // ==============================
    // VALIDACIONES
    // ==============================

    private void ValidateReferences()
    {
        if (mainMenuCanvas == null || levelSelectCanvas == null)
            Debug.LogError("[MenuCanvasSwitcher] Canvas no asignados.");
    }
}

// ==============================
// ESTRUCTURA DE DATOS
// ==============================
[Serializable]
public class LevelButtonData
{
    public Button button;
    public string sceneName;
}

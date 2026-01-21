using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameUI : MonoBehaviour
{
    [Header("HUD")]
    public TMP_Text instructionText;
    public TMP_Text progressText;
    public TMP_Text carryText;
    public TMP_Text timerText;

    [Header("Interact")]
    public Button interactButton;
    public TMP_Text interactButtonText;

    [Header("Toast")]
    public GameObject toastPanel;
    public TMP_Text toastText;
    private float toastTimer;

    [Header("Win")]
    public GameObject winPanel;

    private void Awake()
    {
        if (toastPanel) toastPanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);
    }

    private void Update()
    {
        if (toastTimer > 0f)
        {
            toastTimer -= Time.deltaTime;
            if (toastTimer <= 0f && toastPanel)
                toastPanel.SetActive(false);
        }
    }

    public void SetInstruction(string text)
    {
        if (instructionText) instructionText.text = text;
    }

    public void SetProgress(int placed, int total)
    {
        if (progressText) progressText.text = $"Ofrendas: {placed}/{total}";
    }

    public void SetCarry(bool hasFlower)
    {
        if (!carryText) return;
        carryText.text = hasFlower ? "Llevas: Flor" : "Llevas: Ninguna";
    }

    public void SetTimer(float remainingSeconds)
    {
        if (!timerText) return;
        if (remainingSeconds < 0f) remainingSeconds = 0f;

        int minutes = Mathf.FloorToInt(remainingSeconds / 60f);
        int seconds = Mathf.FloorToInt(remainingSeconds % 60f);
        timerText.text = $"Tiempo: {minutes:00}:{seconds:00}";
    }

    public void SetInteract(bool visible, string label)
    {
        if (interactButton) interactButton.gameObject.SetActive(visible);
        if (interactButtonText) interactButtonText.text = label;
    }

    public void Toast(string msg, float duration = 1.6f)
    {
        if (!toastPanel || !toastText) return;
        toastText.text = msg;
        toastPanel.SetActive(true);
        toastTimer = duration;

        Handheld.Vibrate();
    }

    public void ShowWin(bool show)
    {
        if (winPanel) winPanel.SetActive(show);
    }
}

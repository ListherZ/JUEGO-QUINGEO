using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CemeteryMinigameManager : MonoBehaviour
{
    public int totalFlowers = 10;

    [Header("Timer")]
    [SerializeField] private float totalTimeSeconds = 300f;
    public UnityEvent onTimeUp;

    [Header("Return To Menu")]
    [SerializeField] private string menuSceneName;
    [SerializeField] private float returnDelaySeconds = 3f;

    public GameObject flowerWorldPrefab;
    public GameObject flowerPlacedPrefab;

    public Transform[] flowerSpawnPoints;

    public MinigameUI ui;

    [Header("Completion")]
    public UnityEvent onCompleted;

    public int PlacedCount { get; private set; }
    public bool IsCompleted { get; private set; }

    private float remainingTime;

    private void Start()
    {
        SpawnFlowers();
        PlacedCount = 0;
        IsCompleted = false;
        remainingTime = totalTimeSeconds;
        ui.SetProgress(PlacedCount, totalFlowers);
        ui.SetInstruction("Busca una flor");
        ui.ShowWin(false);
        ui.SetTimer(remainingTime);
    }

    private void Update()
    {
        if (IsCompleted) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime < 0f) remainingTime = 0f;
        ui.SetTimer(remainingTime);

        if (remainingTime <= 0f)
        {
            IsCompleted = true;
            ui.SetInstruction("Fallaste");
            ui.Toast("Se acabó el tiempo");
            ui.ShowWin(false);
            onTimeUp?.Invoke();
            StartCoroutine(ReturnToMenuAfterDelay());
        }
    }

    private void SpawnFlowers()
    {
        int count = Mathf.Min(totalFlowers, flowerSpawnPoints.Length);
        Transform[] points = (Transform[])flowerSpawnPoints.Clone();

        for (int i = 0; i < points.Length; i++)
        {
            int r = Random.Range(i, points.Length);
            (points[i], points[r]) = (points[r], points[i]);
        }

        for (int i = 0; i < count; i++)
            Instantiate(flowerWorldPrefab, points[i].position, points[i].rotation);
    }

    public void NotifyFlowerPlaced()
    {
        if (IsCompleted) return;

        PlacedCount++;
        ui.SetProgress(PlacedCount, totalFlowers);

        if (PlacedCount >= totalFlowers)
        {
            IsCompleted = true;
            ui.SetInstruction("¡Completado!");
            ui.Toast("Recompensa simbólica obtenida");
            ui.ShowWin(true);
            onCompleted?.Invoke();
            StartCoroutine(ReturnToMenuAfterDelay());
        }
        else
        {
            ui.SetInstruction("Busca otra flor");
        }
    }

    private System.Collections.IEnumerator ReturnToMenuAfterDelay()
    {
        if (string.IsNullOrWhiteSpace(menuSceneName)) yield break;
        yield return new WaitForSeconds(returnDelaySeconds);
        SceneManager.LoadScene(menuSceneName);
    }
}

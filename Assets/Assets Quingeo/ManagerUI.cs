using UnityEngine;

public class CemeteryMinigameManager : MonoBehaviour
{
    public int totalFlowers = 10;

    public GameObject flowerWorldPrefab;
    public GameObject flowerPlacedPrefab;

    public Transform[] flowerSpawnPoints;

    public MinigameUI ui;

    public int PlacedCount { get; private set; }

    private void Start()
    {
        SpawnFlowers();
        PlacedCount = 0;
        ui.SetProgress(PlacedCount, totalFlowers);
        ui.SetInstruction("Busca una flor 🌸");
        ui.ShowWin(false);
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
        PlacedCount++;
        ui.SetProgress(PlacedCount, totalFlowers);

        if (PlacedCount >= totalFlowers)
        {
            ui.SetInstruction("¡Completado! 🎉");
            ui.Toast("🏆 Recompensa simbólica obtenida");
            ui.ShowWin(true);
        }
        else
        {
            ui.SetInstruction("Busca otra flor 🌸");
        }
    }
}

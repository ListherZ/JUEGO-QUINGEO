using UnityEngine;

public class PuzzleTilesController : MonoBehaviour
{
    public PuzzleZone zone;
    public RotatingTile[] tiles;

    private bool solved = false;

    public void CheckPuzzle()
    {
        if (solved) return;

        // Revisa las 9 baldosas
        foreach (RotatingTile t in tiles)
        {
            if (!t.IsCorrect())
                return; // al menos una incorrecta → puzzle NO se completa
        }
        Debug.Log("Todas correctas");
        // Si llegó aquí → TODAS están correctas
        solved = true;
        zone.CompletePuzzle();
    }
}

using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    public RotatingTile tile;
    public PuzzleTilesController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tile.RotateTile();
            controller.CheckPuzzle();
        }
    }
}

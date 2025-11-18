using UnityEngine;

public class PuzzleZone : MonoBehaviour
{
    public CameraController2 camController;
    public Vector3 cameraOffsetDuringPuzzle;
    public SimpleDoor door;
    public MonoBehaviour puzzleScript;

    private bool puzzleCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!puzzleCompleted && other.CompareTag("Player"))
        {
            camController.SetPuzzleCamera(cameraOffsetDuringPuzzle);
            puzzleScript.enabled = true;
        }
    }

    public void CompletePuzzle()
    {
        puzzleCompleted = true;
        puzzleScript.enabled = false;
        camController.ResetCamera();
        door.OpenDoor();
    }
}

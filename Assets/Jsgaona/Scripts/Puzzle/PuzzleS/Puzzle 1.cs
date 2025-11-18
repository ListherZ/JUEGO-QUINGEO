using UnityEngine;

public class Puzzle4_Hold : MonoBehaviour
{
    public PuzzleZone zone;
    public float holdTime = 2f;
    private float timer = 0;
    private bool inside = false;

    void Update()
    {
        if (inside)
        {
            timer += Time.deltaTime;
            if (timer >= holdTime)
                zone.CompletePuzzle();
        }
        else
        {
            timer = Mathf.Max(0, timer - Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) inside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) inside = false;
    }
}

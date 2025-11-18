using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public Transform target;          // jugador
    public Vector3 followOffset;      // posición normal de la cámara
    public float smoothSpeed = 6f;

    private Vector3 puzzleOffset;
    private bool inPuzzle = false;

    void LateUpdate()
    {
       target = GameObject.FindGameObjectWithTag("Player").transform;
        if (!inPuzzle)
        {
            Vector3 desiredPos = target.position + followOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothSpeed);
            transform.LookAt(target);
        }
        else
        {
            Vector3 desiredPos = target.position + puzzleOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothSpeed);
        }
    }

    public void SetPuzzleCamera(Vector3 newOffset)
    {
        puzzleOffset = newOffset;
        inPuzzle = true;
    }

    public void ResetCamera()
    {
        inPuzzle = false;
    }
}

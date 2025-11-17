using UnityEngine;

public class SimpleInstantiator : MonoBehaviour {

    [SerializeField] private GameObject prefab;
    [SerializeField] private int instancesPerRow = 10;
    [SerializeField] private float spacing = 2f;

    
    // Metodo de llamada de Unity, se llama una sola vez al iniciar el GameObject
    private void Start() {
        if (prefab == null) return;

        // Ciclo repetitivo que permite generar copias "Clones" del prefab
        for (int x = 0; x < instancesPerRow; x++) {
            for (int z = 0; z < instancesPerRow; z++) {
                Vector3 pos = new Vector3(x * spacing, 0f, z * spacing);
                Instantiate(prefab, pos, Quaternion.identity, transform);
            }
        }
    }
}

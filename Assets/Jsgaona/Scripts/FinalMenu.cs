using UnityEngine;
using UnityEngine.SceneManagement;  // Necesario para manejar las escenas
using UnityEngine.UI;  // Necesario para trabajar con UI (botones, textos, etc.)

public class GameOverManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject gameOverPanel; // Panel que muestra la pantalla de final de juego
    public Button restartButton;     // Botón para volver al menú

    private void Start()
    {
        // Asegúrate de que la pantalla de fin de juego esté oculta al inicio
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(GoToMainMenu);  // Asociar el evento de clic al botón
    }

    // Método para mostrar la pantalla de fin de juego
    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);  // Mostrar el panel de fin de juego
    }

    // Método para cambiar a la escena del menú principal
    void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Cambia "MainMenu" por el nombre de tu escena de menú
    }

    // Detectar colisión con un objeto específico para mostrar la pantalla de fin de juego
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que tocó el collider tiene la etiqueta "Player" (o cualquier otra etiqueta que desees)
        if (other.CompareTag("Player"))
        {
            ShowGameOverScreen();  // Muestra la pantalla de fin de juego
        }
    }
}

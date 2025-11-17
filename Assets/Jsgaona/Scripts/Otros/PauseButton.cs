using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    // Guarda si el juego está pausado

    /// <summary>
    /// Pausa el juego (detiene el tiempo)
    /// </summary>
    public void PauseGame()
    {
       
           Time.timeScale = 0f; // Detiene el tiempo
            Debug.Log("Juego pausado");
        
    }

    /// <summary>
    /// Reanuda el juego (vuelve el tiempo a la normalidad)
    /// </summary>
    public void ResumeGame()
    {
       
            Time.timeScale = 1f; // Restaura el tiempo
            Debug.Log("Juego reanudado");
        
    }

}

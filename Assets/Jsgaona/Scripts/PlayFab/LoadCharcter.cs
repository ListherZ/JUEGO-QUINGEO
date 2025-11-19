using Jsgaona;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Jsgaona
{
    // Se emplea este script para cargar el personaje, desde cualquier escena
    public class LoadCharcter : LobbyManager
    {
        // Referencia del prefab del personaje jugable
        [SerializeField] private GameObject prefabPlayer;

        // Referencia del prefab del canvas
        [SerializeField] private GameObject prefabCanvasUIPlayer;

        // Punto inicial donde aparece el personaje jugable
        [SerializeField] private Transform initialPoint;

        // Referencia de la camara de cinemachine
        [SerializeField] private CinemachineCamera virtualCamera;

        [Header("All enemies game")]
        // Referencia de todos los enemigos dentro del juego
        [SerializeField] private List<EnemyAi> allEnemies;

        [Header("All doors")]
        [SerializeField] private List<Door> allDoors;

        // Referencia del playerCombat
        private PlayerCombat playerCombat;

        // Vidas del jugador
        private int playerLives = 3;

        // UI de Game Over
        [SerializeField] private GameObject gameOverUI;

        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app despues de Awake
        protected override void Start()
        {
            base.Start();

            // Se pregunta si existe la referencia del jugador para destruirlo
            GameObject playerDetected = GameObject.FindGameObjectWithTag("Player");
            if (playerDetected == null)
            {
                playerDetected = Instantiate(prefabPlayer, initialPoint.position, initialPoint.rotation);
            }

            // Se crea la suscripcion para cuando el player muere
            if (playerDetected.TryGetComponent(out PlayerCombat playerCombat))
            {
                this.playerCombat = playerCombat;
                this.playerCombat.onDead += HandlePlayerDeath;
            }

            PlayerHealthView canvas = FindFirstObjectByType<PlayerHealthView>();
            PlayerController playerControl = playerDetected.GetComponent<PlayerController>();
            playerControl.mainCamera = Camera.main.transform;

            if (virtualCamera == null)
            {
                virtualCamera = FindFirstObjectByType<CinemachineCamera>();
            }
            CameraControl cameraControl = playerDetected.GetComponent<CameraControl>();
            if (cameraControl != null && virtualCamera != null)
            {
                cameraControl.SetComponentCameraControl(virtualCamera);
            }

            CameraController cameraController = playerDetected.GetComponent<CameraController>();
            if (cameraController != null && virtualCamera != null)
            {
                cameraController.SetCamera(virtualCamera);
            }

            if (canvas == null)
            {
                GameObject canvasGo = Instantiate(prefabCanvasUIPlayer, Vector3.zero, Quaternion.identity);
                canvas = canvasGo.GetComponent<PlayerHealthView>();

                if (playerControl != null)
                {
                    Joystick[] array = canvas.gameObject.GetComponentsInChildren<Joystick>();
                    playerControl.JoystickController = array[0];
                    cameraController.JoystickController = array[1];
                }

                if (playerDetected.TryGetComponent(out PlayerCombatController playerCombControl))
                {
                    canvas.PlayerCombatController = playerCombControl;
                    playerCombControl.PlayerView = canvas;
                }
            }

            playerDetected.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (virtualCamera != null)
            {
                virtualCamera.Follow = playerDetected.transform;
                virtualCamera.LookAt = playerDetected.transform;
            }

            foreach (EnemyAi currentEnemy in allEnemies)
            {
                currentEnemy.player = playerDetected.transform;
            }

            if (SceneLoadingManager.SceneInstance != null)
            {
                int idConnection = SceneLoadingManager.SceneInstance.DoorId;
                foreach (Door currentDoor in allDoors)
                {
                    if (idConnection == currentDoor.ConexionId)
                    {
                        if (playerControl != null) playerControl.SetPositionAndRotation(currentDoor.transform);
                        break;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (playerCombat != null) playerCombat.onDead -= HandlePlayerDeath;
        }

        // Método que se llama cuando el jugador muere

        private void HandlePlayerDeath()
        {
            playerLives--;  // Reducir las vidas del jugador

            if (playerLives > 0)
            {
                // Recargar la escena actual
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                // Si se quedan sin vidas, mostrar pantalla de Game Over
                ShowGameOverScreen();
            }
        
        }

        // Mostrar pantalla de Game Over
        private void ShowGameOverScreen()
        {
            if (gameOverUI != null)
            {
                gameOverUI.SetActive(true);
            }
        }
    }
}

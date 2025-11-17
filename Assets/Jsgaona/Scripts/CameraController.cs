using UnityEngine;
using Unity.Cinemachine;

namespace Jsgaona {

    public class CameraController : MonoBehaviour {

        [Header("Reference Joystick")]
        [SerializeField] private Joystick joystick;
        [SerializeField] private CinemachineCamera freeLookCam;

        private CinemachineOrbitalFollow orbitalFollow;

        [Header("Sensibilidad")]
        [SerializeField] private float horizontalSensitivity = 2f;
        [SerializeField] private float verticalSensitivity = 2f;

        [Header("Invertir ejes")]
        [SerializeField] private bool invertHorizontal = false;
        [SerializeField] private bool invertVertical = false;

        [Header("Límites Vertical (grados)")]
        [SerializeField] private float verticalMin = -10f;
        [SerializeField] private float verticalMax = 45f;
 
        public Joystick JoystickController  { set { joystick = value; } }
        public void SetCamera( CinemachineCamera camera)
            
        {
            freeLookCam = camera; 
            if (freeLookCam != null)
            {
                orbitalFollow = freeLookCam.GetComponent<CinemachineOrbitalFollow>();
                if (orbitalFollow == null)
                    Debug.LogError("CinemachineOrbitalFollow no está agregado al CinemachineCamera");
            }
            else
            {
                Debug.LogError("freeLookCam no asignada.");
            }
        }

        private void Update()
        {
            if (orbitalFollow == null || joystick == null) return;

            // Multiplicadores de inversión
            float hSign = invertHorizontal ? -1f : 1f;
            float vSign = invertVertical ? -1f : 1f;

            // El joystick actúa como "velocidad" de rotación
            float h = joystick.Horizontal * horizontalSensitivity * Time.deltaTime * hSign;
            float v = joystick.Vertical   * verticalSensitivity   * Time.deltaTime * vSign;

            // Acumular rotación
            orbitalFollow.HorizontalAxis.Value += h;
            orbitalFollow.VerticalAxis.Value   += v;

            // Clamp vertical
            orbitalFollow.VerticalAxis.Value = Mathf.Clamp(
                orbitalFollow.VerticalAxis.Value,
                verticalMin,
                verticalMax
            );
        }
    }
}
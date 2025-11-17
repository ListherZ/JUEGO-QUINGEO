using Unity.Cinemachine;

using UnityEngine;
using System.Collections;

namespace Jsgaona{ 

    // Este script permite gestionar el control de la camara del personaje (Ruido)
    public class CameraControl : MonoBehaviour {

        [Header("Adjust noise")]
        // Amplitud inicial para la camara
        [SerializeField] private float initialAmplitude = 2.0f;

        // Duracion de turbulencia o agitacion de camara
        [SerializeField] private float shakeDuration = 0.25f;


        // Si es verdadero, la camara se encuentra agitandose, caso contrario no
        private bool shakeActive;

        // Referencia del componente de turbulencia de la camara virtual de cinemachine
        public CinemachineBasicMultiChannelPerlin CamCin { get; set; }


        // Metodo de llamada de Unity, se llama al iniciar el aplicativo
        // Se configura e instancia los componentes necesarios para el funcionamiento del script
        public void SetComponentCameraControl(CinemachineCamera cinVirCam){
            var perlin = 
                cinVirCam.GetCinemachineComponent(CinemachineCore.Stage.Aim) as CinemachineBasicMultiChannelPerlin
            ?? cinVirCam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineBasicMultiChannelPerlin;
        }

        // Metodo que permite agitar la camara
        public void ShakeCamera(){
            if(!shakeActive) StartCoroutine(WaitTime());
        }

        // Coroutina que permite agitar la camara
        private IEnumerator WaitTime(){
            shakeActive = true;
            //CamCin.m_AmplitudeGain = initialAmplitude;
            float elapsed = 0.0f;
            // Disminuir la amplitud de manera gradual
            while (elapsed < shakeDuration) {
                elapsed += Time.deltaTime;
                // Interpolacion de la amplitud desde el valor inicial hasta cero
                //CamCin.m_AmplitudeGain = Mathf.Lerp(initialAmplitude, 0.0f, elapsed/shakeDuration);
                yield return null; // Esperar al siguiente frame
            }
            // Asegurarse de que la amplitud este en cero al final
            //CamCin.m_AmplitudeGain = 0.0f;
            shakeActive = false;
        }
    }
}
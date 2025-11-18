using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;
using TMPro;

namespace Jsgaona
{
    public class PlayFabStatsLoader : MonoBehaviour
    {
        [Header("UI")]
        public TMP_Text txtScore;
  

        private void Start()
        {
            // Cargar datos desde la cuenta PlayFab
            CargarDatosJugador();

            // Escuchar cambios de moneda
            PlayerCurrencyManager.Instance.OnCurrencyChanged += UpdateUIInstant;
        }

        private void OnDestroy()
        {
            if (PlayerCurrencyManager.Instance != null)
                PlayerCurrencyManager.Instance.OnCurrencyChanged -= UpdateUIInstant;
        }

        public void CargarDatosJugador()
        {
            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
                result =>
                {
                    int souls = 0;
                   

                    foreach (var stat in result.Statistics)
                    {
                        switch (stat.StatisticName)
                        {
                            case "Souls": souls = stat.Value; break;

                        }
                    }

                    // Enviar al Manager persistente
                    PlayerCurrencyManager.Instance.souls = souls;
                    

                    // Actualizar UI
                    UpdateUIInstant();
                },
                error =>
                {
                    Debug.LogError("❌ Error cargando datos: " + error.GenerateErrorReport());
                });
        }

        private void UpdateUIInstant()
        {
            txtScore.text = "Souls: " + PlayerCurrencyManager.Instance.souls;
        
        }
    }
}

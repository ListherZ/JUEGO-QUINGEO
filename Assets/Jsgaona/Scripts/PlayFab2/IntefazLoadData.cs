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
        public TMP_Text txtCoins;
        public TMP_Text txtDiamonds;

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
                    int score = 0;
                    int coins = 0;
                    int diamonds = 0;

                    foreach (var stat in result.Statistics)
                    {
                        switch (stat.StatisticName)
                        {
                            case "Score": score = stat.Value; break;
                            case "Coins": coins = stat.Value; break;
                            case "Diamonds": diamonds = stat.Value; break;
                        }
                    }

                    // Enviar al Manager persistente
                    PlayerCurrencyManager.Instance.score = score;
                    PlayerCurrencyManager.Instance.coins = coins;
                    PlayerCurrencyManager.Instance.diamonds = diamonds;

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
            txtScore.text = "Score: " + PlayerCurrencyManager.Instance.score;
            txtCoins.text = "Coins: " + PlayerCurrencyManager.Instance.coins;
            txtDiamonds.text = "Diamonds: " + PlayerCurrencyManager.Instance.diamonds;
        }
    }
}

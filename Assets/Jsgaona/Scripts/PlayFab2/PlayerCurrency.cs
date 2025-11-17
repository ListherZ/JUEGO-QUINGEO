using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

namespace Jsgaona
{
    public class PlayerCurrencyManager : MonoBehaviour
    {
        public static PlayerCurrencyManager Instance;

        [Header("Datos del jugador")]
        public int score = 0;
        public int coins = 0;
        public int diamonds = 0;

        // Evento para actualizar UI
        public Action OnCurrencyChanged;

        // Autosave
        private bool dataChanged = false;
        private float saveTimer = 0f;
        private float saveInterval = 10f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (dataChanged)
            {
                saveTimer += Time.deltaTime;
                if (saveTimer >= saveInterval)
                {
                    SaveToPlayFab();
                    saveTimer = 0;
                }
            }
        }

        // ==========================
        //   SUMAR MONEDAS
        // ==========================
        public void AddCurrency(CurrencyType type, int amount)
        {
            switch (type)
            {
                case CurrencyType.Score: score += amount; break;
                case CurrencyType.Coin: coins += amount; break;
                case CurrencyType.Diamond: diamonds += amount; break;
            }

            dataChanged = true;
            OnCurrencyChanged?.Invoke();
        }

        // ==========================
        //   GUARDAR DATOS
        // ==========================
        public void SaveToPlayFab()
        {
            dataChanged = false;

            Debug.Log(
                $"📤 <b>GUARDANDO EN PLAYFAB</b>\n" +
                $"   Score: {score}\n" +
                $"   Coins: {coins}\n" +
                $"   Diamonds: {diamonds}"
            );

            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate { StatisticName = "Score", Value = score },
                    new StatisticUpdate { StatisticName = "Coins", Value = coins },
                    new StatisticUpdate { StatisticName = "Diamonds", Value = diamonds }
                }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request,
                result =>
                {
                    Debug.Log("✔️ Datos guardados correctamente en PlayFab.");
                },
                error =>
                {
                    Debug.LogError("❌ Error guardando en PlayFab: " + error.GenerateErrorReport());
                });
        }

        public void SaveNow()
        {
            SaveToPlayFab();
        }

        // ==========================
        //   CARGAR DATOS
        // ==========================
        public void LoadFromPlayFab(Action callback = null)
        {
            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
                result =>
                {
                    foreach (var stat in result.Statistics)
                    {
                        switch (stat.StatisticName)
                        {
                            case "Score": score = stat.Value; break;
                            case "Coins": coins = stat.Value; break;
                            case "Diamonds": diamonds = stat.Value; break;
                        }
                    }

                    Debug.Log(
                        $"📥 <b>DATOS CARGADOS DESDE PLAYFAB</b>\n" +
                        $"   Score: {score}\n" +
                        $"   Coins: {coins}\n" +
                        $"   Diamonds: {diamonds}"
                    );

                    OnCurrencyChanged?.Invoke();
                    callback?.Invoke();
                },
                error =>
                {
                    Debug.LogError("❌ Error cargando stats: " + error.GenerateErrorReport());
                });
        }
    }
}

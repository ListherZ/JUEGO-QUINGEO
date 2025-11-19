using UnityEngine;

namespace Jsgaona
{
    public class CollectibleOrb : MonoBehaviour
    {
        public CurrencyType currencyType;
        public int amount = 1;
        private PlayerCombat playerCombat;
        private void Update()
        {
            playerCombat = FindAnyObjectByType<PlayerCombat>();
            if (playerCombat == null)
            {
                Debug.LogError("No se encontró el componente PlayerCombat en la escena.");
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (PlayerCurrencyManager.Instance == null)
            {
                Debug.LogError("No hay PlayerCurrencyManager en la escena.");
                return;
            }
            playerCombat.refillResource(25);
            // Sumar moneda
            PlayerCurrencyManager.Instance.AddCurrency(currencyType, amount);
               PlayerCurrencyManager.Instance.SaveToPlayFab();
            // Destruir el orbe
            Destroy(gameObject);
        }
    }
}

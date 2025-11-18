using UnityEngine;

namespace Jsgaona
{
    public class CollectibleOrb : MonoBehaviour
    {
        public CurrencyType currencyType;
        public int amount = 1;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (PlayerCurrencyManager.Instance == null)
            {
                Debug.LogError("No hay PlayerCurrencyManager en la escena.");
                return;
            }

            // Sumar moneda
            PlayerCurrencyManager.Instance.AddCurrency(currencyType, amount);
               PlayerCurrencyManager.Instance.SaveToPlayFab();
            // Destruir el orbe
            Destroy(gameObject);
        }
    }
}

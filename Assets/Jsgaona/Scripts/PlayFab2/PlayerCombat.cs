using UnityEngine;

namespace Jsgaona
{
    // Arquitectura M-V-C  <!-- MODELO -->
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Life and Resoruce")]
        [SerializeField] private int maxLifePoint = 250;
        [SerializeField] private int maxResourcePoint = 100;

        [Header("Skill")]
        [SerializeField] private GameObject nullPulse;
        [SerializeField] private int cost;
        [SerializeField] private float cooldown;

        [Header("Player effects")]
        [SerializeField] private ParticleSystem blood;

        public delegate void OnHealthChanged(int amount, int maxLifePoint);
        public event OnHealthChanged HealthChanged;

        public delegate void OnResourceChanged(int amount, int maxResourcePoint, float cooldown);
        public event OnResourceChanged ResourceChanged;

        public delegate void OnDeadActive();
        public OnDeadActive onDead;

        private bool isAlive = true;
        private int currentLifePoint;
        private int currentResourcePoint;
        public Animator animController;
        private PlayerController playerController;
        private CameraControl cameraControl;

        public int MaxLifePoint => maxLifePoint;
        public int MaxResourcePoint => maxResourcePoint;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            cameraControl = GetComponent<CameraControl>();
        }

        private void Start()
        {
            currentLifePoint = maxLifePoint;
            currentResourcePoint = maxResourcePoint;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C)) NullPulse();
        }

        public void NullPulse()
        {
            if (nullPulse != null && currentResourcePoint >= cost)
            {
                //animController.SetTrigger("hack");
                nullPulse.SetActive(true);
                nullPulse.GetComponent<ParticleSystem>().Play();
                currentResourcePoint -= cost;
                ResourceChanged?.Invoke(currentResourcePoint, maxResourcePoint, cooldown);
            }
        }

        public void Heal(int amount)
        {
            if (!isAlive) return;

            currentLifePoint += amount;
            if (currentLifePoint > maxLifePoint)
                currentLifePoint = maxLifePoint;

            HealthChanged?.Invoke(currentLifePoint, maxLifePoint);
        }
        public void refillResource(int amount)
        {
            currentResourcePoint += amount;
            if (currentResourcePoint > maxResourcePoint)
                currentResourcePoint = maxResourcePoint;
            ResourceChanged?.Invoke(currentResourcePoint, maxResourcePoint, cooldown);
        }
        public void TakeDamage(int amount)
        {
            if (!isAlive) return;

            currentLifePoint -= amount;

            Debug.Log("Daño recibido por PlayerCombat: " + amount + " | Vida actual: " + currentLifePoint);

            //cameraControl.ShakeCamera();

            if (currentLifePoint <= 0)
            {
                currentLifePoint = 0;
                isAlive = false;
                playerController.ManagerMovement(false);
                onDead?.Invoke();
            }

            if (blood != null) blood.Play();

            HealthChanged?.Invoke(currentLifePoint, maxLifePoint);
        }
    }
}

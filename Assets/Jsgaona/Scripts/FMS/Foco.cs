using System.Collections;

using UnityEngine;
namespace Jsgaona
{
    public class Focus : MonoBehaviour, IHackeable
    {
        private Light lightFocus;
        public bool ItsHacked { get; set; }
        public float TimeStopMotion { get; set; }
        private void Awake()
        {
            lightFocus = GetComponent<Light>();
        }
        public void Hack(float timeHack)
        {
            if (!ItsHacked) StartCoroutine(TimeHack(timeHack));
        }

        private IEnumerator TimeHack(float timeHack)

        {
            ItsHacked = true;
            float timer = 0;
            bool on = true;
            float blink = 0.1f;
            while (timer < timeHack)
            {
                on = !on;
                lightFocus.enabled = on;
                yield return new WaitForSeconds(blink);
                timer += blink;
            }
            lightFocus.enabled = true;
            ItsHacked = false;

        }
    }
}


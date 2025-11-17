using UnityEngine;

namespace Assets.DanDanDan.Scripts2
{
    public class RemoteControl : MonoBehaviour
    {
        public Television tv;

        public void PressPower() { Debug.Log("[REMOTE] Botón POWER"); tv?.TogglePower(); }
        public void PressVolUp() { Debug.Log("[REMOTE] Botón VOL+"); tv?.VolumeUp(); }
        public void PressVolDown() { Debug.Log("[REMOTE] Botón VOL-"); tv?.VolumeDown(); }
        public void PressChPlus() { Debug.Log("[REMOTE] Botón CH+"); tv?.NextChannel(); }
        public void PressChMinus() { Debug.Log("[REMOTE] Botón CH-"); tv?.PrevChannel(); }
    }
}

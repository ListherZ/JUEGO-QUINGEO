using UnityEngine;

namespace Assets.DanDanDan.Scripts2
{
    public class Television : MonoBehaviour
    {
        [SerializeField] private bool isOn = true;
        [SerializeField, Range(0, 100)] private int volume = 50;
        [SerializeField] private int channelIndex = 0;

        [SerializeField] private string[] channelNames = { "Noticias", "Películas", "Deportes", "Música", "Documentales" };
        [SerializeField]
        private Color[] channelColors = {
            new Color(0.12f, 0.18f, 0.25f),
            new Color(0.20f, 0.05f, 0.05f),
            new Color(0.05f, 0.20f, 0.08f),
            new Color(0.08f, 0.05f, 0.20f),
            new Color(0.15f, 0.12f, 0.05f)
        };

        [SerializeField] private Renderer screenRenderer;

        private MaterialPropertyBlock mpb;

        void Awake()
        {
            mpb = new MaterialPropertyBlock();
            // Seguridad por si cambias tamaños
            int n = Mathf.Min(channelNames.Length, channelColors.Length);
            if (n <= 0) { channelNames = new[] { "Canal 1" }; channelColors = new[] { Color.gray }; }
            channelIndex = Mathf.Clamp(channelIndex, 0, n - 1);
            Apply();
        }

        public void TogglePower()
        {
            isOn = !isOn;
            Log("[TV] POWER " + (isOn ? "ON" : "OFF"));
            Apply();
        }

        public void VolumeUp()
        {
            if (!isOn) { Log("[TV] Ignorado: TV apagada"); return; }
            volume = Mathf.Clamp(volume + 5, 0, 100);
            Log($"[TV] Volumen ↑ -> {volume}");
            Apply();
        }

        public void VolumeDown()
        {
            if (!isOn) { Log("[TV] Ignorado: TV apagada"); return; }
            volume = Mathf.Clamp(volume - 5, 0, 100);
            Log($"[TV] Volumen ↓ -> {volume}");
            Apply();
        }

        public void NextChannel()
        {
            if (!isOn) { Log("[TV] Ignorado: TV apagada"); return; }
            channelIndex = (channelIndex + 1) % channelNames.Length;
            Log($"[TV] Canal -> {channelNames[channelIndex]}");
            Apply();
        }

        public void PrevChannel()
        {
            if (!isOn) { Log("[TV] Ignorado: TV apagada"); return; }
            channelIndex = (channelIndex - 1 + channelNames.Length) % channelNames.Length;
            Log($"[TV] Canal -> {channelNames[channelIndex]}");
            Apply();
        }

        private void Apply()
        {
            if (screenRenderer == null) return;

            var col = isOn ? channelColors[channelIndex] : Color.black * 0.15f;
            screenRenderer.GetPropertyBlock(mpb);
            mpb.SetColor("_BaseColor", col); // URP/Standard
            mpb.SetColor("_Color", col);     // Built-in
            screenRenderer.SetPropertyBlock(mpb);

            // Estado completo
            Log($"[TV] Estado => Power={(isOn ? "ON" : "OFF")}, Canal={(isOn ? channelNames[channelIndex] : "--")}, Volumen={volume}");
        }

        private void Log(string msg) => Debug.Log(msg);
    }
}
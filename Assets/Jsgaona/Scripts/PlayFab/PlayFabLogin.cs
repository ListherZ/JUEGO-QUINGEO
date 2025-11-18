using TMPro;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using System.Collections.Generic;

namespace Jsgaona
{
    public class PlayFabLogin : MonoBehaviour
    {
        [Header("Formulario Inicio Sesión")]
        [SerializeField] private TMP_InputField txtEmailLogin;
        [SerializeField] private TMP_InputField txtPasswordLogin;

        [Header("Formulario Registro")]
        [SerializeField] private TMP_InputField txtNickUserRegister;
        [SerializeField] private TMP_InputField txtEmailRegister;
        [SerializeField] private TMP_InputField txtPasswordRegister;
        [SerializeField] private TMP_InputField txtConfirmPasswordRegister;

        [Header("Paneles del juego")]
        [SerializeField] private GameObject pnlMainMenu;

        [Header("Recuérdame")]
        [SerializeField] private Toggle toggle;

        // PlayerPrefs keys
        private const string PREF_REMEMBER = "pf_remember";
        private const string PREF_LOGIN_ID = "pf_login_id";
        private const string PREF_PASSWORD = "pf_password";

        private string lastLoginId;
        private string lastPassword;

        private void Awake()
        {
            if (toggle != null)
                toggle.onValueChanged.AddListener(OnToggleRememberChanged);
        }

        private void OnEnable()
        {
            bool remembered = PlayerPrefs.GetInt(PREF_REMEMBER, 0) == 1;

            if (toggle != null)
                toggle.isOn = remembered;

            if (remembered)
            {
                txtEmailLogin.text = PlayerPrefs.GetString(PREF_LOGIN_ID, string.Empty);
                txtPasswordLogin.text = PlayerPrefs.GetString(PREF_PASSWORD, string.Empty);
            }
        }

        private void OnDestroy()
        {
            if (toggle != null)
                toggle.onValueChanged.RemoveListener(OnToggleRememberChanged);
        }

        private void OnToggleRememberChanged(bool isOn)
        {
            PlayerPrefs.SetInt(PREF_REMEMBER, isOn ? 1 : 0);
            PlayerPrefs.Save();

            if (!isOn)
                ClearSavedCredentials();
        }

        //       LOGIN
  
        public void Login()
        {
            string loginId = txtEmailLogin.text;
            string password = txtPasswordLogin.text;

            if (string.IsNullOrEmpty(loginId) || string.IsNullOrEmpty(password))
            {
                Debug.LogWarning("Correo/Usuario o contraseña vacíos.");
                return;
            }

            lastLoginId = loginId;
            lastPassword = password;

            if (loginId.Contains("@"))
                LoginWithEmail(loginId, password);
            else
                LoginWithUsername(loginId, password);
        }

        private void LoginWithUsername(string username, string password)
        {
            var request = new LoginWithPlayFabRequest
            {
                Username = username,
                Password = password
            };

            PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
        }

        private void LoginWithEmail(string email, string password)
        {
            PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest
            {
                Email = email,
                Password = password
            },
            OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log("Login exitoso.");

            bool remember = PlayerPrefs.GetInt(PREF_REMEMBER, 0) == 1;
            if (remember)
                SaveCredentials(lastLoginId, lastPassword);
            else
                ClearSavedCredentials();

            //Cargar estadísticas directamente en el manager global
            PlayerCurrencyManager.Instance.LoadFromPlayFab(() =>
            {
                Debug.Log("Datos del jugador cargados en PlayerCurrencyManager.");
            });
            // Mostrar el menú principal
            gameObject.SetActive(false);
            if (pnlMainMenu != null)
                pnlMainMenu.SetActive(true);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError("Error al iniciar sesión: " + error.GenerateErrorReport());
        }

    
        //       REGISTRO
    
        public void Register()
        {
            string nick = txtNickUserRegister.text;
            string email = txtEmailRegister.text;
            string password = txtPasswordRegister.text;
            string confirm = txtConfirmPasswordRegister.text;

            if (string.IsNullOrEmpty(nick) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm))
            {
                Debug.LogWarning("Campos de registro incompletos.");
                return;
            }

            if (password != confirm)
            {
                Debug.LogWarning("Las contraseñas no coinciden.");
                return;
            }

            RegisterUser(nick, email, password);
        }

        private void RegisterUser(string username, string email, string password)
        {
            var request = new RegisterPlayFabUserRequest
            {
                Username = username,
                Email = email,
                Password = password,
                RequireBothUsernameAndEmail = true
            };

            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            Debug.Log("Registro exitoso. Creando estadísticas iniciales...");

            var stats = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "Score", Value = 0 },
                new StatisticUpdate { StatisticName = "Coins", Value = 0 },
                new StatisticUpdate { StatisticName = "Diamonds", Value = 0 } // 🔥 corregido
            };

            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = stats
            },
            r => Debug.Log(" Estadísticas iniciales creadas."),
            e => Debug.LogError(" Error al crear estadísticas: " + e.GenerateErrorReport()));
        }

        private void OnRegisterFailure(PlayFabError error)
        {
            Debug.LogError("Error en registro: " + error.GenerateErrorReport());
        }


     
        //    RECORDAR DATOS
      
        private void SaveCredentials(string loginId, string password)
        {
            PlayerPrefs.SetString(PREF_LOGIN_ID, loginId);
            PlayerPrefs.SetString(PREF_PASSWORD, password);
            PlayerPrefs.Save();
        }

        private void ClearSavedCredentials()
        {
            if (PlayerPrefs.HasKey(PREF_LOGIN_ID))
                PlayerPrefs.DeleteKey(PREF_LOGIN_ID);

            if (PlayerPrefs.HasKey(PREF_PASSWORD))
                PlayerPrefs.DeleteKey(PREF_PASSWORD);

            PlayerPrefs.Save();
        }
    }
}

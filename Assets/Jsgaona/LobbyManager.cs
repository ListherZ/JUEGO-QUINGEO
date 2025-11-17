using UnityEngine;

namespace Jsgaona {

    // Se emplea este script para gestionar el menu de lobby del juego
    public class LobbyManager2 : MonoBehaviour {

        // Indice de escena que se ha selecionado
        [SerializeField] private int indexScene = 0;


        // Metodo que permite dar inicio al juego
        public void StartGame(){
            Debug.Log(SceneLoadingManager.SceneInstance);
            
                Time.timeScale = 1;
           
            SceneLoadingManager.SceneInstance.LoadGameScene(indexScene);
        }


        // Metodo que permite cerrar el juego
        public void ExitGame(){
            Application.Quit();
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarEscenaDespuesDeTiempo : MonoBehaviour
{
    public float tiempoLimite = 30f; // Establece el tiempo límite en segundos

    void Update()
    {
        // Resta el tiempo transcurrido desde el inicio del juego
        tiempoLimite -= Time.deltaTime;

        // Verifica si se ha alcanzado el tiempo límite
        if (tiempoLimite <= 0f)
        {
            // Reinicia la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

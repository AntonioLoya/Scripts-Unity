using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    public float velocidadCiclo = 30f; // Velocidad del ciclo d�a-noche en segundos
    public Light sol; // Luz direccional que representa el sol/moon
    public Material skyboxDia; // Material de Skybox para el d�a
    public Material skyboxNoche; // Material de Skybox para la noche
    public AudioClip sonidoDia; // Sonido Dia
    public AudioClip sonidoNoche; // Sonido Noche
    public GameObject[] sistemasParticulas; // Asigna los sistemas de part�culas desde el Inspector

    private AudioSource audioSource;
    private bool esDeDia = true;
    private float tiempoActual = 0f;

    public bool EsDeDia()
    {
        return esDeDia;
    }

    private void Start()
    {
        // Crear un objeto separado para manejar los sonidos ambiente
        GameObject sonidoAmbienteObjeto = new GameObject("SonidoAmbiente");
        audioSource = sonidoAmbienteObjeto.AddComponent<AudioSource>();

        // Inicia todos los sistemas de part�culas desactivados
        DesactivarParticulas();
        ReproducirSonidoAmbiente();
    }

    private void Update()
    {
        tiempoActual += Time.deltaTime;

        // Calcula la posici�n del sol/moon
        float anguloRotacion = (tiempoActual / velocidadCiclo) * 360f;
        anguloRotacion = anguloRotacion % 360f; // Asegura que el �ngulo est� en el rango correcto

        sol.transform.rotation = Quaternion.Euler(anguloRotacion, 0f, 0f);

        // Verifica si ha pasado la mitad del ciclo para cambiar el estado del d�a y la noche
        if (anguloRotacion >= 180f)
        {
            tiempoActual = 0f;
            esDeDia = !esDeDia;
            ReproducirSonidoAmbiente();
            ActualizarParticulas();
            ActualizarSkybox();
        }
    }

    private void ReproducirSonidoAmbiente()
    {
        if (esDeDia)
        {
            RenderSettings.skybox = skyboxDia;
            RenderSettings.skybox.SetFloat("_Exposure", 1f); // Ajusta la exposici�n para simular la luz del d�a
            audioSource.clip = sonidoDia;
        }
        else
        {
            RenderSettings.skybox = skyboxNoche;
            RenderSettings.skybox.SetFloat("_Exposure", 0.1f); // Ajusta la exposici�n para simular la luz de la noche
            audioSource.clip = sonidoNoche;
        }

        audioSource.Play();
    }

    private void DesactivarParticulas()
    {
        foreach (var sistema in sistemasParticulas)
        {
            if (sistema != null)
            {
                sistema.SetActive(false);
            }
        }
    }

    private void ActualizarParticulas()
    {
        if (esDeDia)
        {
            DesactivarParticulas();
        }
        else
        {
            foreach (var sistema in sistemasParticulas)
            {
                if (sistema != null)
                {
                    sistema.SetActive(true);
                }
            }
        }
    }

    private void ActualizarSkybox()
    {
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonido : MonoBehaviour
{
    // Referencia al componente AudioSource que emitirá el sonido
    public AudioSource quienEmite;

    // El archivo de audio que se reproducirá
    public AudioClip elArchivoQueBaje;

    // El volumen del sonido
    public float Volumen;

    // Variable para rastrear si el sonido ya se reprodujo para evitar repeticiones
    private bool seReprodujo = false;

    // Almacena los volúmenes originales de otros AudioSource
    private Dictionary<AudioSource, float> originalVolumes = new Dictionary<AudioSource, float>();

    // Método que se llama cuando otro Collider entra en el Collider asociado a este GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el sonido aún no se ha reproducido
        if (!seReprodujo)
        {
            // Silencia los demás AudioSource en la escena
            MuteOtherAudioSources();

            // Reproduce el archivo de audio usando PlayOneShot para evitar interrupciones
            quienEmite.PlayOneShot(elArchivoQueBaje, Volumen);

            // Marca que el sonido se ha reproducido para evitar repeticiones
            seReprodujo = true;

            // Restaura el volumen después de que termine el sonido
            StartCoroutine(RestoreOtherAudioSourceVolumes());
        }
    }

    // Función para silenciar todos los demás AudioSource en la escena
    private void MuteOtherAudioSources()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != quienEmite)
            {
                // Almacena el volumen original
                originalVolumes[audioSource] = audioSource.volume;

                // Silencia el audio
                audioSource.volume = 0f;
            }
        }
    }

    // Corrutina para restaurar el volumen original de los AudioSource después de un tiempo
    private IEnumerator RestoreOtherAudioSourceVolumes()
    {
        // Espera el tiempo que dura el sonido
        yield return new WaitForSeconds(elArchivoQueBaje.length);

        // Restaura el volumen original de los demás AudioSource
        foreach (AudioSource audioSource in originalVolumes.Keys)
        {
            audioSource.volume = originalVolumes[audioSource]; // Restaura el volumen original
        }
    }
}

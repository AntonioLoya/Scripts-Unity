using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recorridodeobjeto : MonoBehaviour
{
    public Transform jugador; // Referencia al jugador
    public float velocidad = 5.0f; // Velocidad de movimiento del duende
    private CicloDiaNoche cicloDiaNoche; // Referencia al script de ciclo día-noche
    private Vector3 posicionInicial; // Posición inicial del duende
    private Renderer duendeRenderer; // Referencia al renderer del duende
    public AudioSource audioSource; // Fuente de audio
    public AudioClip[] audioClips; // Array de clips de audio para el duende
    public AudioClip sonidoAparicion; // Sonido especial para cuando el duende aparece
    public AudioClip sonidoDesaparicion; // Sonido especial para cuando el duende desaparece
    private int indiceAudioActual = 0; // Índice del audio actual
    private bool sonidoReproduciendo = false; // Controla si el sonido está reproduciéndose
    private bool esDeNocheAnteriormente = false; // Para controlar la transición entre día y noche
    private bool sonidoAparicionReproducido = false; // Para controlar si ya se reprodujo el sonido de aparición
    private bool sonidoDesaparicionReproducido = false; // Para controlar si ya se reprodujo el sonido de desaparición

    // Start is called before the first frame update
    void Start()
    {
        // Establecer la posición inicial del duende
        posicionInicial = transform.position;

        // Obtener la referencia al script CicloDiaNoche
        cicloDiaNoche = FindObjectOfType<CicloDiaNoche>();

        // Buscar el Renderer en este objeto o en sus hijos
        duendeRenderer = GetComponentInChildren<Renderer>();

        if (duendeRenderer == null)
        {
            Debug.LogWarning("No se encontró un componente Renderer en el duende o sus hijos.");
        }

        if (jugador == null)
        {
            jugador = Camera.main.transform; // Si el jugador no está asignado, intenta asignar la cámara principal
            Debug.LogWarning("No se ha asignado el jugador.");
        }

        if (audioSource == null)
        {
            // Añadir un AudioSource si no está asignado
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Comprobar si ya es de noche al iniciar la escena
        if (!cicloDiaNoche.EsDeDia())
        {
            esDeNocheAnteriormente = true;
            ReproducirSonidoAparicion();
        }
    }

    // Update is called once per frame
    void Update()
    {
         Debug.Log("Posición del jugador: " + jugador.position);
         
        // Verifica si es de noche
        if (!cicloDiaNoche.EsDeDia())
        {
            // Si acaba de hacerse de noche, reproduce el sonido de aparición y resetea la reproducción de audio
            if (!esDeNocheAnteriormente)
            {
                sonidoAparicionReproducido = false;
                ReproducirSonidoAparicion();
                esDeNocheAnteriormente = true;
                indiceAudioActual = 0; // Empieza con el primer audio
            }

            // Activar el duende si tiene Renderer
            if (duendeRenderer != null)
            {
                duendeRenderer.enabled = true;
            }

            // Si el audio actual terminó, reproduce el siguiente
            if (!audioSource.isPlaying && sonidoReproduciendo)
            {
                ReproducirSiguienteAudio();
            }

            // Seguir al jugador
            SeguirJugador();
        }
        else
        {
            // Si acaba de hacerse de día, reproduce el sonido de desaparición y detiene la reproducción de sonido
            if (esDeNocheAnteriormente)
            {
                sonidoDesaparicionReproducido = false;
                ReproducirSonidoDesaparicion();
                esDeNocheAnteriormente = false;
                sonidoReproduciendo = false;
            }

            // Desactivar el duende si tiene Renderer
            if (duendeRenderer != null)
            {
                duendeRenderer.enabled = false;
            }

            // El duende regresa a su posición inicial
            transform.position = posicionInicial;
        }
    }

    private void SeguirJugador()
    {
        if (jugador == null)
        {
            return;
        }

        // Calcula la dirección hacia el jugador
        Vector3 direccion = jugador.position - transform.position;

        // Proyecta la dirección en el plano XZ para evitar inclinaciones no deseadas
        direccion.y = 0f;

        // Si la distancia al jugador es mayor que un pequeño umbral, sigue al jugador
        if (direccion.magnitude > 0.1f)
        {
            // Orienta al duende hacia el jugador
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5.0f);

            // Mueve al duende hacia el jugador
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        }
    }

    private void ReproducirSiguienteAudio()
    {
        // Reproducir el siguiente audio en la lista de clips
        if (audioClips.Length > 0 && indiceAudioActual < audioClips.Length)
        {
            audioSource.clip = audioClips[indiceAudioActual];
            audioSource.Play();
            sonidoReproduciendo = true;

            // Incrementar el índice para la próxima reproducción
            indiceAudioActual++;

            // Si llegamos al final de la lista, volver al inicio
            if (indiceAudioActual >= audioClips.Length)
            {
                indiceAudioActual = 0;
            }
        }
    }

    private void ReproducirSonidoAparicion()
    {
        if (!sonidoAparicionReproducido && sonidoAparicion != null)
        {
            audioSource.clip = sonidoAparicion;
            audioSource.Play();
            sonidoReproduciendo = true;
            sonidoAparicionReproducido = true;
        }
        else
        {
            ReproducirSiguienteAudio();
        }
    }

    private void ReproducirSonidoDesaparicion()
    {
        if (!sonidoDesaparicionReproducido && sonidoDesaparicion != null)
        {
            audioSource.clip = sonidoDesaparicion;
            audioSource.Play();
            sonidoDesaparicionReproducido = true;
        }
        else
        {
            audioSource.Stop(); // Detener cualquier otro sonido
        }
    }
}

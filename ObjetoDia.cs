using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoDia : MonoBehaviour
{
    private CicloDiaNoche cicloDiaNoche; // Referencia al script de ciclo día-noche
    private Renderer duendeRenderer; // Referencia al renderer del duende
    public AudioSource audioSource; // Fuente de audio
    public AudioClip[] audioClips; // Array de clips de audio para el duende
    public Transform jugador; // Referencia al jugador
    public float rangoEscucha = 10.0f; // Rango de escucha del duende
    public float volumenMaximo = 1.0f; // Volumen máximo
    public float volumenMinimo = 0.0f; // Volumen mínimo
    private bool sonidoReproduciendo = false; // Controla si el sonido está reproduciéndose

    void Start()
    {
        // Obtener la referencia al script CicloDiaNoche
        cicloDiaNoche = FindObjectOfType<CicloDiaNoche>();

        // Buscar el Renderer en este objeto o en sus hijos
        duendeRenderer = GetComponentInChildren<Renderer>();

        if (audioSource == null)
        {
            // Añadir un AudioSource si no está asignado
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Asignar el jugador si no está asignado
        if (jugador == null)
        {
            jugador = GameObject.FindGameObjectWithTag("Player").transform; // Asegúrate de que el jugador tenga la etiqueta "Player"
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Verifica si es de día
        if (cicloDiaNoche.EsDeDia())
        {
            // Activar el duende si tiene Renderer
            if (duendeRenderer != null)
            {
                duendeRenderer.enabled = true;
            }

            // Comprobar la distancia al jugador
            float distancia = Vector3.Distance(transform.position, jugador.position);
            if (distancia <= rangoEscucha)
            {
                // Reproducir sonido aleatorio si no se está reproduciendo
                if (!sonidoReproduciendo)
                {
                    ReproducirSonidoAleatorio();
                }

                // Ajustar el volumen del audio según la distancia
                float volumen = Mathf.Lerp(volumenMinimo, volumenMaximo, 1 - (distancia / rangoEscucha));
                audioSource.volume = volumen;
            }
            else
            {
                // Detener el sonido si el jugador está fuera de rango
                if (sonidoReproduciendo)
                {
                    audioSource.Stop();
                    sonidoReproduciendo = false;
                }
            }
        }
        else
        {
            // Desactivar el duende si tiene Renderer
            if (duendeRenderer != null)
            {
                duendeRenderer.enabled = false;
            }

            // Detener sonido si es de noche
            if (sonidoReproduciendo)
            {
                audioSource.Stop();
                sonidoReproduciendo = false;
            }
        }
    }

    private void ReproducirSonidoAleatorio()
    {
        // Reproducir un sonido aleatorio de la lista de clips
        if (audioClips.Length > 0)
        {
            int indiceAleatorio = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[indiceAleatorio];
            audioSource.Play();
            sonidoReproduciendo = true;

            // Reiniciar el estado de reproducción después de que termine el clip
            StartCoroutine(EsperarFinDelSonido(audioSource.clip.length));
        }
    }

    private IEnumerator EsperarFinDelSonido(float duracion)
    {
        yield return new WaitForSeconds(duracion);
        sonidoReproduciendo = false; // Permitir reproducir otro sonido después de que termine
    }
}

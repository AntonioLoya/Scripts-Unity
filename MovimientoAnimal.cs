using UnityEngine;

public class MovimientoAnimal : MonoBehaviour
{
    public Transform[] puntosMovimiento; // Array que contiene los puntos a los que se mover� el animal
    public float velocidad = 5.0f; // Velocidad de movimiento del animal
    private int indicePuntoActual = 0; // �ndice del punto al que se est� moviendo

    private CicloDiaNoche cicloDiaNoche; // Referencia al script de ciclo d�a-noche
    private Vector3 posicionInicial; // Posici�n inicial del animal

    private void Start()
    {
        // Inicialmente, el animal se mover� hacia el primer punto
        if (puntosMovimiento.Length > 0)
        {
            posicionInicial = puntosMovimiento[0].position;
            transform.position = posicionInicial;
        }

        // Obt�n la referencia al script CicloDiaNoche
        cicloDiaNoche = FindObjectOfType<CicloDiaNoche>();
    }

    private void Update()
    {
        // Verifica si hay puntos a los que moverse
        if (puntosMovimiento.Length == 0)
        {
            Debug.LogWarning("No se han asignado puntos de movimiento.");
            return;
        }

        // Mueve al animal solo si es de noche
        if (!cicloDiaNoche.EsDeDia())
        {
            MoverAnimal();
        }
        else
        {
            // Si es de d�a, el animal se queda en la posici�n inicial
            transform.position = posicionInicial;
        }
    }

    private void MoverAnimal()
    {
        // Calcula la direcci�n hacia el punto actual
        Vector3 direccion = puntosMovimiento[indicePuntoActual].position - transform.position;

        // Proyecta la direcci�n en el plano XZ para evitar inclinaciones no deseadas
        direccion.y = 0f;

        // Si el animal no est� en el punto actual, avanza hacia �l
        if (direccion.magnitude > 0.1f)
        {
            // Orienta al animal hacia el siguiente punto
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5.0f);

            // Mueve al animal hacia el punto
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        }
        else
        {
            // Si el animal alcanza el punto, pasa al siguiente punto en el array
            indicePuntoActual = (indicePuntoActual + 1) % puntosMovimiento.Length;
        }
    }
}

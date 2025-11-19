using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SequencePuzzle : MonoBehaviour
{
    [Header("Referencias")]
    public List<SequenceCube> allCubes;        // Lista de TODOS los cubos de la pista
    public Transform player;                   // Jugador
    public Transform startPoint;               // Punto inicial donde vuelve el jugador
    public TextMeshProUGUI timerText;                     // UI del tiempo numerico

    [Header("Secuencia")]
    public bool useManualSequence = false;     // Si está en TRUE usa la secuencia manual
    public List<int> manualSequence;           // IDs de los cubos manuales
    public int sequenceLength = 4;             // Largo de secuencia aleatoria

    private List<int> sequence = new List<int>(); // Secuencia final usada
    private int currentIndex = 0;                 // En qué paso de la secuencia va el jugador
    private bool playerTurn = false;              // Si el jugador puede moverse o no

    [Header("Tiempo")]
    public float maxTime = 20f;                 // Segundos totales
    private float currentTime;                  // Tiempo restante
    private bool timerRunning = false;

    void Start()
    {
        GenerateSequence();      // Crea la secuencia (manual o aleatoria)
        StartCoroutine(ShowSequence());
        currentTime = maxTime;
        timerRunning = true;
    }

    void Update()
    {
        if (timerRunning)
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                currentTime = 0;
                timerRunning = false;
                WrongStep(); // Si el tiempo llega a 0, reinicia todo
            }

            timerText.text = currentTime.ToString("F0"); // Tiempo NUMÉRICO
        }
    }

    // ---------------------------------------------
    // GENERAR SECUENCIA
    // ---------------------------------------------
    void GenerateSequence()
    {
        sequence.Clear();

        if (useManualSequence)
        {
            // Usa la secuencia que tú coloques en el Inspector
            sequence.AddRange(manualSequence);
        }
        else
        {
            // Genera aleatoria
            for (int i = 0; i < sequenceLength; i++)
            {
                int index = Random.Range(0, allCubes.Count);
                sequence.Add(index);
            }
        }
    }

    // ---------------------------------------------
    // MOSTRAR SECUENCIA (bloquea al jugador)
    // ---------------------------------------------
    IEnumerator ShowSequence()
    {
        playerTurn = false;   // bloquea movimiento/interacción

        // Resetea colores antes
        foreach (var cube in allCubes)
            cube.ResetColor();

        yield return new WaitForSeconds(1f);

        // Muestra secuencia uno por uno
        foreach (int id in sequence)
        {
            allCubes[id].Flash();  // ilumina cubo
            yield return new WaitForSeconds(1f);
        }

        playerTurn = true;     // Ahora sí puede jugar
        currentIndex = 0;      // Empieza desde el primer cubo
    }

    // ---------------------------------------------
    // LLAMADO POR CADA CUBO CUANDO EL JUGADOR LO PISA
    // ---------------------------------------------
    public void CubeStepped(int cubeID)
    {
        if (!playerTurn) return;  // si todavía se está mostrando la secuencia → ignorar

        if (sequence[currentIndex] == cubeID)
        {
            // Correcto
            allCubes[cubeID].SetCorrect();

            currentIndex++;

            // Completó toda la secuencia
            if (currentIndex >= sequence.Count)
            {
                Debug.Log("GANASTE LA SECUENCIA");
                timerRunning = false;
            }
        }
        else
        {
            // Incorrecto → reiniciar
            WrongStep();
        }
    }

    // ---------------------------------------------
    // CUANDO EL JUGADOR FALLA
    // ---------------------------------------------
    void WrongStep()
    {
        Debug.Log("FALLÓ - Reiniciando puzzle");

        currentIndex = 0;
        playerTurn = false;

        // Mueve al jugador al inicio
        player.position = startPoint.position;

        // Reinicia el tiempo
        currentTime = maxTime;

        // Vuelve a mostrar la secuencia
        StartCoroutine(ShowSequence());
    }
}

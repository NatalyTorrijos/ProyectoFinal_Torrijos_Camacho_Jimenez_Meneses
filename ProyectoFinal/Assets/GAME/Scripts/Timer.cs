using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerMinutes;     // UI minutos
    public TextMeshProUGUI timerSeconds;     // UI segundos
    public TextMeshProUGUI timerSeconds100;  // UI centésimas (00–99)

    private float startTime;                 // Tiempo en que inicia
    private float stopTime;                  // Tiempo cuando para
    private float timerTime;                 // Tiempo actual del cronómetro
    private bool isRunning = false;          // ¿Está contando?

    public float StopTime { get => stopTime; set => stopTime = value; }

    void Start()
    {
        TimerStart();    // El temporizador inicia automáticamente
    }

    // Inicia el cronómetro
    public void TimerStart()
    {
        if (!isRunning)
        {
            isRunning = true;
            startTime = Time.time;
        }
    }

    // Detiene el cronómetro
    public void TimerStop()
    {
        if (isRunning)
        {
            isRunning = false;
            stopTime = timerTime;
            Debug.Log("Tiempo detenido: " + stopTime);
        }
    }

    // Reinicia el cronómetro
    public void TimerReset()
    {
        stopTime = 0;
        isRunning = false;
        timerMinutes.text = timerSeconds.text = timerSeconds100.text = "00";
    }

    void Update()
    {
        // Contador: tiempo detenido + tiempo activo
        timerTime = stopTime + (Time.time - startTime);

        int minutesInt = (int)timerTime / 60;
        int secondsInt = (int)timerTime % 60;
        int seconds100Int = (int)((timerTime - (secondsInt + minutesInt * 60)) * 100);

        if (isRunning)
        {
            timerMinutes.text = minutesInt < 10 ? "0" + minutesInt : minutesInt.ToString();
            timerSeconds.text = secondsInt < 10 ? "0" + secondsInt : secondsInt.ToString();
            timerSeconds100.text = seconds100Int < 10 ? "0" + seconds100Int : seconds100Int.ToString();
        }
    }
}

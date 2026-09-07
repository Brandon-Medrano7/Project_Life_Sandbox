using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Configuración de Tiempo")]
    [Tooltip("Cuántos minutos reales dura una hora en el juego")]
    public float realMinutesPerHour = 5f;

    [Header("Tiempo actual")]
    public int currentMinute { get; private set; }
    public int currentHour { get; private set; }
    public int currentDay { get; private set; }
    public DayOfWeek currentDayOfWeek { get; private set; }

    private float timer;

    public static event Action OnMinuteChanged;
    public static event Action OnHourChanged;
    public static event Action OnDayChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentMinute = 0;
        currentHour = 8;
        currentDay = 1;
        currentDayOfWeek = DayOfWeek.Monday;
    }

    private void Update()
    {
        CalculateTime();
    }

    private void CalculateTime()
    {
        float gameSecondsPerRealSecond = 3600f / (realMinutesPerHour * 60f);
        timer += Time.deltaTime * gameSecondsPerRealSecond;

        while (timer >= 60f)
        {
            timer -= 60f;
            AdvanceMinute();
        }
    }

    private void AdvanceMinute()
    {
        currentMinute++;
        OnMinuteChanged?.Invoke();

        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;
            OnHourChanged?.Invoke();

            Debug.Log($"[RELOJ] Hora actual: {currentHour:00}:{currentMinute:00}");

            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;
                AdvanceDayOfWeek();
                OnDayChanged?.Invoke();
            }
        }
    }

    private void AdvanceDayOfWeek()
    {
        currentDayOfWeek = (DayOfWeek)(((int)currentDayOfWeek + 1) % 7);
        Debug.Log($"[RELOJ] Nuevo día: {currentDayOfWeek}. Día {currentDay} en el mundo.");
    }
}
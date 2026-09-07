using System; // <-- Asegúrate de que tenga este using hasta arriba del todo
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // ESTA LÍNEA ES LA QUE FALTA: Es el mensajero que avisa cuando cambia la hora
    public static Action OnHourChanged; 

    // ... (aquí abajo deja todo el código que ya tenías de tu reloj)
}
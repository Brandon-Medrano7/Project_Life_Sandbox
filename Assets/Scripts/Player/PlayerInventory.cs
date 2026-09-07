using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    [Header("Configuración de Carga (Estilo PZ)")]
    public float maxWeightCapacity = 18.0f; // Capacidad máxima de carga en Kg
    public float currentWeight = 0.0f;       // Peso actual acumulado

    // La lista que guardará todas las tarjetas de datos de los ítems que recolectemos
    [Header("Contenido del Inventario")]
    public List<ItemData> inventoryItems = new List<ItemData>();

    private PlayerStats playerStats; // Conexión directa con sus atributos físicos

    private void Start()
    {
        // Buscamos el script de stats en el mismo objeto para calcular la fuerza real
        playerStats = GetComponent<PlayerStats>();
        CalibrateMaxWeight();
    }

    // Optimización: Ajustamos la carga máxima según el rasgo o fuerza del personaje
    public void CalibrateMaxWeight()
    {
        if (playerStats != null)
        {
            // Base de 12 Kg + 2 Kg extras por cada nivel de fuerza muscular que tenga en PC
            maxWeightCapacity = 12.0f + (playerStats.muscleStrength * 2.0f);
        }
    }

    // Función pública para cuando levantemos cosas del mapa
    public bool AddItem(ItemData newItem)
    {
        // 1. Validar si meter el objeto excede la capacidad física actual
        if (currentWeight + newItem.weight > maxWeightCapacity)
        {
            Debug.LogWarning($"[🎒 INVENTARIO LLENO] No puedes cargar '{newItem.itemName}'. ¡Pesa demasiado! ({newItem.weight} Kg)");
            return false; // Bloquea la acción
        }

        // 2. Si hay espacio en la espalda, se agrega a la lista
        inventoryItems.Add(newItem);
        CalculateTotalWeight();
        
        Debug.Log($"[📥 ÍTEM GUARDADO] Recogiste: {newItem.itemName}. Peso actual: {currentWeight}/{maxWeightCapacity} Kg.");
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
            CalculateTotalWeight();
            Debug.Log($"[📤 ÍTEM RETIRADO] Soltaste: {item.itemName}. Peso actual: {currentWeight} Kg.");
        }
    }

    // Optimización para PC: Solo recalculamos el peso cuando la mochila cambia, ahorrando ciclos de CPU
    private void CalculateTotalWeight()
    {
        float calculatedWeight = 0.0f;
        foreach (ItemData item in inventoryItems)
        {
            calculatedWeight += item.weight;
        }
        currentWeight = calculatedWeight;

        // Si el jugador va súper cargado, le mandamos una advertencia (después afectará su estamina)
        if (currentWeight > maxWeightCapacity && playerStats != null)
        {
            Debug.LogWarning("[⚠️ SOBRECARGA] Estás cargando más de lo que tus músculos aguantan. Te moverás más lento.");
        }
    }

    // Función pública para que el jugador consuma comida o bebida de su mochila
    public void ConsumeItem(ItemData item)
    {
        // 1. Verificar si el ítem realmente está en la mochila
        if (!inventoryItems.Contains(item))
        {
            Debug.LogWarning($"[⚠️ ERROR] No tienes '{item.itemName}' en el inventario para consumirlo.");
            return;
        }

        // 2. Verificar que sea algo comestible o bebible
        if (item.type != ItemType.Food && item.type != ItemType.Drink)
        {
            Debug.LogWarning($"[⚠️ ACCIÓN INVÁLIDA] '{item.itemName}' no es un artículo consumible.");
            return;
        }

        // 3. Aplicar los efectos directamente en los stats del jugador
        if (playerStats != null)
        {
            // Restamos el hambre y la sed (reducir el valor en las barras)
            playerStats.hunger -= item.hungerRecovery;
            playerStats.thirst -= item.thirstRecovery;

            // Evitamos que las variables bajen de 0 (el límite perfecto)
            playerStats.hunger = Mathf.Clamp(playerStats.hunger, 0f, 100f);
            playerStats.thirst = Mathf.Clamp(playerStats.thirst, 0f, 100f);

            Debug.Log($"[🍔 CONSUMIDO] Te alimentaste con: {item.itemName}. Hambre reducida en {item.hungerRecovery} puntos.");
        }

        // 4. Sacarlo de la mochila y recalcular el peso de la espalda en PC
        RemoveItem(item);
    }
}
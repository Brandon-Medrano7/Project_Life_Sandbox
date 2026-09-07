using UnityEngine;

// Registramos qué tipos de ítems tendrá nuestro sandbox de PC
public enum ItemType
{
    Food,       // Comida (baja el hambre)
    Drink,      // Bebida (baja la sed)
    Tool,       // Herramientas (llaves, desarmadores)
    CarPart,    // Refacciones pesadas (batería de Bora, llantas, alternadores)
    Weapon      // Armas de defensa
}

// Esta línea mágica nos permitirá crear los ítems con un clic derecho en los menús de Unity
[CreateAssetMenu(fileName = "NuevoItem", menuName = "Sandbox/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Información Básica")]
    public string itemName;
    [TextArea] public string itemDescription;
    public ItemType type;
    public Sprite itemIcon; // La imagen que se verá en el inventario de la PC

    [Header("Logística y Físicas (Estilo PZ)")]
    public float weight = 1.0f; // Peso en Kg. ¡Clave para la sobrecarga en PC!
    public int maxStack = 1;    // Cuántos se enciman en la misma ranura (balas sí, cajas de herramientas no)

    [Header("Efectos al Consumir")]
    public float hungerRecovery = 0f; // Cuánta hambre te quita si te lo comes
    public float thirstRecovery = 0f; // Cuánta sed te quita si te lo bebes
}
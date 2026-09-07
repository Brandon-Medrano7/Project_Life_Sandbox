using UnityEngine;

// Obligamos al objeto a tener un CircleCollider2D para que detecte la cercanía del jugador automáticamente
[RequireComponent(typeof(CircleCollider2D))]
public class ItemPickup : MonoBehaviour
{
    [Header("Datos del Ítems")]
    public ItemData itemSettings; // Aquí arrastraremos la Llave Inglesa o la Lata de Atún

    private SpriteRenderer spriteRenderer;
    private bool isPlayerNearby = false;
    private PlayerInventory playerInventory;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Configuramos el colisionador en automático como un Trigger (fantasma) para optimizar
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.8f; // Rango de cercanía para poder levantar el objeto
    }

    private void Start()
    {
        // Si le asignamos un ítem, cargamos su icono visual en el mapa de forma automática
        if (itemSettings != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = itemSettings.itemIcon;
        }
    }

    private void Update()
    {
        // En PC usamos la tecla 'E' para interactuar al estar cerca
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            TryToPickUp();
        }
    }

    private void TryToPickUp()
    {
        if (playerInventory != null && itemSettings != null)
        {
            // Intentamos meterlo a la mochila del jugador
            bool itemAdded = playerInventory.AddItem(itemSettings);

            // Si el inventario no estaba lleno y lo aceptó, el objeto se destruye del suelo
            if (itemAdded)
            {
                Destroy(gameObject);
            }
        }
    }

    // Unity detecta automáticamente cuando el jugador entra al rango
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                isPlayerNearby = true;
                Debug.Log($"[💡 INTERACCIÓN] Presiona 'E' para recoger: {itemSettings.itemName}");
            }
        }
    }

    // Rompe la conexión si el jugador se aleja caminando
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            playerInventory = null;
            Debug.Log("[💡 INTERACCIÓN] Te alejaste del objeto.");
        }
    }
}
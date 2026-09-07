using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    [Header("Componentes")]
    public BoxCollider2D solidCollider;
    public CircleCollider2D interactionTrigger;
    public SpriteRenderer doorVisual;

    [Header("Configuración")]
    public bool isOpen = false;
    public float openRotationZ = 90f;

    private bool isPlayerNearby = false;

    private void Awake()
    {
        if (doorVisual == null)
        {
            doorVisual = GetComponent<SpriteRenderer>();
        }

        if (solidCollider == null)
        {
            solidCollider = GetComponent<BoxCollider2D>();
        }

        if (interactionTrigger == null)
        {
            interactionTrigger = GetComponent<CircleCollider2D>();
        }

        if (interactionTrigger != null)
        {
            interactionTrigger.isTrigger = true;
            interactionTrigger.radius = 1.2f;
        }

        ApplyDoorState();
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        ApplyDoorState();

        if (isOpen)
        {
            Debug.Log("[PUERTA] Abriste la puerta.");
        }
        else
        {
            Debug.Log("[PUERTA] Cerraste la puerta.");
        }
    }

    private void ApplyDoorState()
    {
        if (solidCollider != null)
        {
            solidCollider.enabled = !isOpen;
        }

        transform.rotation = isOpen
            ? Quaternion.Euler(0f, 0f, openRotationZ)
            : Quaternion.Euler(0f, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("[INTERACCIÓN] Presiona E para usar la puerta.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("[INTERACCIÓN] Te alejaste de la puerta.");
        }
    }
}
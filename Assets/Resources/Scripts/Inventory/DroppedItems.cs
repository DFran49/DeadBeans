using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [Header("Item Data")]
    public int itemId;
    public int quantity;
    public Item itemData;
    
    [Header("Visual Settings")]
    public float bobHeight = 0.5f;
    public float bobSpeed = 2f;
    public float rotationSpeed = 50f;
    
    private Vector2 startPosition;
    private bool canBePickedUp = true;
    
    void Start()
    {
        startPosition = transform.position;
        
        // Configurar el item visual
        SetupItemVisual();
        
        // Añadir un pequeño impulso aleatorio al spawning
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomForce = new Vector2(
                Random.Range(-2f, 2f),
                Random.Range(2f, 4f)
            );
            rb.AddForce(randomForce, ForceMode2D.Impulse);
            
            // Detener el rigidbody después de un tiempo
            Invoke("StopPhysics", 1f);
        }
    }
    
    void Update()
    {
        // Animación de flotación y rotación
        if (canBePickedUp)
        {
            AnimateItem();
        }
    }
    
    private void SetupItemVisual()
    {
        // Configurar la apariencia del item
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && itemData != null)
        {
            // Colores según el subtipo del item
            switch (itemData.subtype)
            {
                case ItemSubtype.weapon:
                    spriteRenderer.color = Color.red;
                    break;
                case ItemSubtype.armor:
                    spriteRenderer.color = Color.blue;
                    break;
                case ItemSubtype.material:
                    spriteRenderer.color = Color.green;
                    break;
                case ItemSubtype.consumable:
                    spriteRenderer.color = Color.yellow;
                    break;
                default:
                    spriteRenderer.color = Color.white;
                    break;
            }
        }
    }
    
    private void AnimateItem()
    {
        // Flotación en Y
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector2(transform.position.x, newY);
        
        // Rotación en Z para 2D
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    
    private void StopPhysics()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
        }
        
        // Actualizar la posición inicial para la animación de flotación
        startPosition = transform.position;
    }
    
    public void Initialize(int id, int qty, Item data)
    {
        itemId = id;
        quantity = qty;
        itemData = data;
        
        // Configurar el tag y layer para que el pickup handler lo detecte
        gameObject.tag = "DroppedItem";
        gameObject.layer = LayerMask.NameToLayer("Item"); // Asegúrate de tener esta layer
    }
}
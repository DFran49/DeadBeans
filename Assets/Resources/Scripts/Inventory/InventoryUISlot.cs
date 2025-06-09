using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{
    [Header("UI Components")]
    public Button itemButton;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public GameObject selector;
    
    [Header("Equipment Slot Components (Optional)")]
    public Image hint;
    
    private InventorySlotData slotData;
    private int slotIndex;
    private bool isEquipmentSlot;
    private ItemSubtype equipmentType;
    private InventoryManager inventoryManager;
    
    private void Awake()
    {
        if (itemButton == null)
            itemButton = GetComponentInChildren<Button>();
            
        itemButton.onClick.AddListener(OnSlotClicked);
    }
    
    public void Initialize(InventoryManager manager, int index, bool isEquipSlot = false, ItemSubtype equipType = ItemSubtype.material)
    {
        inventoryManager = manager;
        slotIndex = index;
        isEquipmentSlot = isEquipSlot;
        equipmentType = equipType;
        
        // Configurar hint para slots de equipamiento
        if (isEquipmentSlot && hint != null)
        {
            hint.gameObject.SetActive(true);
        }
        
        UpdateVisuals();
    }
    
    public void UpdateSlotData(InventorySlotData data)
    {
        slotData = data;
        UpdateVisuals();
    }
    
    private void UpdateVisuals()
    {
        if (slotData == null || slotData.IsEmpty())
        {
            // Slot vacío
            icon.gameObject.SetActive(false);
            quantityText.gameObject.SetActive(false);
            
            if (isEquipmentSlot && hint != null)
            {
                hint.gameObject.SetActive(true);
            }
        }
        else
        {
            // Slot con item
            icon.gameObject.SetActive(true);
            
            if (isEquipmentSlot && hint != null)
            {
                hint.gameObject.SetActive(false);
            }
            
            // Cargar sprite del item
            Item itemData = GetItemDataById(slotData.itemId);
            if (itemData != null)
            {
                // Aquí cargarías el sprite real del item
                icon.sprite = Resources.Load<Sprite>("Scripts/Inventory/Items" + itemData.icono);
                
                // Por ahora, usar colores según el subtipo
                switch (itemData.subtype)
                {
                    case ItemSubtype.weapon:
                        icon.color = Color.red;
                        break;
                    case ItemSubtype.armor:
                        icon.color = Color.blue;
                        break;
                    case ItemSubtype.material:
                        icon.color = Color.green;
                        break;
                    case ItemSubtype.consumable:
                        icon.color = Color.yellow;
                        break;
                }
            }
            
            // Mostrar cantidad si es mayor a 1
            if (slotData.quantity > 1)
            {
                quantityText.gameObject.SetActive(true);
                quantityText.text = slotData.quantity.ToString();
            }
            else
            {
                quantityText.gameObject.SetActive(false);
            }
        }
    }
    
    private void OnSlotClicked()
    {
        Debug.Log("OnSlotClicked");
        if (isEquipmentSlot)
        {
            Debug.Log("Equip");
            inventoryManager.SelectEquipmentSlot(equipmentType, slotData);
        }
        else
        {
            Debug.Log("Unequip");
            inventoryManager.SelectStorageSlot(slotIndex, slotData);
        }
    }
    
    public void SetSelected(bool selected)
    {
        Image imagen = selector.GetComponent<Image>();
        Color color = imagen.color;
        if (selected)
        {
            Debug.Log("Selected");
            color.a = 1;
        }
        else
        {
            Debug.Log("Not Selected");
            color.a = 0;
        }
        imagen.color = color;
    }
    
    private Item GetItemDataById(int itemId)
    {
        Item[] allItems = Resources.LoadAll<Item>("Scripts/Inventory/Items");
        
        foreach (Item item in allItems)
        {
            if (item.item_id == itemId)
            {
                return item;
            }
        }
        
        return null;
    }
}
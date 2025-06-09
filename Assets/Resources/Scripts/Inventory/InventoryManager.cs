using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform storageContainer;
    public InventoryUISlot weaponSlot;
    public InventoryUISlot armorSlot;
    public InventoryUISlot consumableSlot;
    
    [Header("Action Buttons")]
    public Button equipButton;
    public Button unequipButton;
    public Button deleteButton;
    
    [Header("Item Info Display")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemValueText;
    public TextMeshProUGUI itemStatusText;
    
    [Header("Player Stats Display")]
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI agilityText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI magicDefenseText;
    public TextMeshProUGUI attackSpeedText;
    
    [Header("Inventory Panel")]
    public GameObject inventoryPanel;
    
    private InventoryData inventoryData;
    private List<InventoryUISlot> storageSlots;
    private InventoryUISlot selectedSlot;
    private InventorySlotData selectedSlotData;
    private int selectedStorageIndex = -1;
    private ItemSubtype selectedEquipmentType;
    private bool isEquipmentSlotSelected;
    
    private void Awake()
    {
        InitializeInventory();
        SetupButtons();
        LoadInventoryFromPlayer();
    }
    
    private void InitializeInventory()
    {
        inventoryData = new InventoryData();
        storageSlots = new List<InventoryUISlot>();
        
        // Inicializar slots de almacenamiento
        for (int i = 0; i < storageContainer.childCount && i < 25; i++)
        {
            InventoryUISlot slot = storageContainer.GetChild(i).GetComponent<InventoryUISlot>();
            if (slot != null)
            {
                slot.Initialize(this, i);
                storageSlots.Add(slot);
            }
        }
        
        // Inicializar slots de equipamiento
        if (weaponSlot != null)
            weaponSlot.Initialize(this, -1, true, ItemSubtype.weapon);
        if (armorSlot != null)
            armorSlot.Initialize(this, -1, true, ItemSubtype.armor);
        if (consumableSlot != null)
            consumableSlot.Initialize(this, -1, true, ItemSubtype.consumable);
    }
    
    private void SetupButtons()
    {
        if (equipButton != null)
            equipButton.onClick.AddListener(EquipSelectedItem);
        if (unequipButton != null)
            unequipButton.onClick.AddListener(UnequipSelectedItem);
        if (deleteButton != null)
            deleteButton.onClick.AddListener(DeleteSelectedItem);
            
        UpdateButtonVisibility();
    }
    
    public void SelectStorageSlot(int index, InventorySlotData slotData)
    {
        // Deseleccionar slots anteriores
        DeselectAllSlots();
        
        selectedStorageIndex = index;
        selectedSlotData = slotData;
        isEquipmentSlotSelected = false;
        
        if (index >= 0 && index < storageSlots.Count)
        {
            selectedSlot = storageSlots[index];
            selectedSlot.SetSelected(true);
        }
        
        UpdateItemInfo();
        UpdateButtonVisibility();
    }
    
    public void SelectEquipmentSlot(ItemSubtype equipType, InventorySlotData slotData)
    {
        // Deseleccionar slots anteriores
        DeselectAllSlots();
        
        selectedEquipmentType = equipType;
        selectedSlotData = slotData;
        isEquipmentSlotSelected = true;
        selectedStorageIndex = -1;
        
        switch (equipType)
        {
            case ItemSubtype.weapon:
                selectedSlot = weaponSlot;
                break;
            case ItemSubtype.armor:
                selectedSlot = armorSlot;
                break;
            case ItemSubtype.consumable:
                selectedSlot = consumableSlot;
                break;
        }
        
        if (selectedSlot != null)
            selectedSlot.SetSelected(true);
        
        UpdateItemInfo();
        UpdateButtonVisibility();
    }
    
    private void DeselectAllSlots()
    {
        foreach (var slot in storageSlots)
        {
            slot.SetSelected(false);
        }
        
        if (weaponSlot != null) weaponSlot.SetSelected(false);
        if (armorSlot != null) armorSlot.SetSelected(false);
        if (consumableSlot != null) consumableSlot.SetSelected(false);
    }
    
    private void UpdateItemInfo()
    {
        if (selectedSlotData == null || selectedSlotData.IsEmpty())
        {
            itemNameText.text = "Sin objeto seleccionado";
            itemValueText.text = "0";
            itemStatusText.text = "";
            return;
        }
        
        Item itemData = GetItemDataById(selectedSlotData.itemId);
        if (itemData != null)
        {
            itemNameText.text = $"{itemData.name} - {selectedSlotData.quantity}";
            int totalValue = itemData.value * selectedSlotData.quantity;
            itemValueText.text = totalValue.ToString();
            itemStatusText.text = inventoryData.IsItemEquipped(selectedSlotData.itemId) ? "Equipado" : "No equipado";
        }
        else
        {
            itemNameText.text = $"Item ID: {selectedSlotData.itemId}";
            itemValueText.text = "0";
            itemStatusText.text = "No equipado";
        }
    }
    
    private void UpdateButtonVisibility()
    {
        bool hasSelection = selectedSlotData != null && !selectedSlotData.IsEmpty();
        
        // Botón eliminar - siempre visible si hay selección
        if (deleteButton != null)
            deleteButton.gameObject.SetActive(hasSelection);
        
        if (!hasSelection)
        {
            if (equipButton != null) equipButton.gameObject.SetActive(false);
            if (unequipButton != null) unequipButton.gameObject.SetActive(false);
            return;
        }
        
        Item itemData = GetItemDataById(selectedSlotData.itemId);
        bool isEquippable = itemData != null && (itemData.subtype == ItemSubtype.weapon || 
                                                itemData.subtype == ItemSubtype.armor || 
                                                itemData.subtype == ItemSubtype.consumable);
        bool isEquipped = inventoryData.IsItemEquipped(selectedSlotData.itemId);
        
        // Botón equipar - visible si el item es equipable, no está equipado y no es un slot de equipamiento
        if (equipButton != null)
            equipButton.gameObject.SetActive(isEquippable && !isEquipped && !isEquipmentSlotSelected);
        
        // Botón desequipar - visible si el item está equipado
        if (unequipButton != null)
            unequipButton.gameObject.SetActive(isEquipped);
    }
    
    private void EquipSelectedItem()
    {
        if (selectedStorageIndex >= 0 && inventoryData.EquipItem(selectedStorageIndex))
        {
            // ARREGLO: Guardar cambios inmediatamente
            SaveInventoryToPlayer();
            RefreshAllSlots();
            UpdatePlayerStats();
            UpdateItemInfo();
            UpdateButtonVisibility();
        }
    }
    
    private void UnequipSelectedItem()
    {
        if (isEquipmentSlotSelected)
        {
            if (inventoryData.UnequipItem(selectedEquipmentType))
            {
                // ARREGLO: Guardar cambios inmediatamente
                SaveInventoryToPlayer();
                RefreshAllSlots();
                UpdatePlayerStats();
                UpdateItemInfo();
                UpdateButtonVisibility();
            }
        }
        else
        {
            // Determinar tipo de equipamiento basado en el item
            Item itemData = GetItemDataById(selectedSlotData.itemId);
            if (itemData != null && inventoryData.UnequipItem(itemData.subtype))
            {
                // ARREGLO: Guardar cambios inmediatamente
                SaveInventoryToPlayer();
                RefreshAllSlots();
                UpdatePlayerStats();
                UpdateItemInfo();
                UpdateButtonVisibility();
            }
        }
    }
    
    private void DeleteSelectedItem()
    {
        if (isEquipmentSlotSelected)
        {
            // Eliminar del slot de equipamiento
            switch (selectedEquipmentType)
            {
                case ItemSubtype.weapon:
                    inventoryData.weaponSlot.Clear();
                    break;
                case ItemSubtype.armor:
                    inventoryData.armorSlot.Clear();
                    break;
                case ItemSubtype.consumable:
                    inventoryData.consumableSlot.Clear();
                    break;
            }
        }
        else if (selectedStorageIndex >= 0)
        {
            // Eliminar del inventario
            inventoryData.RemoveItem(selectedStorageIndex);
        }
        
        SaveInventoryToPlayer();
        RefreshAllSlots();
        UpdatePlayerStats();
        DeselectAllSlots();
        selectedSlotData = null;
        UpdateItemInfo();
        UpdateButtonVisibility();
    }
    
    public bool AddItemToInventory(int itemId, int quantity)
    {
        bool success = inventoryData.AddItem(itemId, quantity);
        if (success)
        {
            SaveInventoryToPlayer();
            RefreshAllSlots();
        }
        return success;
    }
    
    private void RefreshAllSlots()
    {
        // Actualizar slots de almacenamiento
        for (int i = 0; i < storageSlots.Count && i < inventoryData.storageSlots.Count; i++)
        {
            storageSlots[i].UpdateSlotData(inventoryData.storageSlots[i]);
        }
        
        // Actualizar slots de equipamiento
        if (weaponSlot != null)
            weaponSlot.UpdateSlotData(inventoryData.weaponSlot);
        if (armorSlot != null)
            armorSlot.UpdateSlotData(inventoryData.armorSlot);
        if (consumableSlot != null)
            consumableSlot.UpdateSlotData(inventoryData.consumableSlot);
    }
    
    private void UpdatePlayerStats()
    {
        ScriptablePlayer playerData = Resources.Load<ScriptablePlayer>("Scripts/Player/Player");
        if (playerData == null) return;
        
        // Stats base del jugador
        int baseStrength = 0;
        int baseSpeed = 0;
        int baseAgility = 0;
        int baseDefense = 0;
        int baseMagicDefense = 0;
        int baseAttackSpeed = 0;
        
        // Aplicar bonificaciones de equipamiento
        int totalStrength = baseStrength + GetEquipmentStat("strength");
        int totalSpeed = baseSpeed + GetEquipmentStat("speed");
        int totalAgility = baseAgility + GetEquipmentStat("agility");
        int totalDefense = baseDefense + GetEquipmentStat("def");
        int totalMagicDefense = baseMagicDefense + GetEquipmentStat("magic_def");
        int totalAttackSpeed = baseAttackSpeed + GetEquipmentStat("attack_speed");
        
        // Actualizar UI
        if (strengthText != null) strengthText.text = totalStrength.ToString();
        if (speedText != null) speedText.text = totalSpeed.ToString();
        if (agilityText != null) agilityText.text = totalAgility.ToString();
        if (defenseText != null) defenseText.text = totalDefense.ToString();
        if (magicDefenseText != null) magicDefenseText.text = totalMagicDefense.ToString();
        if (attackSpeedText != null) attackSpeedText.text = totalAttackSpeed.ToString();
    }
    
    private int GetEquipmentStat(string statName)
    {
        int totalStat = 0;
        
        // Verificar arma
        if (!inventoryData.weaponSlot.IsEmpty())
        {
            Item weaponData = GetItemDataById(inventoryData.weaponSlot.itemId);
            if (weaponData != null)
            {
                switch (statName)
                {
                    case "strength": totalStat += weaponData.strength; break;
                    case "attack_speed": totalStat += Mathf.RoundToInt(weaponData.attack_speed); break;
                }
            }
        }
        
        // Verificar armadura
        if (!inventoryData.armorSlot.IsEmpty())
        {
            Item armorData = GetItemDataById(inventoryData.armorSlot.itemId);
            if (armorData != null)
            {
                switch (statName)
                {
                    case "def": totalStat += armorData.def; break;
                    case "magic_def": totalStat += armorData.magic_def; break;
                    case "agility": totalStat += armorData.agility; break;
                    case "speed": totalStat += armorData.speed; break;
                }
            }
        }
        
        return totalStat;
    }
    
    private void LoadInventoryFromPlayer()
    {
        ScriptablePlayer playerData = Resources.Load<ScriptablePlayer>("Scripts/Player/Player");
        if (playerData != null && playerData.inventoryData != null)
        {
            inventoryData = playerData.inventoryData;
            RefreshAllSlots();
            UpdatePlayerStats();
        }
        else
        {
            inventoryData = new InventoryData();
            SaveInventoryToPlayer();
        }
    }
    
    public void SaveInventoryToPlayer()
    {
        ScriptablePlayer playerData = Resources.Load<ScriptablePlayer>("Scripts/Player/Player");
        if (playerData != null)
        {
            playerData.inventoryData = inventoryData;
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(playerData);
            #endif
        }
    }
    
    public InventoryData GetInventoryData()
    {
        return inventoryData;
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
    
    private void OnDestroy()
    {
        SaveInventoryToPlayer();
    }
    
    public void ShowInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true);
            RefreshInventory();
        }
    }
    
    public void HideInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
            DeselectAllSlots();
            selectedSlotData = null;
            UpdateItemInfo();
            UpdateButtonVisibility();
        }
    }
    
    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            if (inventoryPanel.activeInHierarchy)
            {
                HideInventory();
            }
            else
            {
                ShowInventory();
            }
        }
    }
    
    public void RefreshInventory()
    {
        // Recargar datos del jugador y refrescar todo
        LoadInventoryFromPlayer();
        RefreshAllSlots();
        UpdatePlayerStats();
        UpdateItemInfo();
        UpdateButtonVisibility();
    }
    
    public bool IsInventoryOpen()
    {
        return inventoryPanel != null && inventoryPanel.activeInHierarchy;
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool juegoPausado = false;
    public static bool inventarioAbierto = false;
    public static bool shopOpen = false; 
    private PlayerInputs inputs;
    
    public GameObject pauseMenuUI;
    public GameObject sellButton;

    private GameObject player;

    void Awake()
    {
        inputs = new PlayerInputs();
    }
    
    private void OnEnable() {
        inputs.Enable();
        inputs.Gameplay.Pause.performed += onMenuKeyPressed;
        inputs.Gameplay.Inventory.performed += onInventoryKeyPressed;
    }
    
    private void OnDisable() {
        inputs.Gameplay.Pause.performed -= onMenuKeyPressed;
        inputs.Gameplay.Inventory.performed -= onInventoryKeyPressed;
        inputs.Disable();
    }

    public void Reanudar()
    {
        Time.timeScale = 1;
        player.GetComponent<PlayerController>().EnableInputs();
        player.GetComponent<PlayerMovement>().EnableInputs();
        player.GetComponent<AnimationCompController>().ResumeAnimation();
        
        pauseMenuUI.SetActive(false);
        juegoPausado = false;
        shopOpen = false;
        inventarioAbierto = false;
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        inventoryManager.HideInventory();
    }

    public void Pausar()
    {
        player.GetComponent<PlayerController>().DisableInputs();
        player.GetComponent<PlayerMovement>().DisableInputs();
        player.GetComponent<AnimationCompController>().PauseAnimation();
        Time.timeScale = 0;
    }

    public void AbrirMenuPrincipal()
    {
        player.GetComponent<PlayerController>().Save();
        Time.timeScale = 1;
        juegoPausado = false;
        shopOpen = false;
        inventarioAbierto = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void CerrarJuego()
    {
        player.GetComponent<PlayerController>().Save();
        Debug.Log("Cerrando juego...");
        Application.Quit();
    }
    
    public void onMenuKeyPressed(InputAction.CallbackContext context)
    {
        if (inventarioAbierto || shopOpen)
            return;
        
        if (juegoPausado)
        {
            Reanudar();
        } 
        else
        {
            Pausar();
            juegoPausado = true;
            pauseMenuUI.SetActive(true);
        }
    }

    public void onInventoryKeyPressed(InputAction.CallbackContext context)
    {
        if (juegoPausado || shopOpen)
            return;
        
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        inventoryManager.ToggleInventory();
        if (inventarioAbierto)
        {
            Reanudar();
        } 
        else
        {
            Pausar();
            inventarioAbierto = true;
        }
    }

    public void OpenShop()
    {
        if (juegoPausado || inventarioAbierto)
            return;

        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        inventoryManager.ToggleInventory();
        
        if (shopOpen)
        {
            Reanudar();
            sellButton.active = false;
        }
        else
        {
            Pausar();
            sellButton.active = true;
            shopOpen = true;
        }
    }
    
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}

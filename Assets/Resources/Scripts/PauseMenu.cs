using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool juegoPausado = false;
    private PlayerInputs inputs;
    
    public GameObject pauseMenuUI;

    private GameObject player;

    void Awake()
    {
        inputs = new PlayerInputs();
    }
    
    private void OnEnable() {
        inputs.Enable();
        inputs.Gameplay.Pause.performed += onMenuKeyPressed;
    }
    
    private void OnDisable() {
        inputs.Gameplay.Pause.performed -= onMenuKeyPressed;
        inputs.Disable();
    }

    public void Reanudar()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        juegoPausado = false;
        player.GetComponent<PlayerController>().EnableInputs();
        player.GetComponent<PlayerMovement>().EnableInputs();
        player.GetComponent<AnimationCompController>().ResumeAnimation();
    }

    public void Pausar()
    {
        player.GetComponent<PlayerController>().DisableInputs();
        player.GetComponent<PlayerMovement>().DisableInputs();
        player.GetComponent<AnimationCompController>().PauseAnimation();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        juegoPausado = true;
    }

    public void AbrirMenuPrincipal()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void CerrarJuego()
    {
        Debug.Log("Cerrando juego...");
        Application.Quit();
    }
    
    public void onMenuKeyPressed(InputAction.CallbackContext context)
    {
        if (juegoPausado)
        {
            Reanudar();
        } 
        else
        {
            Pausar();
        }
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}

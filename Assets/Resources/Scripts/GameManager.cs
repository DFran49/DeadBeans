using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector2 spawnPosition = new Vector2(1, 1);
    
    [SerializeField] private Camera cam;
    
    private PauseMenu pauseMenu;

    private void Awake()
    {
        pauseMenu = GetComponent<PauseMenu>();
    }

    private void Start()
    {
        SpawnPlayer();
        cam.GetComponent<CameraFollow>().target = player.transform;
        pauseMenu.SetPlayer(player);
    }

    private void SpawnPlayer()
    {
        if (player == null)
        {
            player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            player.name = "Player";
            var health = player.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.OnDeath += OnPlayerDeath;
            }
        }
        //Borrar cuando haya game over
        else
        {
            player.transform.position = spawnPosition;
            player.SetActive(true);
            
            player.GetComponent<PlayerController>().Die();
            
            var hurtHb = player.transform.Find("Hurt_Hitbox").GetComponent<CapsuleCollider2D>();
            hurtHb.enabled = false;
            Debug.Log("Invulnerable");
            StartCoroutine(ReenableComponent(hurtHb, 3f));
        }
    }
    
    //Será inútil cuando haya game over
    private IEnumerator ReenableComponent(CapsuleCollider2D component, float delay)
    {
        yield return new WaitForSeconds(delay);
        component.enabled = true;
        Debug.Log("Vulnerable");
    }
    
    private void OnPlayerDeath()
    {
        Debug.Log("Jugador muerto. Respawn en 5 segundos.");
        player.SetActive(false);
        Invoke(nameof(SpawnPlayer), 5f);
        //Meter game over
    }
}

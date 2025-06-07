using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoom : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform spawnPos;
    public GameObject enterDoor;
    public GameObject exitDoor;
    
    private GameObject boss;

    public bool IsBossSpawned = false;

    //ELIMINAR
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !IsBossSpawned)
        {
            boss = Instantiate(bossPrefab, spawnPos.position, Quaternion.identity, transform);
            
            IsBossSpawned = true;
            CloseDoors();
        }
    }

    private void CloseDoors()
    {
        enterDoor.GetComponent<SpriteRenderer>().enabled = true;
        var doorPos = enterDoor.transform.position;
        doorPos.y += 1f;
        enterDoor.transform.position = doorPos;
    }

    public void SetUpDoors()
    {
        Debug.Log("SetUpDoors");
        enterDoor.GetComponent<SpriteRenderer>().enabled = false;
        var doorPos = enterDoor.transform.position;
        doorPos.y -= 1f;
        enterDoor.transform.position = doorPos;
        enterDoor.GetComponent<Door>().OnHitboxTriggerEnter += (Collider2D other) => GoBack(other);
        
        exitDoor.GetComponent<SpriteRenderer>().enabled = false;
        doorPos = exitDoor.transform.position;
        doorPos.y += 1f;
        exitDoor.transform.position = doorPos;
        exitDoor.GetComponent<Door>().OnHitboxTriggerEnter += (Collider2D other) => ExitDungeon(other);
    }

    private void GoBack(Collider2D other)
    {
        if (other.transform.parent != null)
        {
            if (other.transform.parent.CompareTag("Player"))
            {
                var pos = other.transform.parent.position;
                pos.y -= 19;
                other.transform.parent.position = pos;
            }
        }
    }

    private void ExitDungeon(Collider2D other)
    {
        Debug.Log("Exiting dungeon");
        SceneManager.LoadScene("City");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

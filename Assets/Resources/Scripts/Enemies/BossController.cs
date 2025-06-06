using System;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private BossRoom room;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        room = gameObject.transform.parent.GetComponent<BossRoom>();
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
        room.SetUpDoors();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

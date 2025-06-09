using System;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameObject interactionTag;
    public TextMeshProUGUI actionText;
    public TextMeshProUGUI interactionText;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name.Equals("Interaction_Hitbox"))
        {
            Debug.Log("Entra");
            interactionTag.SetActive(true);
            actionText.text = "Presiona la tecla F para " + interactionText.text;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.name.Equals("Interaction_Hitbox"))
            interactionTag.SetActive(false);
    }
}

using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    public Transform playerTrans;
    private float speed;
    public float stopDistance = 0.5f;

    private Rigidbody2D rb;
    private Vector2 knockbackVelocity = Vector2.zero;
    private float knockbackTime = 0f;
    private bool isKnockbackActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (playerTrans == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                playerTrans = playerObject.transform;
        }
    }

    void FixedUpdate()
    {
        if (playerTrans == null || speed == 0) return;
        
        if (isKnockbackActive)
        {
            rb.linearVelocity = knockbackVelocity;
            knockbackTime -= Time.fixedDeltaTime;
        
            if (knockbackTime <= 0f)
            {
                isKnockbackActive = false;
                rb.linearVelocity = Vector2.zero;
            }

            return;
        }

        if (!player.activeInHierarchy)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            Vector2 direction = (playerTrans.position - transform.position).normalized;
            float distance = Vector2.Distance(playerTrans.position, transform.position);

            if (distance > stopDistance)
            {
                Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
                rb.MovePosition(newPosition);
            }
        }
    }
    
    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        knockbackVelocity = direction.normalized * force;
        knockbackTime = duration;
        isKnockbackActive = true;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        playerTrans = player.transform;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}

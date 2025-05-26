using System;
using UnityEngine;

public class AttackHitboxController : MonoBehaviour
{
    [SerializeField] private HitboxesData hitboxesData;
    
    
    private PolygonCollider2D polygonCollider;
    
    public event Action<Collider2D> OnHitboxTriggerEnter;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*private void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider == null)
        {
            Debug.LogError("No hay PolygonCollider2D en este GameObject");
            return;
        }

        PrintPoints();
    }

    private void PrintPoints()
    {
        Vector2[] points = polygonCollider.points;
        string pointsStr = "new Vector2[] { ";

        for (int i = 0; i < points.Length; i++)
        {
            pointsStr += $"new Vector2({points[i].x}f, {points[i].y}f)";
            if (i != points.Length - 1)
                pointsStr += ", ";
        }

        pointsStr += " };";

        Debug.Log(pointsStr);
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
    
    private void Awake()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false;
    }

    public void SetHitboxFrame(int direction)
    {
        HitboxFrames frames = null;

        switch (direction)
        {
            case 0:
                frames = hitboxesData.down;
                break;
            case 1:
                frames = hitboxesData.left;
                break;
            case 2:
                frames = hitboxesData.up;
                break;
            case 3:
                frames = hitboxesData.right;
                break;
        }

        if (frames == null)
        {
            Debug.LogWarning("Frame index out of range or direction not found.");
            polygonCollider.enabled = false;
            return;
        }

        Vector2[] points = frames.frames[0].points;

        polygonCollider.pathCount = 1;
        polygonCollider.SetPath(0, points);
        polygonCollider.enabled = true;
    }

    public void DisableHitboxes()
    {
        polygonCollider.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHitboxTriggerEnter?.Invoke(other);
        
    }
}

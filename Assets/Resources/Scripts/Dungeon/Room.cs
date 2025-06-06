using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour {
    
    public List<Directions> exitDirections;
    private List<Directions> lockedDirections;
    public Directions enterDirection;
    
    public List<GameObject> doors;

    private Vector2Int arrayPos;

    private bool isSpecial = false;

    private Directions curDoorDirection;

    private void Awake()
    {
        exitDirections = new List<Directions>();
        lockedDirections = new List<Directions>();
    }

    public void AddExitDirection(Directions dir) {
        if (!exitDirections.Contains(dir) && dir != enterDirection) 
            exitDirections.Add(dir);
    }

    public void SetEnterDirection(Directions dir)
    {
        enterDirection = dir;
    }

    public List<Directions> GetExitDirections()
    {
        return exitDirections;
    }

    public Directions GetEnterDirection()
    {
        return enterDirection;
    }

    public void SetLockedDirections(int width, int height)
    {
        if (arrayPos.x <= 0)
            lockedDirections.Add(Directions.Left);
        if (arrayPos.y <= 0)
            lockedDirections.Add(Directions.Down);
        if (arrayPos.x >= width-1)
            lockedDirections.Add(Directions.Right);
        if (arrayPos.y >= height-1)
            lockedDirections.Add(Directions.Up);
        if (!lockedDirections.Contains(enterDirection))
            lockedDirections.Add(enterDirection);
    }

    public List<Directions> GetLockedDirections()
    {
        return lockedDirections;
    }

    public void AddLockedDirection(Directions dir)
    {
        lockedDirections.Add(dir);
    }

    public List<Directions> GetFreeDirections()
    {
        List<Directions> dirs = System.Enum.GetValues(typeof(Directions)).Cast<Directions>().ToList();
        foreach (Directions dir in lockedDirections)
        {
            dirs.Remove(dir);
        }
        return dirs;
    }

    public void SetUpDoors()
    {
        foreach (GameObject door in doors)
        {
            switch (door.name)
            {
                case "Exit_Right":
                    if (exitDirections.Contains(Directions.Right) || enterDirection == Directions.Right)
                    {
                        SetExit(door,Directions.Right);
                    }
                    else
                        CloseExit(door);
                    break;
                case "Exit_Left":
                    if (exitDirections.Contains(Directions.Left) || enterDirection == Directions.Left)
                    {
                        SetExit(door, Directions.Left);
                    }
                    else
                        CloseExit(door);
                    break;
                case "Exit_Up":
                    if (exitDirections.Contains(Directions.Up) || enterDirection == Directions.Up)
                    {
                        SetExit(door, Directions.Up);
                    }
                    else
                        CloseExit(door);
                    break;
                case "Exit_down":
                    if (exitDirections.Contains(Directions.Down) || enterDirection == Directions.Down)
                    {
                        SetExit(door, Directions.Down);
                    }
                    else
                        CloseExit(door);
                    break;
            }
        }
    }

    private void SetExit(GameObject door, Directions direction)
    {
        door.GetComponent<SpriteRenderer>().enabled = false;
        var margen = 0f;
        switch (direction)
        {
            case Directions.Left:
                margen = -1f;
                break;
            case Directions.Right:
                margen = 1f;
                break;
            case Directions.Up:
                margen = 1f;
                break;
            case Directions.Down:
                margen = -1f;
                break;
        }

        var doorPos = door.transform.position;
        
        if (direction == Directions.Down || direction == Directions.Up)
        {
            doorPos.y += margen;
        }
        else
        {
            doorPos.x += margen;
        }
        
        door.transform.position = doorPos;
        
        Door d = door.GetComponent<Door>();
        if (door.transform.parent.name != "SalaBoss")
            d.OnHitboxTriggerEnter += (Collider2D other) => HandleHitboxTrigger(other, direction);
    }

    private void HandleHitboxTrigger(Collider2D other, Directions dir)
    {
        if (other.transform.parent != null)
        {
            if (other.transform.parent.CompareTag("Player"))
            {
                var pos = other.transform.parent.position;
                switch (dir)
                {
                    case Directions.Right: pos.x += 23; break;
                    case Directions.Left: pos.x -= 23; break;
                    case Directions.Up: pos.y += 23; break;
                    case Directions.Down: pos.y -= 23; break;
                }
                other.transform.parent.position = pos;
            }
        }
    }

    private void CloseExit(GameObject door)
    {
        door.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void SetArrayPos(Vector2Int pos)
    {
        arrayPos = pos;
    }

    public Vector2Int GetArrayPos()
    {
        return arrayPos;
    }

    public bool IsSpecial()
    {
        return isSpecial;
    }

    public void SetIsSpecial(bool isSpecial)
    {
        this.isSpecial = isSpecial;
    }
}

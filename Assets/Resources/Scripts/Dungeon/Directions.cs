using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Directions
{
    Up, Right, Down, Left, None
}

public static class DirectionsUtils
{
    public static Directions GetRandomDirection()
    {
        Directions[] valores = (Directions[]) System.Enum.GetValues(typeof(Directions));
        return (Directions)valores[Random.Range(0, valores.Length)];
    }
    
    public static Directions GetRandomDirectionExcluding(Directions[] excluded)
    {
        Directions[] valores = (Directions[]) System.Enum.GetValues(typeof(Directions));
        List<Directions> available = new List<Directions>(valores);
        available.RemoveAll(t => excluded.Contains(t));
        return available[Random.Range(0, available.Count)];
    }

    public static Directions GetOppositeDirection(Directions dir)
    {
        switch (dir)
        {
            case Directions.Up:
                return Directions.Down;
            case Directions.Right:
                return Directions.Left;
            case Directions.Down:
                return Directions.Up;
            case Directions.Left:
                return Directions.Right;
            default:
                return Directions.None;
        }
    }

    public static Vector2Int GetPointingDirection(Directions dir, Vector2Int pos)
    {
        int x = pos.x;
        int y = pos.y;
        
        switch (dir)
        {
            case Directions.Up:
                y += 1;
                break;
            case Directions.Right:
                x += 1;
                break;
            case Directions.Down:
                y -= 1;
                break;
            case Directions.Left:
                x -= 1;
                break;
        }
        
        Vector2Int pointingPos = new Vector2Int(x, y);
        return pointingPos;
    }
}
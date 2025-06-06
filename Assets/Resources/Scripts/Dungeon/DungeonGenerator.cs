using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {
    public GameObject spawnRoomPrefab;  // Sala de spawn
    public GameObject[] normalRoomPrefabs; // Sala normal
    public GameObject bossRoomPrefab;   // Sala del boss
    
    public int maxBranch = 5;      // Número máximo de salas normales en una rama
    public int maxRooms = 15;       //Número máximo de salas normales en total
    public int dungeonWidth = 5;       // Ancho total del dungeon
    public int dungeonHeight = 5;      // Alto total del dungeon

    private GameObject[,] dungeon;  // Lista de salas generadas
    
    
    private bool bossIsGenerated = false;
    private bool isFirstRoom = true;
    private bool isFinished = false;

    private Vector2Int nextRoom;
    private Directions enterDirection;

    private int cont = 0;

    void Start()
    {
        dungeon = new GameObject[dungeonWidth, dungeonHeight+1];
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        GenerateSpawnRoom();
        InstantiateRoom(nextRoom, enterDirection,1);
        GenerateBossRoom();
        
        foreach (GameObject room in dungeon)
        {
            if (room != null)
            {
                room.GetComponent<Room>().SetUpDoors();
            }
        }
    }

    void GenerateSpawnRoom()
    {
        Vector2 roomPos = new Vector2(0, 0);
        GameObject room = Instantiate(spawnRoomPrefab, roomPos,Quaternion.identity);
        room.name = "SalaSpawn";
        dungeon[0, 0] = room;
        var script = room.GetComponent<Room>();
        script.SetArrayPos(new Vector2Int(0,0));
        script.AddExitDirection(Directions.Up);
        script.SetIsSpecial(true);
        enterDirection = DirectionsUtils.GetOppositeDirection(Directions.Up);
        nextRoom = new Vector2Int(0, 1);
    }

    void InstantiateRoom(Vector2Int position, Directions dir, int branchIndex)
    {
        cont++;
        Vector2 roomPos = new Vector2(position.x*35, position.y*35);
        GameObject roomPrefab = normalRoomPrefabs[Random.Range(0, normalRoomPrefabs.Length)];
        GameObject room = Instantiate(roomPrefab, roomPos,Quaternion.identity);
        room.name = "Sala"+cont;
        dungeon[position.x, position.y] = room;
        var script = room.GetComponent<Room>();
        script.SetArrayPos(position);
        script.SetEnterDirection(dir);
        script.SetLockedDirections(dungeonWidth, dungeonHeight);
        
        List<Directions> availableDirs = script.GetFreeDirections();
        List<Directions> newExits = new List<Directions>();
        bool isEnd = true;
        int exits = 1;

        while (availableDirs.Any())
        {
            float chance = 100f / exits;
            Directions ranDir = GetRandomDirection(script);
            Vector2Int pointNext = DirectionsUtils.GetPointingDirection(ranDir, script.GetArrayPos());

            if (dungeon[pointNext.x, pointNext.y] != null)
            {
                if (Random.value > 0.5f)
                {
                    var path = dungeon[pointNext.x, pointNext.y].GetComponent<Room>();
                    if (!path.IsSpecial())
                    {
                        script.AddExitDirection(ranDir);
                        path.AddExitDirection(DirectionsUtils.GetOppositeDirection(ranDir));
                    }
                    
                }
                    
            }
            else if (maxRooms > cont && maxBranch > branchIndex)
            {
                if (Random.Range(0f, 100f) < chance)
                {
                    script.AddExitDirection(ranDir);
                    newExits.Add(ranDir);
                    isEnd = false;
                }
            }
            
            script.AddLockedDirection(ranDir);
            availableDirs.Remove(ranDir);
        }
        
        if (maxRooms > cont)
        {
            if (!isEnd)
            {
                foreach (Directions d in newExits)
                {
                    Vector2Int newRoom = DirectionsUtils.GetPointingDirection(d, script.GetArrayPos());
                    if (dungeon[newRoom.x, newRoom.y] == null)
                    {
                        Directions enterDir = DirectionsUtils.GetOppositeDirection(d);
                        if (branchIndex < maxBranch)
                            InstantiateRoom(newRoom, enterDir, branchIndex+1);
                    }
                }
            }
        }
    }

    Directions GetRandomDirection(Room script)
    {
        Directions[] excludedArr = script.GetLockedDirections().ToArray();
        return DirectionsUtils.GetRandomDirectionExcluding(excludedArr);
    }

    void GenerateBossRoom()
    {
        List<GameObject> rooms = new List<GameObject>();
        foreach (GameObject room in dungeon)
        {
            if (room != null)
            {
                var scriptTemp = room.GetComponent<Room>();
                Vector2Int positionUp = DirectionsUtils.GetPointingDirection(Directions.Up,scriptTemp.GetArrayPos());
                if (!scriptTemp.exitDirections.Contains(Directions.Up) && dungeon[positionUp.x, positionUp.y] == null)
                {
                    rooms.Add(room);
                }
            }
        }

        var selectedRoom = rooms[Random.Range(0, rooms.Count)].GetComponent<Room>();
        selectedRoom.AddExitDirection(Directions.Up);
        
        Vector2Int position = DirectionsUtils.GetPointingDirection(Directions.Up,selectedRoom.GetArrayPos());
        Debug.Log("42 aaaaaaaaaaaaaaaaaa");
        Vector2 roomPos = new Vector2(position.x*35, position.y*35);
        GameObject bossRoom = Instantiate(bossRoomPrefab, roomPos,Quaternion.identity);
        bossRoom.name = "SalaBoss";
        dungeon[position.x, position.y] = bossRoom;
        var script = bossRoom.GetComponent<Room>();
        script.SetArrayPos(position);
        script.SetIsSpecial(true);
        script.SetEnterDirection(Directions.Down);
    }
}
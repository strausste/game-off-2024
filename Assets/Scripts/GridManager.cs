using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // ====================================================================================
    // Custom types definitions
    // ====================================================================================

     private struct Cell
    {
        public Vector2Int position; 
        public bool isOccupied; // TODO: keep only tileObject and check for isOccupied by tileObject != null
        public GameObject tileObject;

        public Cell(int x, int y)
        {
            position = new Vector2Int(x, y);
            isOccupied = false;
            tileObject = null;
        }
    }

    // ====================================================================================


    // ====================================================================================
    // Class attributes
    // ====================================================================================

    // TODO: Probably this is overkilled (we just need coordinates)
    private Cell[,] _grid;
    private static GridManager _instance; // Singleton

    [Header("Grid settings")] 
    [SerializeField] private int gridSizeX = 34;
    [SerializeField] private int gridSizeY = 20;

    // TODO: Make these scale up with the levels
    [Header("Room settings")]
    [SerializeField] private int maxNumberOfRooms = 5;
    [SerializeField] private int minRoomWidth = 3;
    [SerializeField] private int maxRoomWidth = 8;
    [SerializeField] private int minRoomHeight = 3;
    [SerializeField] private int maxRoomHeight = 6;
    [SerializeField] private int roomOffset = 1; // TODO: make it optional ?
    [SerializeField] private int maxDoorsPerRoom = 2;


    [Header("Prefabs")] 
    [SerializeField] private GameObject gridParent;

    [SerializeField] private GameObject debugPrefab;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject wallPrefab;

    // [SerializeField] private GameObject chestPrefab;
    // [SerializeField] private GameObject enemyPrefab;
    // ...

    // ====================================================================================


    // ====================================================================================
    // Class methods
    // ====================================================================================

    // Access the instance (singleton)
    public static GridManager GetInstance()
    {
        // If the instance doesn't exist, find or create it
        if (_instance == null)
        {
            _instance = FindFirstObjectByType<GridManager>();

            // If no instances exist in the scene, create a new GameObject and add the script
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject(nameof(GridManager));
                _instance = singletonObject.AddComponent<GridManager>();
            }
        }

        return _instance;
    }


    public Vector2Int GetGridSize()
    {
        return new Vector2Int(gridSizeX, gridSizeY);
    }


    private void CreateGrid()
    {
        _grid = new Cell[gridSizeX, gridSizeY];

         for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeY; z++)
            {
                _grid[x, z] = new Cell(x, z);
            }
        }
    }


    /** Instantiates and stores the desired prefab in the _grid */
    public void CreateCube(GameObject prefab, int x, int z)
    {
        // Position for the cube
        Vector3 cubePosition = new Vector3(x, 0, z);
        GameObject cube = Instantiate(prefab, cubePosition, Quaternion.identity);
        cube.transform.parent = gridParent.transform;

        // Update grid cell
        _grid[x, z].tileObject = cube;
        _grid[x, z].isOccupied = prefab != null;
    }

    /** Deletes a prefab in the _grid */
    public void DeleteCube(int x, int z)
    {
        if (_grid[x, z].tileObject != null)
        {
            Destroy(_grid[x, z].tileObject);
            _grid[x, z].tileObject = null;
            _grid[x, z].isOccupied = false;
        }
    }


    /** Marks area as occupied in the Cell struct */
    private void MarkArea(int startX, int startZ, int width, int height)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int z = startZ; z < startZ + height; z++)
            {
                _grid[x, z].isOccupied = true;
            }
        }
    }
    

    /** Generates random rooms within the grid */
    private void GenerateRooms()
    {
        int roomsCreated = 0;
        int maxAttemps = 1000; // TODO: [SerializeField] ???

        while(roomsCreated < maxNumberOfRooms && maxAttemps > 0)
        {
            int roomWidth = UnityEngine.Random.Range(minRoomWidth, maxRoomWidth);
            int roomHeight = UnityEngine.Random.Range(minRoomHeight, maxRoomHeight);
            int startX = UnityEngine.Random.Range(1, gridSizeX - roomWidth - 1);  // Avoiding edges
            int startZ = UnityEngine.Random.Range(1, gridSizeY - roomHeight - 1); // Avoiding edges

            if (CanPlaceRoom(startX, startZ, roomWidth, roomHeight))
            {
                CreateRoom(startX, startZ, roomWidth, roomHeight);
                MarkArea(startX, startZ, roomWidth, roomHeight);
                roomsCreated++;
            }

            maxAttemps--;
        }
    }
    

    private bool CanPlaceRoom(int startX, int startZ, int width, int height)
    {
        // Ensure we have roomOffset space around the room
        int startXWithOffset = Mathf.Max(0, startX - roomOffset);
        int startZWithOffset = Mathf.Max(0, startZ - roomOffset);
        int endXWithOffset = Mathf.Min(gridSizeX, startX + width + roomOffset);
        int endZWithOffset = Mathf.Min(gridSizeY, startZ + height + roomOffset);

        for (int x = startXWithOffset; x < endXWithOffset; x++)
        {
            for (int z = startZWithOffset; z < endZWithOffset; z++)
            {
                if (_grid[x, z].isOccupied) return false; // Room can't be placed here
            }
        }

        return true;
    }


    private void CreateRoom(int startX, int startZ, int width, int height)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int z = startZ; z < startZ + height; z++)
            {
                bool isWall = (x == startX || x == startX + width - 1 || z == startZ || z == startZ + height - 1);

                if (isWall)
                {
                    CreateCube(wallPrefab, x, z);

                    if (x == startX || x == startX + width - 1)
                        _grid[x,z].tileObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
            }
        }

        // Doors
        int doorsInThisRoom = UnityEngine.Random.Range(1, maxDoorsPerRoom);
        for(int i=0; i<doorsInThisRoom; i++)
        {
            // Randomly choose one of the four walls
            int wall = UnityEngine.Random.Range(0, 4);
            int doorX = startX;
            int doorZ = startZ;

            switch (wall)
            {
                case 0: // Top wall, exclude corners
                    doorX = UnityEngine.Random.Range(startX + 1, startX + width - 1);
                    doorZ = startZ;
                    break;
                case 1: // Bottom wall, exclude corners
                    doorX = UnityEngine.Random.Range(startX + 1, startX + width - 1);
                    doorZ = startZ + height - 1;
                    break;
                case 2: // Left wall, exclude corners
                    doorX = startX;
                    doorZ = UnityEngine.Random.Range(startZ + 1, startZ + height - 1);
                    break;
                case 3: // Right wall, exclude corners
                    doorX = startX + width - 1;
                    doorZ = UnityEngine.Random.Range(startZ + 1, startZ + height - 1);
                    break;
            }

            if (_grid[doorX, doorZ].tileObject.CompareTag("Wall"))
            {
                // Remove wall and place door
                DeleteCube(doorX, doorZ);
                CreateCube(doorPrefab, doorX, doorZ);
            }
        }
    }

    // ====================================================================================


    // ====================================================================================
    // Monobehaviour methods
    // ====================================================================================

    private void Awake()
    {
        // Ensure there's only one instance, and persist it between scenes
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
        }
        
        CreateGrid();
        GenerateRooms();
    }

    // ====================================================================================
}

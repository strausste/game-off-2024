using UnityEngine;

public class GridManager : MonoBehaviour
{
    // ====================================================================================
    // Custom types definitions
    // ====================================================================================

     private struct Cell
    {
        public Vector2Int position; 
        public bool isOccupied;
        public GameObject tilePrefab;

        public Cell(int x, int y)
        {
            position = new Vector2Int(x, y);
            isOccupied = false;
            tilePrefab = null;
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
    [Header("Rooms settings")]
    [SerializeField] private int numberOfRooms = 5;
    [SerializeField] private int minRoomWidth = 3;
    [SerializeField] private int maxRoomWidth = 8;
    [SerializeField] private int minRoomHeight = 3;
    [SerializeField] private int maxRoomHeight = 6;
    [SerializeField] private int roomOffset = 1; // TODO: make it optional ?


    [Header("Prefabs")] 
    [SerializeField] private GameObject gridParent;

    [SerializeField] private GameObject debugPrefab;
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
        Vector3 cubePosition = new Vector3(x, 0, z); // y is set to 0 because all cubes have the same height

        // Instantiate the prefab
        GameObject cube = Instantiate(prefab, cubePosition, Quaternion.identity);
        cube.transform.parent = gridParent.transform;

        // Storing the cube in the grid array
        _grid[x, z].isOccupied = true;
        _grid[x, z].tilePrefab = cube;
    }


    public void DeleteCube(int x, int z)
    {
        Destroy(_grid[x, z].tilePrefab);
        _grid[x, z].tilePrefab = null;
        _grid[x, z].isOccupied = false;
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

        while(roomsCreated < numberOfRooms && maxAttemps > 0)
        {
            int roomWidth = Random.Range(minRoomWidth, maxRoomWidth);
            int roomHeight = Random.Range(minRoomHeight, maxRoomHeight);
            int startX = Random.Range(1, gridSizeX - roomWidth - 1);  // Avoiding edges
            int startZ = Random.Range(1, gridSizeY - roomHeight - 1); // Avoiding edges

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
                    GameObject wall = Instantiate(wallPrefab, new Vector3(x, 0, z), Quaternion.identity);
                    wall.transform.parent = gridParent.transform;

                    if (x == startX || x == startX + width - 1)
                        wall.transform.rotation = Quaternion.Euler(0, 90, 0);

                    _grid[x, z].isOccupied = true;
                }
                else
                {
                    if (_grid[x, z].isOccupied) Destroy(_grid[x, z].tilePrefab);
                    _grid[x, z].isOccupied = true;
                }
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

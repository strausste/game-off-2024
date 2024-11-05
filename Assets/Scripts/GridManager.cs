using UnityEngine;

public class GridManager : MonoBehaviour
{
    // ====================================================================================
    // Class attributes
    // ====================================================================================

    // TODO: Probably this is overkilled (we just need coordinates)
    private GameObject[,] _grid;
    private static GridManager _instance; // Singleton

    [Header("Grid settings")] 
    [SerializeField] private int gridSizeX = 34;
    [SerializeField] private int gridSizeY = 20;

    [Header("Prefabs")] 
    [SerializeField] private GameObject gridParent;

    [SerializeField] private GameObject debugPrefab;
    [SerializeField] private GameObject wallPrefab;

    // [SerializeField] private GameObject chestPrefab;
    // [SerializeField] private GameObject enemyPrefab;

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


    public GameObject[,] GetGrid()
    {
        return this._grid;
    }


    private void CreateGrid()
    {
        _grid = new GameObject[gridSizeX, gridSizeY];

        // Create all debug cubes
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeY; z++)
            {
                CreateCube(debugPrefab, x, z);
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
        _grid[x, z] = cube;
    }


    public void DeleteCube(int x, int z)
    {
        Destroy(_grid[x, z]);
    }
    

    /*
    private void GenerateRoom() {
        
    }

    private void PopolateRoom() {
        // parametric w.r.t. level
    }

    */
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
    }

    // ====================================================================================
}

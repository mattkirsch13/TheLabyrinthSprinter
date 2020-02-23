using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;

    public GameObject mazeObj;
    public GameObject spawn;
    public GameObject exit;
    public GameObject oneWay;
    public GameObject cornerWay;
    public GameObject straightWay;
    public GameObject threeWay;
    public GameObject fourWay1;
    public GameObject fourWay2;
    public GameObject fourWay3;
    public GameObject gate1;
    public GameObject gate2;
    public GameObject gate3;
    public GameObject gate4;

    public ButtonScript button1;
    public ButtonScript button2;
    public ButtonScript button3;

    public int mazeWidth;
    public float cellWidth;

    bool generated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ButtonScript.pressed && !generated)
        {
            mazeWidth = (int)ButtonScript.difficulty;
            generateMaze();
            generated = true;
            freeButtons();
            gate1.GetComponent<Animator>().SetBool("isOpen", true);
            gate2.GetComponent<Animator>().SetBool("isOpen", true);
            gate3.GetComponent<Animator>().SetBool("isOpen", true);
            gate4.GetComponent<Animator>().SetBool("isOpen", true);
        }
    }

    void freeButtons()
    {
        button1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        button2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        button3.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        button1.GetComponent<Rigidbody>().AddForce(getExplosionVector() * 20.0f);
        button2.GetComponent<Rigidbody>().AddForce(getExplosionVector() * 20.0f);
        button3.GetComponent<Rigidbody>().AddForce(getExplosionVector() * 20.0f);
        button1.GetComponent<Rigidbody>().AddTorque(getExplosionVector() * 10.0f);
        button2.GetComponent<Rigidbody>().AddTorque(getExplosionVector() * 10.0f);
        button3.GetComponent<Rigidbody>().AddTorque(getExplosionVector() * 10.0f);
    }   

    Vector3 getExplosionVector()
    {
        return new Vector3(Random.Range(-20.0f, 20.0f), 50.0f, Random.Range(-20.0f, 20.0f));
    }

    void generateMaze()
    {
        Debug.Log("Generating Maze");
        MazeData maze = new MazeData(mazeWidth);
        maze.generateMaze();
        
        for (int j = 0; j < mazeWidth; j++)
        {
            for (int i = 0; i < mazeWidth; i++)
            {
                if (i == Mathf.Floor(mazeWidth / 2) && j == Mathf.Floor(mazeWidth / 2))
                {
                    continue;
                }
                else
                {
                    generateRoom(maze.m_maze[i + (j * mazeWidth)], getRoomLocation(i, j));
                }
            }
        }
        // Instantiate Exit
        Vector3 exitOffset;
        float exitRotation;
        switch (maze.m_exitDirection)
        {
            case MazeData.Direction.North:
                exitOffset = new Vector3(1.0f, 0.0f, 0.0f);
                exitRotation = -90.0f;
                break;
            case MazeData.Direction.South:
                exitOffset = new Vector3(-1.0f, 0.0f, 0.0f);
                exitRotation = 90.0f;
                break;
            case MazeData.Direction.East:
                exitOffset = new Vector3(0.0f, 0.0f, -1.0f);
                exitRotation = 0.0f;
                break;
            case MazeData.Direction.West:
                exitOffset = new Vector3(0.0f, 0.0f, 1.0f);
                exitRotation = 180.0f;
                break;
            default:
                throw new System.Exception("No maze exit found");
        }
        Instantiate(exit, getExitLocation(maze, exitOffset), Quaternion.Euler(0.0f, exitRotation, 0.0f), mazeObj.transform);

        Debug.Log("Done");
    }

    // Simply returns a vector of where a room in the given index should be in the world space
    Vector3 getRoomLocation(int row, int col)
    {
        float totalWidth = mazeWidth * cellWidth;
        Vector3 startingPoint = new Vector3((totalWidth / 2) - (cellWidth / 2), 0.0f, (totalWidth / 2) - (cellWidth / 2));
        return startingPoint + (row * cellWidth * Vector3.back) + (col * cellWidth * Vector3.left);
    }

    Vector3 getExitLocation(MazeData maze, Vector3 offset)
    {
        float totalWidth = mazeWidth * cellWidth;
        Vector3 startingPoint = new Vector3((totalWidth / 2) - (cellWidth / 2), 0.0f, (totalWidth / 2) - (cellWidth / 2));
        return startingPoint + (Vector3.back * cellWidth) * (maze.m_exitConnectorIdx % mazeWidth) + (Vector3.left * cellWidth) * (maze.m_exitConnectorIdx / mazeWidth) + (offset * cellWidth);
    }

    void generateRoom(Cell cellData, Vector3 position)
    {
        int numConnections = getNumConnections(cellData);

        switch (numConnections)
        {
            case 0:
                throw new System.Exception("Cannot instantiate maze cell with no connections");
                //break;
            case 1:
                if (cellData.nConnect != null)
                {
                    var obj = Instantiate(oneWay, position, Quaternion.Euler(0, -90, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else if (cellData.sConnect != null)
                {
                    var obj = Instantiate(oneWay, position, Quaternion.Euler(0, 90, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else if (cellData.eConnect != null)
                {
                    var obj = Instantiate(oneWay, position, Quaternion.Euler(0, 0, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else if (cellData.wConnect != null)
                {
                    var obj = Instantiate(oneWay, position, Quaternion.Euler(0, 180, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else
                {
                    throw new System.Exception("Maze cell with 1 connection found, but connections do not correspond");
                }
                break;
            case 2:
                if (cellData.nConnect != null && cellData.sConnect != null)
                {
                    var obj = Instantiate(straightWay, position, Quaternion.Euler(0, 90, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else if (cellData.wConnect != null && cellData.eConnect != null)
                {
                    var obj = Instantiate(straightWay, position, Quaternion.Euler(0, 0, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else if (cellData.nConnect != null && cellData.eConnect != null)
                {
                    var obj = Instantiate(cornerWay, position, Quaternion.Euler(0, 0, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else if (cellData.eConnect != null && cellData.sConnect != null)
                {
                    var obj = Instantiate(cornerWay, position, Quaternion.Euler(0, 90, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else if (cellData.sConnect != null && cellData.wConnect != null)
                {
                    var obj = Instantiate(cornerWay, position, Quaternion.Euler(0, 180, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else if (cellData.wConnect != null && cellData.nConnect != null)
                {
                    var obj = Instantiate(cornerWay, position, Quaternion.Euler(0, -90, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else
                {
                    throw new System.Exception("Maze cell with 2 connection found, but connections do not correspond");
                }
                break;
            case 3:
                if (cellData.nConnect == null)
                {
                    var obj = Instantiate(threeWay, position, Quaternion.Euler(0, 0, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else if (cellData.sConnect == null)
                {
                    var obj = Instantiate(threeWay, position, Quaternion.Euler(0, 180, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else if (cellData.eConnect == null)
                {
                    var obj = Instantiate(threeWay, position, Quaternion.Euler(0, 90, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else if (cellData.wConnect == null)
                {
                    var obj = Instantiate(threeWay, position, Quaternion.Euler(0, -90, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else
                {
                    throw new System.Exception("Maze cell with 3 connection found, but connections do not correspond");
                }
                break;
            case 4:
                if (cellData.nConnect != null && cellData.sConnect != null && cellData.eConnect != null && cellData.wConnect != null)
                {
                    var obj = Instantiate(fourWay1, position, Quaternion.Euler(0, 0, 0));
                    obj.transform.parent = mazeObj.transform;
                }
                else
                {
                    throw new System.Exception("Maze cell with 4 connection found, but connections do not correspond");
                }
                break;
            default:
                throw new System.Exception("Error instantiating maze cell, connections is " + numConnections);
                //break;
        }
    }

    int getNumConnections(Cell celldata)
    {
        int numConnections = 0;
        if (celldata.nConnect != null)
        {
            numConnections++;
        }
        if (celldata.sConnect != null)
        {
            numConnections++;
        }
        if (celldata.eConnect != null)
        {
            numConnections++;
        }
        if (celldata.wConnect != null)
        {
            numConnections++;
        }

        return numConnections;
    }
}

public class Cell
{
    public Cell nConnect = null;
    public Cell sConnect = null;
    public Cell eConnect = null;
    public Cell wConnect = null;
    public bool visited = false;
    public bool exit = false;
}

public class MazeData
{
    public Cell m_exit = new Cell();
    public int m_exitConnectorIdx;
    public Direction m_exitDirection;
    public List<Cell> m_maze = new List<Cell>();
    private Stack<int> stack = new Stack<int>();
    private int m_size;
    private int m_width;

    public MazeData(int width)
    {
        if (width % 2 == 0)
        {
            throw new System.Exception("Maze must be an odd size");
        }
        m_width = width;
        m_size = width * width;
        m_maze.Capacity = m_size;

        for (int i = 0; i < m_size; i++)
        {
            m_maze.Add(new Cell());
        }  
        
    }

    public void generateMaze()
    {
        // Choose initial cell, Mark visited, push to stack
        int initialCellIdx = (m_size - 1) / 2;
        m_maze[initialCellIdx].visited = true;
        stack.Push(initialCellIdx);

        // While the stack is not empty
        while (stack.Count != 0)
        {
            // Pop cell, and mark as current
            int currentCellIdx = stack.Pop();

            // Check for neighbors not visisted
            int neighborCellIdx = -1;
            List<Direction> uncheckedDirections = new List<Direction> { Direction.North, Direction.South, Direction.East, Direction.West };
            Direction direction = Direction.None;
            while (true)
            {
                // Get a random direction, check if that neighbor both exists and is not visisted
                if  (uncheckedDirections.Count == 0)
                {
                    // This implies there are no valid neighbors, so we should begin backtracking
                    neighborCellIdx = -1;
                    break;
                }

                direction = uncheckedDirections[Random.Range(0, uncheckedDirections.Count)];
                neighborCellIdx = getNeighbor(currentCellIdx, direction);
                if (neighborCellIdx != -1 && !m_maze[neighborCellIdx].visited)
                {
                    break;
                }
                uncheckedDirections.Remove(direction);
            }

            // If there were no non-visisted neighbor cells, we have completed this cell and can pop the next one
            if (neighborCellIdx == -1)
            {
                continue;
            }

            // Push the chosen cell to stack
            stack.Push(currentCellIdx);

            // Remove the wall between current and chosen
            setConnection(currentCellIdx, direction);

            // Mark chosen cell as visited and push it to the stack as well
            m_maze[neighborCellIdx].visited = true;
            stack.Push(neighborCellIdx);
        }

        // Create an Exit

        // Choose which side it's on
        int side = (int)Random.Range(0, 4);
        int exitIdx = -1;
        switch (side)
        {
            case 0:
                exitIdx = (int)Random.Range(0, m_width);
                break;
            case 1:
                exitIdx = (int)Random.Range(m_size - m_width, m_size);
                break;
            case 2:
                exitIdx = m_width * (int)Random.Range(0, m_width) + m_width - 1;
                break;
            case 3:
                exitIdx = m_width * (int)Random.Range(0, m_width);
                break;
        }

        addExit(exitIdx, (Direction)side);
    }

    void addExit(int cellIdx, Direction direction)
    {
        m_exitDirection = direction;
        m_exitConnectorIdx = cellIdx;
        switch (direction)
        {
            case Direction.North:
                m_maze[cellIdx].nConnect = m_exit;
                m_maze[cellIdx].nConnect.sConnect = m_maze[cellIdx];
                break;
            case Direction.South:
                m_maze[cellIdx].sConnect = m_exit;
                m_maze[cellIdx].sConnect.nConnect = m_maze[cellIdx];
                break;
            case Direction.East:
                m_maze[cellIdx].eConnect = m_exit;
                m_maze[cellIdx].eConnect.wConnect = m_maze[cellIdx];
                break;
            case Direction.West:
                m_maze[cellIdx].wConnect = m_exit;
                m_maze[cellIdx].wConnect.eConnect = m_maze[cellIdx];
                break;
        }
    }

    void setConnection(int cellIdx, Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                m_maze[cellIdx].nConnect = m_maze[getNeighbor(cellIdx, Direction.North)];
                m_maze[cellIdx].nConnect.sConnect = m_maze[cellIdx];
                break;
            case Direction.South:
                m_maze[cellIdx].sConnect = m_maze[getNeighbor(cellIdx, Direction.South)];
                m_maze[cellIdx].sConnect.nConnect = m_maze[cellIdx];
                break;
            case Direction.East:
                m_maze[cellIdx].eConnect = m_maze[getNeighbor(cellIdx, Direction.East)];
                m_maze[cellIdx].eConnect.wConnect = m_maze[cellIdx];
                break;
            case Direction.West:
                m_maze[cellIdx].wConnect = m_maze[getNeighbor(cellIdx, Direction.West)];
                m_maze[cellIdx].wConnect.eConnect = m_maze[cellIdx];
                break;
            default:
                return;
        }
    }

    public enum Direction
    {
        North = 0,
        South = 1,
        East = 2,
        West = 3,
        None = 4
    }

    int getNeighbor(int cellIdx, Direction direction)
    {
        int idx = cellIdx;
        int row = idx / m_width;
        int col = idx % m_width;
        
        switch (direction)
        {
            case Direction.North:
                if (row == 0)
                {
                    return -1; // Maybe replace null with custom border cell?
                }
                return idx - m_width;
            case Direction.South:
                if (row == (m_width - 1))
                {
                    return -1;
                }
                return idx + m_width;
            case Direction.East:
                if (col == m_width - 1)
                {
                    return -1;
                }
                return idx + 1;
            case Direction.West:
                if (col == 0)
                {
                    return -1;
                }
                return idx - 1;
        }

        throw new System.Exception("No direction specified when finding Cell neighbor");
    }

}

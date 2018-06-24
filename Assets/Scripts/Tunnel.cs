using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will be a tunnel between two rooms.
/// </summary>
public class Tunnel {
    /// <summary>
    /// By default, all tunnels will use this value as length.
    /// </summary>
    public const int DEFAULT_LENGTH = 10;

    private Vector2[] startingPoints;
    private Dictionary<Vector2, GameObject> grid;
    private GameObject[] tunnelTiles;
    private GameObject[] wallTiles;
    Room.Exits direction;

    /// <summary>
    /// Builds a tunnel from an Array of Vector2 in a certain direction.
    /// </summary>
    /// <param name="startingPoints"></param>
    /// <param name="tunnelTiles"></param>
    /// <param name="wallTiles"></param>
    /// <param name="direction"></param>
    public Tunnel(Vector2[] startingPoints, GameObject[] tunnelTiles, GameObject[] wallTiles, Room.Exits direction)
    {
        this.startingPoints = startingPoints;
        this.tunnelTiles = tunnelTiles;
        this.wallTiles = wallTiles;
        this.direction = direction;
        this.grid = new Dictionary<Vector2, GameObject>();
        buildTunnel();
    }


    public Dictionary<Vector2, GameObject> getGrid()
    {
        return grid;
    }

    private void buildTunnel()
    {
        Vector2 newPosition;
        //control what to change in a vector (avoiding this on a cycle
        int valueDiffX = 0;
        int valueDiffY = 0;
        int[] edges = new int[2];
        switch (direction)
        {
            case Room.Exits.UP:
                edges = calculateXEdges();
                valueDiffX = 0;
                valueDiffY = 1;
                break;
            case Room.Exits.DOWN:
                edges = calculateXEdges();
                valueDiffX = 0;
                valueDiffY = -1;
                break;
            case Room.Exits.LEFT:
                edges = calculateYEdges();
                valueDiffX = -1;
                valueDiffY = 0;
                break;
            case Room.Exits.RIGHT:
                edges = calculateYEdges();
                valueDiffX = 1;
                valueDiffY = 0;
                break;
        }
        //
        foreach (Vector2 vec in startingPoints)
        {
            newPosition = new Vector2(vec.x, vec.y);
            for (int i = 0; i < DEFAULT_LENGTH; i++)
            {
                newPosition = new Vector2(newPosition.x + valueDiffX, newPosition.y + valueDiffY);
                //TO-DO make this random
                if(vec.x == edges[0] || vec.x == edges[1] || vec.y == edges[0] || vec.y == edges[1])
                    grid[new Vector2(newPosition.x, newPosition.y)] = wallTiles[0];
                else
                    grid[new Vector2(newPosition.x, newPosition.y)] = tunnelTiles[0];
            }
        }           
    }

    //0 is the left one and 1 is the right one
    private int[] calculateXEdges()
    {
        int[] edges = new int[2];
        int minimum, maximum;
        minimum = (int)startingPoints[0].x;
        maximum = (int)startingPoints[0].x;
        foreach (Vector2 vec in startingPoints)
        {
            if ((int)vec.x < minimum)
                minimum = (int)vec.x;
            if ((int)vec.x > maximum)
                maximum = (int)vec.x;
        }
        edges[0] = minimum;
        edges[1] = maximum;
        return edges;
    }

    private int[] calculateYEdges()
    {

        int[] edges = new int[2];
        int minimum, maximum;
        minimum = (int)startingPoints[0].y;
        maximum = (int)startingPoints[0].y;
        foreach (Vector2 vec in startingPoints)
        {
            if ((int)vec.y < minimum)
                minimum = (int)vec.y;
            if ((int)vec.y > maximum)
                maximum = (int)vec.y;
        }
        edges[0] = minimum;
        edges[1] = maximum;
        return edges;
    }


}

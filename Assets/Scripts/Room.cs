using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will be a room, with four possible exits and a grid of tiles.
/// </summary>
public class Room
{
    private const int EVEN_TUNNEL_WIDTH = 5;
    private const int ODD_TUNNEL_WIDTH = 6;

    private Transform levelHolder;
    private Dictionary<Vector2, GameObject> grid;
    private GameObject[] floorTiles;
    private GameObject[] wallTiles;
    private GameObject[] tunnelFloorTiles;
    private GameObject[] tunnelWallTiles;
    private int sX, sY, lenX, lenY;
    //flags
    private bool isStart, isEnd, isDeadEnd, isMain, isShop;
    /// <summary>
    ///  Possible exits.
    /// </summary>
    public enum Exits { UP, DOWN, LEFT, RIGHT }
    private Dictionary<Exits, Room> possibleExits;
    private Dictionary<Exits, Tunnel> tunnels;

    /// <summary>
    /// Builds a room with a specific X, y, x length, y length and different tiles.
    /// </summary>
    /// <param name="sX"></param>
    /// <param name="sY"></param>
    /// <param name="lenX"></param>
    /// <param name="lenY"></param>
    /// <param name="floorTiles"></param>
    /// <param name="wallTiles"></param>
    /// <param name="tunnelFloorTiles"></param>
    /// <param name="tunnelWallTiles"></param>
    public Room(int sX, int sY, int lenX, int lenY, GameObject[] floorTiles, GameObject[] wallTiles, GameObject[] tunnelFloorTiles, GameObject[] tunnelWallTiles)
    {
        this.sX = sX;
        this.sY = sY;
        auxConstructor(lenX, lenY, floorTiles, wallTiles, tunnelFloorTiles, tunnelWallTiles);
    }
    /// <summary>
    /// Builds a room with a specific Vector2(y,x), x length, y length and different tiles.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="lenX"></param>
    /// <param name="lenY"></param>
    /// <param name="floorTiles"></param>
    /// <param name="wallTiles"></param>
    /// <param name="tunnelFloorTiles"></param>
    /// <param name="tunnelWallTiles"></param>
    public Room(Vector2 start, int lenX, int lenY, GameObject[] floorTiles, GameObject[] wallTiles, GameObject[] tunnelFloorTiles, GameObject[] tunnelWallTiles)
    {
        this.sX = (int)start.x;
        this.sY = (int)start.y;
        auxConstructor(lenX, lenY, floorTiles, wallTiles, tunnelFloorTiles, tunnelWallTiles);
    }

    private void auxConstructor(int lenX, int lenY, GameObject[] floorTiles, GameObject[] wallTiles, GameObject[] tunnelFloorTiles, GameObject[] tunnelWallTiles)
    {
        this.lenX = lenX;
        this.lenY = lenY;
        this.floorTiles = floorTiles;
        this.wallTiles = wallTiles;
        this.tunnelFloorTiles = tunnelFloorTiles;
        this.tunnelWallTiles = tunnelWallTiles;
        possibleExits = new Dictionary<Exits, Room>();
        tunnels = new Dictionary<Exits, Tunnel>();
        grid = new Dictionary<Vector2, GameObject>();
        buildRoom(0);
    }


    public int getSX()
    {
        return sX;
    }

    public int getSY()
    {
        return sY;
    }

    public int getLenX()
    {
        return lenX;
    }

    public int getLenY()
    {
        return lenY;
    }

    /// <summary>
    /// Sets a room as dead end.
    /// </summary>
    public void setDeadEnd()
    {
        isDeadEnd = true;
        //prototype
        buildInnerRoom(1);
    }
    /// <summary>
    /// Sets a room as the end.
    /// </summary>
    public void setEnd()
    {
        isEnd = true;
        //prototype
        buildInnerRoom(2);
    }
    /// <summary>
    /// Sets a room as the start.
    /// </summary>
    public void setStart()
    {
        isStart = true;
        buildInnerRoom(1);
    }
    /// <summary>
    /// Sets a room as a shop.
    /// </summary>
    public void setShop()
    {
        isShop = true;
        buildInnerRoom(3);
    }

    public bool isEndRoom()
    {
        return isEnd;
    }

    public bool isStartRoom()
    {
        return isStart;
    }

    public bool isDeadEndRoom()
    {
        return isDeadEnd;
    }

    public bool isShopRoom()
    {
        return isShop;
    }

    public Dictionary<Room.Exits, Tunnel> getTunnels()
    {
        return tunnels;
    }

    public Dictionary<Vector2, GameObject> getGrid()
    {
        return grid;
    }
    /// <summary>
    /// Gets a array of Vecto2 with the coordinates of the door slots.
    /// </summary>
    /// <param name="exit"></param>
    /// <returns></returns>
    public Vector2[] getDoorSlots(Exits exit)
    {
        Vector2[] doorSlots;
        int control = 0;
        if (lenX % 2 == 0)
        {
            doorSlots = new Vector2[ODD_TUNNEL_WIDTH];
            switch (exit)
            {
                case Exits.UP:
                    for (int i = 0; i < ODD_TUNNEL_WIDTH / 2; i++)
                    {
                        doorSlots[control++] = new Vector2((sX + (lenX / 2)) + i, lenY + sY - 1);
                        doorSlots[control++] = new Vector2((sX + (lenX / 2)) - (i + 1), lenY + sY - 1);
                    }
                    break;
                case Exits.DOWN:
                    for (int i = 0; i < ODD_TUNNEL_WIDTH / 2; i++)
                    {
                        doorSlots[control++] = new Vector2((sX + (lenX / 2)) + i, sY);
                        doorSlots[control++] = new Vector2((sX + (lenX / 2)) - (i + 1), sY);
                    }
                    break;
                case Exits.LEFT:
                    for (int i = 0; i < ODD_TUNNEL_WIDTH / 2; i++)
                    {
                        doorSlots[control++] = new Vector2(sX, (sY + (lenY / 2)) + i);
                        doorSlots[control++] = new Vector2(sX, (sY + (lenY / 2)) - (i + 1));
                    }
                    break;
                case Exits.RIGHT:
                    for (int i = 0; i < ODD_TUNNEL_WIDTH / 2; i++)
                    {
                        doorSlots[control++] = new Vector2(lenX + sX - 1, (sY + (lenY / 2)) + i);
                        doorSlots[control++] = new Vector2(lenX + sX - 1, (sY + (lenY / 2)) - (i + 1));
                    }
                    break;
            }
        }
        else
        {
            doorSlots = new Vector2[EVEN_TUNNEL_WIDTH];
            switch (exit)
            {
                case Exits.UP:
                    doorSlots[control++] = new Vector2(sX + Mathf.Ceil((lenX / 2)), lenY + sY - 1);
                    for (int i = 0; i < EVEN_TUNNEL_WIDTH / 2; i++)
                    {
                        doorSlots[control++] = new Vector2(sX + Mathf.Ceil((lenX / 2)) + (i + 1), lenY + sY - 1);
                        doorSlots[control++] = new Vector2(sX + Mathf.Ceil((lenX / 2)) - (i + 1), lenY + sY - 1);
                    }
                    break;
                case Exits.DOWN:
                    doorSlots[control++] = new Vector2((sX + (lenX / 2)), sY);
                    for (int i = 0; i < EVEN_TUNNEL_WIDTH / 2; i++)
                    {
                        doorSlots[control++] = new Vector2((sX + (lenX / 2)) + (i + 1), sY);
                        doorSlots[control++] = new Vector2((sX + (lenX / 2)) - (i + 1), sY);
                    }
                    break;
                case Exits.LEFT:
                    doorSlots[control++] = new Vector2(sX, (sY + (lenY / 2)));
                    for (int i = 0; i < EVEN_TUNNEL_WIDTH / 2; i++)
                    {
                        doorSlots[control++] = new Vector2(sX, (sY + (lenY / 2)) + (i + 1));
                        doorSlots[control++] = new Vector2(sX, (sY + (lenY / 2)) - (i + 1));
                    }
                    break;
                case Exits.RIGHT:
                    doorSlots[control++] = new Vector2(lenX + sX - 1, (sY + (lenY / 2)));
                    for (int i = 0; i < EVEN_TUNNEL_WIDTH / 2; i++)
                    {
                        doorSlots[control++] = new Vector2(lenX + sX - 1, (sY + (lenY / 2)) + (i + 1));
                        doorSlots[control++] = new Vector2(lenX + sX - 1, (sY + (lenY / 2)) - (i + 1));
                    }
                    break;
            }
        }
        return doorSlots;
    }
    /// <summary>
    /// Gets the room connect on a specific exit.
    /// </summary>
    /// <param name="exit"></param>
    /// <returns></returns>
    public Room getConnectedRoom(Exits exit)
    {
        return possibleExits[exit];
    }

    private void buildRoom(int fIndex)
    {
        grid = new Dictionary<Vector2, GameObject>();
        for (int x = sX; x < lenX + sX; x++)
        {
            for (int y = sY; y < lenY + sY; y++)
            {
                //check if it's a wall
                if (x == sX || x == lenX - 1 + sX || y == sY || y == lenY - 1 + sY)
                {
                    grid[new Vector2(x, y)] = wallTiles[0];
                }
                else
                {
                    grid[new Vector2(x, y)] = floorTiles[fIndex];
                }

            }
        }
    }

    private void buildInnerRoom(int fIndex)
    {
        for (int x = sX; x < lenX + sX; x++)
        {
            for (int y = sY; y < lenY + sY; y++)
            {
                //check if it's a wall
                if (!(x == sX || x == lenX - 1 + sX || y == sY || y == lenY - 1 + sY))
                    grid[new Vector2(x, y)] = floorTiles[fIndex];
            }
        }
    }
    /// <summary>
    /// Generates the door slots, replacing walls with floor tiles.
    /// </summary>
    /// <param name="exit"></param>
    public void generateDoor(Exits exit)
    {
        foreach (Vector2 doorCoord in getDoorSlots(exit))
        {
            //replace for tunnel entrance
            grid[doorCoord] = floorTiles[0];
        }
    }

    public void removeDoor(Exits exit)
    {
        foreach (Vector2 doorCoord in getDoorSlots(exit))
        {
            //replace for wall
            grid[doorCoord] = wallTiles[0];
        }
    }

    /// <summary>
    /// Connects a room to another room in a specific direction.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="exit"></param>
    public void connectRoom(Room room, Exits exit)
    {
        //add room to the exit list
        addExitRoom(room, exit);
        tunnels[exit] = new Tunnel(getDoorSlots(exit), tunnelFloorTiles, tunnelWallTiles, exit);
        //generate door for the exit
        generateDoor(exit);
        //add this room to the other room list
        room.addExitRoom(this, oppositeDirection(exit));
        //generate door on the opposite direction
        room.generateDoor(oppositeDirection(exit));
    }

    protected void addExitRoom(Room room, Exits exit)
    {
        possibleExits[exit] = room;
    }

    private Exits oppositeDirection(Exits exit)
    {
        switch (exit)
        {
            case Exits.DOWN:
                return Exits.UP;
            case Exits.UP:
                return Exits.DOWN;
            case Exits.LEFT:
                return Exits.RIGHT;
            case Exits.RIGHT:
                return Exits.LEFT;
            default:
                Debug.Log("Something went wrong");
                return Exits.UP;
        }
    }

    public Vector2 getNextRoomCoordinate(Exits exit)
    {
        int newSX = 0;
        int newSY = 0; //coordinates for the next room
        //5 é o comprimento do tunel, TODO change this
        switch (exit)
        {
            case Exits.UP:
                newSX = sX;
                newSY = sY + lenY + Tunnel.DEFAULT_LENGTH;
                break;
            case Exits.DOWN:
                newSX = sX;
                newSY = sY - lenY - Tunnel.DEFAULT_LENGTH;
                break;
            case Exits.LEFT:
                newSX = sX - lenX - Tunnel.DEFAULT_LENGTH;
                newSY = sY;
                break;
            case Exits.RIGHT:
                newSX = sX + lenX + Tunnel.DEFAULT_LENGTH;
                newSY = sY;
                break;
        }
        return new Vector2(newSX, newSY);
    }

    /// <summary>
    /// Picks a random coordinate inside the room, with a specific margin from the right and up side of the room.
    /// </summary>
    /// <param name="marginX"></param>
    /// <param name="marginY"></param>
    /// <returns></returns>
    public Vector2 getRandomInnerCoord(int marginX, int marginY)
    {
        int coordX = Random.Range(sX + 1, sX + lenX - (marginX + 1));
        int coordY = Random.Range(sY + 1, sY + lenY - (marginY + 1));
        return new Vector2(coordX, coordY);
    }
}

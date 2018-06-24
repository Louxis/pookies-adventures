using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that will implement the random algorithm to generate a level.
/// </summary>
public class LevelBuilder {

    int x, y;
    private Room[,] roomGrid;
    //amount of rooms for the main path
    private const int MIN_MAIN_LENGTH = 3;
    private const int MAX_MAIN_LENGTH = 6;
    private const int ROOM_SIZE = 16;

    private GameObject[] floorTiles;
    private GameObject[] wallTiles;
    private GameObject[] tunnelFloorTiles;
    private GameObject[] tunnelWallTiles;

    /// <summary>
    /// Builds a level builder with a grid of X,Y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public LevelBuilder(int x,int y)
    {
        this.x = x;
        this.y = y;
        roomGrid = new Room[x, y];
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public void setFloorTiles(GameObject[] floorTiles)
    {
        this.floorTiles = floorTiles;
    }

    public void setWallTiles(GameObject[] wallTiles)
    {
        this.wallTiles = wallTiles;
    }

    public void setTunnelFloorTiles(GameObject[] tunnelFloorTiles)
    {
        this.tunnelFloorTiles = tunnelFloorTiles;
    }

    public void setTunnelWallTiles(GameObject[] tunnelWallTiles)
    {
        this.tunnelWallTiles = tunnelWallTiles;
    }

    public Room[,] getRoomGrid()
    {
        return roomGrid;
    }

    private int currentX = 0;
    private int currentY = 0;
    /// <summary>
    /// Builds the level grid.
    /// </summary>
    public void buildGrid()
    {
        Room startingRoom;
        //create first room
        startingRoom = generateStartingRoom();
        //create main path
        Room[] mainPath = generateMainPath(startingRoom);
        //create optional dead end
        Room[] deadEnd = generateDeadEnd(startingRoom, mainPath);
        //create a shop in the level
        Room shop = generateShop(startingRoom, mainPath, deadEnd);
    }
    //generate shop room, will always be 1 per level.
    private Room generateShop(Room start, Room[] main, Room[] deadEnd)
    {
        int control = 0;
        Room shop;
        Room forkRoom = pickRandomForkShop(start, main, deadEnd);
        Room.Exits randomExit = getRandomDirection();
        currentX = (int)getRoomPosition(forkRoom).x;
        currentY = (int)getRoomPosition(forkRoom).y;
        while (!isPossible(currentX, currentY, randomExit))
        {
            //control variable to avoid infinite loops
            control++;
            if (control == 30)
            {
                Debug.Log("I failed shop");
                return generateShop(start, main, deadEnd);
            }
            randomExit = getRandomDirection();
        }
        shop = new Room(forkRoom.getNextRoomCoordinate(randomExit), ROOM_SIZE, ROOM_SIZE, floorTiles, wallTiles, tunnelFloorTiles, tunnelWallTiles);
        forkRoom.connectRoom(shop, randomExit);
        updateGrid(currentX, currentY, shop, randomExit);
        shop.setShop();
        return shop;
    }
    //picks a random room to fork to a shop.
    private Room pickRandomForkShop(Room start, Room[] main, Room[] deadEnd)
    {
        Room forkRoom;
        switch (Random.Range(0, 3))
        {
            case 0:
                forkRoom = start;
                break;
            case 1:
                forkRoom = main[Random.Range(0, main.Length)];
                break;
            case 2:
                forkRoom = deadEnd[Random.Range(0, deadEnd.Length)];
                break;
            default:
                Debug.Log("something went wrong with pickRandomForkShop");
                forkRoom = start;
                break;
        }
        return forkRoom;
    }

    private bool slotHasRoom(int x, int y)
    {
        return roomGrid[x,y] is Room;
    }

    private Room generateStartingRoom()
    {
        currentX = Random.Range(1, x-1);
        currentY = Random.Range(1, y-1);
        Room startingRoom = new Room(0, 0, ROOM_SIZE, ROOM_SIZE, floorTiles, wallTiles, tunnelFloorTiles, tunnelWallTiles);
        updateGrid(currentX, currentY, startingRoom);
        startingRoom.setStart();
        return startingRoom;
    }

    private Room[] generateMainPath(Room currentRoom)
    {
        Room newRoom;
        int length = Random.Range(MIN_MAIN_LENGTH, MAX_MAIN_LENGTH + 1);
        Room[] mainPath = new Room[length];
        for (int i = 0; i < length; i++)
        {
            Room.Exits randomExit = getRandomDirection();
            int control = 0;
            while (!isPossible(currentX, currentY, randomExit))
            {
                //control variable to avoid infinite loops
                control++;
                if (control == 30)
                    break;
                randomExit = getRandomDirection();
            }
            newRoom = new Room(currentRoom.getNextRoomCoordinate(randomExit), ROOM_SIZE, ROOM_SIZE, floorTiles, wallTiles, tunnelFloorTiles, tunnelWallTiles);
            currentRoom.connectRoom(newRoom, randomExit);
            updateGrid(currentX, currentY, newRoom, randomExit);
            currentRoom = newRoom;
            mainPath[i] = currentRoom;
        }
        currentRoom.setEnd();
        return mainPath;
    }


    private Room[] generateDeadEnd(Room start, Room[] main)
    {
        int deadEndLength = Random.Range(1, 4);
        Room[] deadEnd = new Room[deadEndLength];
        Room newRoom;
        int totalRooms = main.Length;
        int forkRoomIndex = Random.Range(0, totalRooms);
        Room forkRoom;
        if (forkRoomIndex >= main.Length - 1)
            forkRoom = start;
        else
            forkRoom = main[forkRoomIndex];
        //update current coords to the fork room
        currentX = (int)getRoomPosition(forkRoom).x;
        currentY = (int)getRoomPosition(forkRoom).y;
        for (int i = 0; i < deadEndLength; i++)
        {            
            Room.Exits randomExit = getRandomDirection();
            int control = 0;
            while (!isPossible(currentX, currentY, randomExit))
            {
                //control variable to avoid infinite loops
                control++;
                if (control == 30)
                {
                    Debug.Log("I failed");
                    break;
                }
                    
                randomExit = getRandomDirection();
            }
            if(control == 30)
            {
                //end the dead end sooner
                break;
            }
            newRoom = new Room(forkRoom.getNextRoomCoordinate(randomExit), ROOM_SIZE, ROOM_SIZE, floorTiles, wallTiles, tunnelFloorTiles, tunnelWallTiles);
            forkRoom.connectRoom(newRoom, randomExit);
            updateGrid(currentX, currentY, newRoom, randomExit);
            forkRoom = newRoom;
            forkRoom.setDeadEnd();
            deadEnd[i] = forkRoom;
        }
        return deadEnd;
    }

    private Vector2 getRoomPosition(Room room)
    {
        if (room == null)
        {
            Debug.Log("Something went wrong with getRoomPosition");
            return new Vector2(0, 0);
        }
        for(int x = 0; x < this.x; x++)
        {
            for (int y = 0; y < this.y; y++)
            {
                //Debug.Log(roomGrid[x, y].getSX());
                if (roomGrid[x, y] is Room && roomGrid[x, y] != null)
                {
                    if (roomGrid[x, y].getSX() == room.getSX() && roomGrid[x, y].getSY() == room.getSY())
                        return new Vector2(x, y);
                }
            }
        }
        Debug.Log("Something went wrong with getRoomPosition");
        return new Vector2(0, 0);
    }

    private void updateGrid(int x, int y, Room room)
    {
        currentX = x;
        currentY = y;
        roomGrid[x, y] = room;
    }

    private void updateGrid(int x, int y, Room room, Room.Exits exit)
    {
        switch (exit)
        {
            case Room.Exits.UP:
            case Room.Exits.DOWN:
                x = updateX(x, exit);
                break;
            case Room.Exits.LEFT:
            case Room.Exits.RIGHT:
                y = updateY(y, exit);
                break;
        }
        updateGrid(x,y,room);
    }

    private int updateX(int x, Room.Exits exit)
    {
        switch (exit)
        {
            case Room.Exits.UP:
                return ++x;
            case Room.Exits.DOWN:
                return --x;
        }
        Debug.Log("Something went wrong with updateX");
        return 0;
    }

    private int updateY(int y, Room.Exits exit)
    {
        switch (exit)
        {
            case Room.Exits.LEFT:
                return --y;
            case Room.Exits.RIGHT:
                return ++y;
        }
        Debug.Log("Something went wrong with updateY");
        return 0;
    }

    private bool isPossible(int x, int y, Room.Exits exit)
    {
        switch (exit)
        {
            case Room.Exits.UP:
            case Room.Exits.DOWN:
                x = updateX(x, exit);
                break;
            case Room.Exits.LEFT:
            case Room.Exits.RIGHT:
                y = updateY(y, exit);
                break;
        }
        return x >= 0 && x < this.x && y >= 0 && y < this.y && !slotHasRoom(x,y);
    }

    private Room.Exits getRandomDirection()
    {
       return (Room.Exits)(System.Enum.GetValues(typeof(Room.Exits))).GetValue(Random.Range(0,4));
    }
}

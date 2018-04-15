using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class will be responsible of rendering rooms and tunnels.
/// </summary>
public class RoomGenerator : MonoBehaviour {

    //object holder
    private Transform levelHolder;
    //additional objects to spawn
    private GameObject exit;
    private GameObject pookie;
    private GameObject[] enemies;
    private GameObject coin;
    private GameObject[] destructables;
    private GameObject[] mods;

    public void setExit(GameObject exit)
    {
        this.exit = exit;
    }

    public void setPookie(GameObject pookie)
    {
        this.pookie = pookie;
    }

    public void setEnemies(GameObject[] enemies)
    {
        this.enemies = enemies;
    }

    public void setCoin(GameObject coin)
    {
        this.coin = coin;
    }

    public void setDestructables(GameObject[] destructables)
    {
        this.destructables = destructables;
    }

    public void setMods(GameObject[] mods)
    {
        this.mods = mods;
    }

    public void renderLevel(Room[,] level, int sizeX, int sizeY)
    {
        levelHolder = new GameObject("Board").transform;
        for (int x = 0; x < sizeX; x++)
        {
            for(int y = 0; y < sizeY; y++)
            {
                if(level[x,y] is Room)
                    renderRoom(level[x, y]);
            }
        }
    }

    //TO-DO make code cleaner and less duplicated :(
    public void renderTunnel(Tunnel tunnel)
    {
        foreach (KeyValuePair<Vector2, GameObject> entry in tunnel.getGrid())
        {
            // do something with entry.Value or entry.Key
            GameObject instance = Instantiate(entry.Value, entry.Key, Quaternion.identity) as GameObject;
            instance.transform.SetParent(levelHolder);
        }
    }

    public void renderRoom(Room room)
    {
        GameObject instance;
        foreach (KeyValuePair<Vector2, GameObject> entry in room.getGrid())
        {
            // do something with entry.Value or entry.Key
            instance = Instantiate(entry.Value, entry.Key , Quaternion.identity) as GameObject;
            instance.transform.SetParent(levelHolder);
            
        }
        foreach (KeyValuePair<Room.Exits,Tunnel> tunnel in room.getTunnels())
        {
            renderTunnel(tunnel.Value);
        }
        float coordX, coordY;
        if (room.isEndRoom())
        {
            coordX = room.getSX() + room.getLenX() / 2 - 0.5f;
            coordY = room.getSY() + room.getLenY() / 2 - 0.5f;
            instance = Instantiate(exit, new Vector2(coordX, coordY), Quaternion.identity) as GameObject;
            instance.transform.SetParent(levelHolder);
        } else if (room.isStartRoom())
        {
            coordX = room.getSX() + room.getLenX() / 2 - 0.5f;
            coordY = room.getSY() + room.getLenY() / 2 - 0.5f;
            instance = Instantiate(pookie, new Vector2(coordX, coordY), Quaternion.identity) as GameObject;
            instance.transform.SetParent(levelHolder);
        } else if (room.isShopRoom())
        {
            coordX = room.getSX() + room.getLenX() / 2 - 0.5f;
            coordY = room.getSY() + room.getLenY() / 2 - 0.5f;
            instance = Instantiate(mods[Random.Range(0, mods.Length)], new Vector2(coordX - 2, coordY - 2), Quaternion.identity) as GameObject;
            instance.transform.SetParent(levelHolder);
            instance = Instantiate(mods[Random.Range(0, mods.Length)], new Vector2(coordX + 2, coordY + 2), Quaternion.identity) as GameObject;
            instance.transform.SetParent(levelHolder);
        }
        else
        {
            int numberEnemies = Random.Range(1, 4);
            int numberCoin = Random.Range(0, 2);
            int numberDestructables = Random.Range(1, 3);
            ArrayList filledPositions = new ArrayList();
            Vector2 spawnPosition;
            for (int i = 0; i < numberEnemies; i++)
            {
                spawnPosition = room.getRandomInnerCoord(2, 2);
                while (filledPositions.Contains(spawnPosition))
                {
                    spawnPosition = room.getRandomInnerCoord(2, 2);
                }
                filledPositions.Add(spawnPosition);
                instance = Instantiate(enemies[0], spawnPosition , Quaternion.identity) as GameObject;
                instance.transform.SetParent(levelHolder);
            }
            for (int i = 0; i < numberCoin; i++)
            {
                spawnPosition = room.getRandomInnerCoord(2, 2);
                while (filledPositions.Contains(spawnPosition))
                {
                    spawnPosition = room.getRandomInnerCoord(2, 2);
                }
                filledPositions.Add(spawnPosition);
                instance = Instantiate(coin, spawnPosition, Quaternion.identity) as GameObject;
                instance.transform.SetParent(levelHolder);
            }

            for (int i = 0; i < numberDestructables; i++)
            {
                spawnPosition = room.getRandomInnerCoord(2, 2);
                while (filledPositions.Contains(spawnPosition))
                {
                    spawnPosition = room.getRandomInnerCoord(2, 2);
                }
                filledPositions.Add(spawnPosition);
                instance = Instantiate(destructables[Random.Range(0, destructables.Length)], spawnPosition, Quaternion.identity) as GameObject;
                instance.transform.SetParent(levelHolder);
            }
        }
    }
}

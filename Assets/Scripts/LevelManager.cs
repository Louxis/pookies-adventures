using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Manages the level generator, builder and switches between the game and the menu.
/// </summary>
public class LevelManager : MonoBehaviour
{
    private LevelBuilder builder;
    public RoomGenerator generator;

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] tunnelFloorTiles;
    public GameObject[] tunnelWallTiles;
    public GameObject[] enemies;
    public GameObject exit;
    public GameObject pookie;
    public GameObject coin;
    public GameObject[] destructables;
    public GameObject[] mods;


    // Use this for initialization
    void Start()
    {
        generatorSetup();
        builderSetup();
    }
    /// <summary> Starts a new Level. </summary>
    public void LevelStart()
    {
        generator.renderLevel(builder.getRoomGrid(),builder.getX(),builder.getY());        
    }

    public void DestroyLevel()
    {
        Destroy(GameObject.Find("Board"));
    }

    /// <summary> Goes to the next Level. </summary>
    public void NextLevel()
    {
        PookieController pookie = GameObject.FindGameObjectWithTag("Player").GetComponent<PookieController>();
        DestroyLevel();
        builderSetup();
        LevelStart();        

    }
    //sets up the builder
    private void builderSetup()
    {
        builder = new LevelBuilder(6, 6);
        builder.setFloorTiles(floorTiles);
        builder.setWallTiles(wallTiles);
        builder.setTunnelFloorTiles(tunnelFloorTiles);
        builder.setTunnelWallTiles(tunnelWallTiles);
        builder.buildGrid();
    }
    //sets up the generator
    private void generatorSetup()
    {
        generator.setExit(exit);
        generator.setPookie(pookie);
        generator.setEnemies(enemies);
        generator.setCoin(coin);
        generator.setDestructables(destructables);
        generator.setMods(mods);
    }	

    public LevelBuilder getBuilder()
    {
        return builder;
    }
}

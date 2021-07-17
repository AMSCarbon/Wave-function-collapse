using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildNeighbourLists : MonoBehaviour
{

    public Tilemap tilemap;
    public Dictionary<int, SingleTile> tileDictionary;
    public GameObject displayPrefab;

    public List<SingleTile> Run() {
        tilemap.CompressBounds();
        tileDictionary = new Dictionary<int, SingleTile>();
        initialiseTiles();
        determineFrequency();
        determineNeighbours();
        createDisplayObjects();
        return makeReturnList();
    }

    private List<SingleTile> makeReturnList()
    {
        List<SingleTile> tileList = new List<SingleTile>();
        foreach (int key in tileDictionary.Keys)
        {
            tileList.Add(tileDictionary[key]);
        }
        return tileList;
    }

    // debug tool while creating the neighbour lists. Have a game object that picks out the data from the SingleTile Class into lists. 
    // Should be able to see these in the editor. 
    private void createDisplayObjects()
    {
        foreach (int key in tileDictionary.Keys) {
            GameObject display = Instantiate(displayPrefab);
            display.GetComponent<SingleTileDisplay>().setData(tileDictionary[key]);
        }
    }

    //Loop through the map once and enter all the tiles into a hash map. 
    private void initialiseTiles() {

        for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
        {
            for (int y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++)
            {
                Sprite tileSprite = tilemap.GetSprite(new Vector3Int(x, y, 0));
                if (!tileDictionary.ContainsKey(tileSprite.GetHashCode())) {
                    tileDictionary.Add(tileSprite.GetHashCode(), new SingleTile(tilemap.GetTile<Tile>(new Vector3Int(x, y, 0))));
                }
            }
        }
    }

    private void determineFrequency() {
        for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
        {
            for (int y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++)
            {
                int currHash = tilemap.GetSprite(new Vector3Int(x, y, 0)).GetHashCode();
                SingleTile currTile = (SingleTile)tileDictionary[currHash];
                currTile.count++;
            }
        }
    }


    //loop through each tile again. For each tile and the valid n/s/e/w directions, add the neighbouring tile as a valid possibility.
    private void determineNeighbours() {
        for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
        {
            for (int y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++)
            {
                int currHash = tilemap.GetSprite(new Vector3Int(x, y, 0)).GetHashCode();
                SingleTile currTile =  (SingleTile)tileDictionary[currHash];
                foreach (Vector2Int dir in validDirs(x,y)) {
                    currTile.addNeibhour(dir, tilemap.GetTile<Tile>(new Vector3Int(x + dir.x, y + dir.y, 0)));
                }
            }
        }
    }

    private List<Vector2Int> validDirs(int x, int y)
    {
        List<Vector2Int> dirs = new List<Vector2Int>();
        if (x - 1 > tilemap.cellBounds.min.x) {
            dirs.Add(new Vector2Int(- 1, 0));
        }
        if (y - 1 > tilemap.cellBounds.min.y)
        {
            dirs.Add(new Vector2Int(0, -1));
        }
        if (x + 1 < tilemap.cellBounds.max.x)
        {
            dirs.Add(new Vector2Int( 1, 0));
        }
        if (y + 1 < tilemap.cellBounds.max.y)
        {
            dirs.Add(new Vector2Int(0, 1));
        }
        return dirs;
    }

}

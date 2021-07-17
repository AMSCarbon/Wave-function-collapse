using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class WFCMap : MonoBehaviour
{
    public int rows;
    public int cols;
    public Tile error;
    public int inputTileCount; 

    WFCTile[,] tiles;

    // if any tile is not collapsed, the map is not collapsed. 
    public bool isCollapsed() {
        bool result = true;
        for (int x = 0; x < cols; x++)
        {
            for ( int y = 0; y < rows; y++)
            {
                result = result && tiles[x, y].tileCollapsed();
            }
        }
        return result;
    }


    public void initialise(int x, int y, List<SingleTile> options) {
        tiles = new WFCTile[x, y];
        cols = x;
        rows = y;
        for (x = 0; x < cols; x++) {
            for (y = 0; y < rows; y ++) {
                tiles[x, y] = new WFCTile(options);
            }
        }
    }


    //finds tile with the lowest entropy, if multiple exist return random chosen from all. 
    public Vector2Int lowestEntropyTile()
    {
        Dictionary<int, List<Vector2Int>> entropyGroupedTiles = new Dictionary<int, List<Vector2Int>>();
        int minEntropy = int.MaxValue;
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (tiles[x, y].tileCollapsed()) continue;

                int currentEntropy = tiles[x, y].getEntropy();
                //add current tile to hashmap.
                if (!entropyGroupedTiles.ContainsKey(currentEntropy))
                {
                    entropyGroupedTiles.Add(currentEntropy, new List<Vector2Int>());
                }
                entropyGroupedTiles[currentEntropy].Add(new Vector2Int(x, y));

                //update lowest entropy 
                minEntropy = currentEntropy < minEntropy ? currentEntropy : minEntropy;
            }
        }

        if (entropyGroupedTiles[minEntropy].Count == 1)
        {
            return entropyGroupedTiles[minEntropy][0];
        }
        else
        {
            // if there's multiple with the same entropy, return random one;
            return entropyGroupedTiles[minEntropy][UnityEngine.Random.Range(0, entropyGroupedTiles[minEntropy].Count)];
        }

    }

    public void collapseTile(int x, int y)
    {
        tiles[x, y].collapse(this);
    }

    public void propogateCollapse(int x, int y)
    {
        //Creat the stack and add the first element on.
        Stack<Vector2Int> toExpand = new Stack<Vector2Int>();
        toExpand.Push(new Vector2Int(x, y));

        while (toExpand.Count > 0) {
            //Get the top of the stack.
            Vector2Int currentCoordinate = toExpand.Pop();

            foreach (Vector2Int dir in validDirections(currentCoordinate.x, currentCoordinate.y)) {
                //Make the new coordinate by adding the direction.
                Vector2Int otherCoordinate = currentCoordinate + dir;
                List<SingleTile> otherPossibleStates = new List<SingleTile>(tiles[otherCoordinate.x, otherCoordinate.y].getStatesList());
                List<Tile> possibleNeighbourStates = tiles[currentCoordinate.x, currentCoordinate.y].getNeighboursInDirection(dir);
               
                // if the tile has already been collapsed, we don't need to bother with it. 
                if (tiles[otherCoordinate.x, otherCoordinate.y].tileCollapsed()) continue;


                // Determine the states which need to be removed. 
               
                foreach (SingleTile state in otherPossibleStates) {
                    if (!possibleNeighbourStates.Contains(state.getTile())) {
                        tiles[otherCoordinate.x, otherCoordinate.y].removeState(state);
                        if (!toExpand.Contains(otherCoordinate))
                        {
                            toExpand.Push(otherCoordinate);
                        }
                    }
                }
                //If there's no neighbours left, error out, we buggered up.
                //This can happen from time to time, the algorithm doesn't have any forward checking.
                if (tiles[otherCoordinate.x, otherCoordinate.y].getStatesList().Count == 0)
                {
                    tiles[otherCoordinate.x, otherCoordinate.y].errorTile = error;
                }
            }
        }
    } 


    private List<Vector2Int> validDirections(int x, int y)
    {
        List<Vector2Int> dirs = new List<Vector2Int>();
        if (x - 1 >= 0)
        {
            dirs.Add(new Vector2Int(-1, 0));
        }
        if (y - 1 >= 0)
        {
            dirs.Add(new Vector2Int(0, -1));
        }
        if (x + 1 < cols)
        {
            dirs.Add(new Vector2Int(1, 0));
        }
        if (y + 1 < rows)
        {
            dirs.Add(new Vector2Int(0, 1));
        }
        return dirs.OrderBy(a => Guid.NewGuid()).ToList();
    }


    public Tile getTile(int x, int y) {
        return tiles[x, y].getTileDisplay();
    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class NeighbourList
{

    List<Tile> northNeighbours;
    List<Tile> southNeighbours;
    List<Tile> eastNeighbours;
    List<Tile> westNeighbours;

    public NeighbourList()
    {
        northNeighbours = new List<Tile>();
        southNeighbours = new List<Tile>();
        eastNeighbours = new List<Tile>();
        westNeighbours = new List<Tile>();
    }

    public void addNeibhour(Vector2Int dir, Tile s)
    {
        if (dir.x == 1 && !eastNeighbours.Contains(s)) {
            eastNeighbours.Add(s);
        }
        else if (dir.x == -1 && !westNeighbours.Contains(s)) {
            westNeighbours.Add(s);
        }
        else if (dir.y == 1 && !northNeighbours.Contains(s)) {
            northNeighbours.Add(s);
        }
        else if (dir.y == -1 && !southNeighbours.Contains(s))
        {
            southNeighbours.Add(s);
        }
    }

    public List<List<Tile>> GetTileLists() {
        List<List<Tile>> n = new List<List<Tile>>();
        n.Add(northNeighbours);
        n.Add(southNeighbours);
        n.Add(eastNeighbours);
        n.Add(westNeighbours);
        return n;
    }

    public List<List<Sprite>> GetSpriteLists()
    {
        List<List<Sprite>> all = new List<List<Sprite>>();

        all.Add(TileToSpriteList(northNeighbours));
        all.Add(TileToSpriteList(southNeighbours));
        all.Add(TileToSpriteList(eastNeighbours));
        all.Add(TileToSpriteList(westNeighbours));

        return all;
    }

    private List<Sprite> TileToSpriteList(List<Tile> tileList) {
        List<Sprite> l = new List<Sprite>();
        foreach (Tile t in tileList) {
            l.Add(t.sprite);
        }
        return l;
    }

    public List<Tile> getNeighboursInDirection(Vector2Int dir)
    {
        if (dir.x == 1 )
        {
          return  eastNeighbours;
        }
        else if (dir.x == -1 )
        {
            return westNeighbours;
        }
        else if (dir.y == 1  )
        {
            return northNeighbours;
        }
        else 
        {
            return southNeighbours;
        }
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SingleTile 
{
    Tile state;
    NeighbourList neibhours;
    public int count;   

    public SingleTile(Tile s) {
        state = s; 
        neibhours = new NeighbourList();
    }

    public void addNeibhour(Vector2Int dir, Tile s) {
        neibhours.addNeibhour(dir, s);
    }

    public Tile getTile() {
        return state;
    }

    public Sprite GetSprite() {
        return state.sprite;
    }

    public List<List<Tile>> getNeighbours() {

        return neibhours.GetTileLists();
    }

    public List<List<Sprite>> getNeighbourSprites()
    {
        return neibhours.GetSpriteLists();
    }

    public List<Tile> getNeighboursInDirection(Vector2Int dir) {
        return neibhours.getNeighboursInDirection(dir);
    }
}

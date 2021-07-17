using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTileDisplay : MonoBehaviour
{

    public SingleTile data;
    public Sprite s;

    public List<Sprite> northNeighbours;
    public List<Sprite> southNeighbours;
    public List<Sprite> eastNeighbours;
    public List<Sprite> westNeighbours;


    public void setData(SingleTile tile) {
        data = tile;
        s = data.GetSprite();
        List<List<Sprite>>  neighbours = data.getNeighbourSprites();
        northNeighbours = neighbours[0];
        southNeighbours= neighbours[1];
        eastNeighbours = neighbours[2];
        westNeighbours = neighbours[3];
    }
}

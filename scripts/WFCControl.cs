using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class WFCControl : MonoBehaviour
{

    List<SingleTile> options;
    public int targetRows;
    public int targetColumns;
    public Tilemap inputMap;
    public Tilemap newMap;
    public int totalOptions;
    int frameCounter = 0 ;
    public int frameRate = 300;
    WFCMap wfcMap;
    bool initialising = true;

    void Start()
    {
        options = GetComponent<BuildNeighbourLists>().Run();
        totalOptions = options.Count;
        newMap.size = new Vector3Int(targetColumns, targetRows, 1);
        newMap.origin = Vector3Int.zero;
        wfcMap = GetComponent<WFCMap>();
        wfcMap.inputTileCount = inputMap.cellBounds.max.x * inputMap.cellBounds.max.y;
    }

    public void runWFC() {
        initialising = true;
        wfcMap.initialise(targetColumns, targetRows, options);
        initialising = false;

    }
    private void fillMap()
    {
       while (! wfcMap.isCollapsed() ) {
            iterateCollapse();
       }
    }

    private void iterateCollapse() {
        
        Vector2Int lowestEntropy = wfcMap.lowestEntropyTile();
        wfcMap.collapseTile(lowestEntropy.x, lowestEntropy.y);
        wfcMap.propogateCollapse(lowestEntropy.x, lowestEntropy.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (initialising) return;
        //collapse the next state.
    
        if (!wfcMap.isCollapsed())
        {
            iterateCollapse();
        }

        //update the display.
        for (int x = newMap.cellBounds.min.x; x < newMap.cellBounds.max.x; x++)
        {
            for (int y = newMap.cellBounds.min.y; y < newMap.cellBounds.max.y; y++)
            {
                newMap.SetTile(new Vector3Int(x, y, 0), wfcMap.getTile(x+ Math.Abs(newMap.cellBounds.min.x), y + Math.Abs(newMap.cellBounds.min.y)));
            }
        }
    }


    // randomly fills in the map with tiles from the list of options.
    private void placeTiles()
    {
        totalOptions = options.Count;
        int index = 0;
        for (int x = newMap.cellBounds.min.x; x < newMap.cellBounds.max.x; x++)
        {
            for (int y = newMap.cellBounds.min.y; y < newMap.cellBounds.max.y; y++)
            {
                newMap.SetTile(new Vector3Int(x, y, 0), options[index].getTile());
                index++;
                if (index == totalOptions)
                {
                    index = 0;
                }
            }
        }
    }
}

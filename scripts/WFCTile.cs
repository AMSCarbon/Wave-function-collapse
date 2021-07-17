using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WFCTile
{
    List<SingleTile> possibleStates;
    public Tile errorTile;
    private int entropy; 


    public WFCTile(List<SingleTile> options) {
        //Don't use the list directly cause it gets impacted when other tiles are reduced.
        possibleStates = new List<SingleTile>(options);
        updateEntropy();
    }

    public bool tileCollapsed() {
        return possibleStates.Count <= 1;
    }

    // Pick a random tile from the list of tiles that can be used.
    public void collapse(WFCMap map ) {
        SingleTile chosenTile = chooseTile();
        possibleStates = new List<SingleTile>();
        possibleStates.Add(chosenTile);
    }

    private SingleTile chooseTile()
    {
        int index = Random.Range(0, possibleStates.Count);
        int sum = 0;
        foreach (SingleTile state in possibleStates) {
            sum += state.count;
        }
        List<float> probability = new List<float>();
        foreach (SingleTile state in possibleStates)
        {
            //I'm paranoid about C# lmao
           probability.Add(((float)state.count/(float)sum));
        }
        return possibleStates[ChooseIndex(probability.ToArray())];
    }

    // from unity docs
    private int ChooseIndex(float[] probs)
    {
        float total = 0;
        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }

    private void updateEntropy() {
        int neighbourSum = 0;
        foreach (SingleTile state in possibleStates)
        {
            foreach (List<Tile> neighbourList in state.getNeighbours())
            {
                neighbourSum += neighbourList.Count;
            }
        }
         entropy = possibleStates.Count + neighbourSum;
    }
    // shooooould be an int because current implementation makes a hashmap with entropy as the key. float isn't reliable.
    // The total number of states plus the total number of possible neighbour states.
    // Assumes now that entropy is updated when states are removed;
    public int getEntropy() {
        return entropy;
    }

    public Tile getTileDisplay() {
        return chooseTile().getTile();
    }

    public List<SingleTile> getStatesList() {
        return possibleStates;
    }

    public void setStateList(List<SingleTile> newStates)
    {
        possibleStates = new List<SingleTile>(newStates);
        updateEntropy();
    }

    public void removeState(SingleTile state) {
        possibleStates.Remove(state);
        updateEntropy();
    }

    public List<Tile> getNeighboursInDirection(Vector2Int dir) {
        List<Tile> allNeighbours = new List<Tile>();
        foreach (SingleTile state in possibleStates) {
            allNeighbours.AddRange(state.getNeighboursInDirection(dir));
        }
        return allNeighbours;
    }
}

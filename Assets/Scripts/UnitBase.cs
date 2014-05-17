using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBase : MonoBehaviour {

	public animalPhyla phyla;
	public int hp, maxFuel, upKeep, maxAmmo, moveDist;
	public GameObject tile;
	protected int playerIndex,hiSlots, midSlots, lowSlots, intelligence, complexity, buildCost;
	public int[] _moveCost;
	public Dictionary<terrainType, int> moveCost;
	List<GameObject> tilesInMoveRange;


	public void Start(){
		//loadMoveCost ();

		tile = GameObject.Find("19, 10");
		tilesInMoveRange = tile.GetComponent<MapHex>().getTilesWithin(moveDist);
		/*List<GameObject> test = tile.GetComponent<MapHex>().getTilesWithin(moveDist);
		foreach(GameObject hex in test){
			hex.GetComponent<SpriteRenderer>().color = Color.clear;
		}

		GameObject nearestMountain = tile.GetComponent<MapHex>().findNearestType(terrainType.Mountain);
		nearestMountain.GetComponent<SpriteRenderer>().color = Color.red;*/
	}

	public void Update(){
		if(MouseTracker.selected == gameObject){
			tilesInMoveRange = tile.GetComponent<MapHex>().getTilesWithin(moveDist);
			foreach(GameObject hex in tilesInMoveRange){
				hex.GetComponent<SpriteRenderer>().color = new Color(0.75f,0f,0f,0.75f);
			}
		}
		else if(MouseTracker.selected == null){
			foreach(GameObject hex in tilesInMoveRange){
				hex.GetComponent<SpriteRenderer>().color = Color.white;
			}
			tilesInMoveRange.Clear();
		}

		if(tilesInMoveRange.Contains (MouseTracker.selected)){
			gameObject.transform.position = MouseTracker.selected.transform.position;
			tile = MouseTracker.selected;
			foreach(GameObject hex in tilesInMoveRange){
				hex.GetComponent<SpriteRenderer>().color = Color.white;
			}
			tilesInMoveRange.Clear();
		}
	}

	public void move(){
		//Need some way of A*ing to find all moveable tiles, considering travel difficulty.
		//Make these highlighted.
		//Show an arrow of the path to be taken.
		//If clicked, slide unit along path.
		//reduce movesLeft count.
		//update tile.
	}


	public delegate void attack();

	public void meleeAttack(){}

	public void rangedAttack(){}

	public void loadMoveCost(){
		moveCost = new Dictionary<terrainType, int>();
		moveCost.Add (terrainType.City, _moveCost[0]);
		moveCost.Add (terrainType.Desert, _moveCost[1]);
		moveCost.Add (terrainType.Forest,_moveCost[2]);
		moveCost.Add (terrainType.Hill,_moveCost[3]);
		moveCost.Add (terrainType.Jungle,_moveCost[4]);
		moveCost.Add (terrainType.Mountain,_moveCost[5]);
		moveCost.Add (terrainType.Ocean,_moveCost[6]);
		moveCost.Add (terrainType.Path,_moveCost[7]);
		moveCost.Add (terrainType.Plain,_moveCost[8]);
		moveCost.Add (terrainType.Reef,_moveCost[9]);
		moveCost.Add (terrainType.River,_moveCost[10]);
		moveCost.Add (terrainType.Shore,_moveCost[11]);
		moveCost.Add (terrainType.Tundra,_moveCost[12]);
	}

	/*
	 * Each hull hs a few base stats, but all derive from this. It needs to have all the requisite behaviours for any given unit:
	 * Move
	 * Attack
	 * Die
	 * Build Cost
	 */

}

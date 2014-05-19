using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarTestScript : MonoBehaviour {

	public Dictionary<terrainType,int> moveCost;
	public int[] _moveCost;
	public GameObject from, to;

	void Start(){
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

	void Update () {

		if(Input.GetKeyDown(KeyCode.R)){
			Application.LoadLevel (0);
		}

		if(from != null){
			to = MouseTracker.selected;
		}
		
		if(to != null && to == MouseTracker.selected){
			to.GetComponent<SpriteRenderer>().color = Color.magenta;
		}
		if(from == null) from = MouseTracker.selected;

		if(from != null && from == MouseTracker.selected){
			from.GetComponent<SpriteRenderer>().color = Color.cyan;
		}

		if(Input.GetKeyDown(KeyCode.Space)){
			List<GameObject> winPath = AStar.findPath (from,to,moveCost);
			if(winPath != null){
				foreach(GameObject hex in winPath){
					hex.GetComponent<SpriteRenderer>().color = Color.green;
				}
			}
			//StartCoroutine(AStar.findPath (from,to,moveCost));
		}
	}
}

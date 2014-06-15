using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AStar : MonoBehaviour {
	

	public static List<GameObject> aStar(GameObject nodeFrom, GameObject nodeTo, Dictionary<terrainType,int> moveDict){
	//	StartCoroutine(findPath (nodeFrom,nodeTo,moveDict));
		return null;
	}

	public static List<GameObject> findPath(GameObject nodeFrom, GameObject nodeTo, Dictionary<terrainType,int> moveDict){
		List<GameObject> openList = new List<GameObject>(); 
		List<GameObject> closedList = new List<GameObject>();
		Dictionary<GameObject,GameObject> parentDict = new Dictionary<GameObject, GameObject>();
		Dictionary<GameObject, int> fScore = new Dictionary<GameObject, int>();
		Dictionary <GameObject, int> gScore = new Dictionary<GameObject, int>();
		GameObject currentNode = nodeFrom;

		gScore.Add (currentNode,0);
		fScore.Add (currentNode,gScore[currentNode]+getHeuristic(nodeFrom,nodeTo));
		openList.Add(currentNode);
		while(openList.Count >0){
			currentNode = getLowest(openList,fScore);
			if(currentNode == nodeTo){
				return (bakePath (nodeFrom, nodeTo, parentDict));
			}
			openList.Remove (currentNode);
			closedList.Add (currentNode);
			foreach(GameObject hex in currentNode.GetComponent<MapHex>().neighborList){
				int tempG = (gScore[currentNode]+moveDict[hex.GetComponent<MapHex>().getTerrain()]);
				if(!openList.Contains (hex) && !closedList.Contains(hex)){
					openList.Add (hex);
					parentDict.Add (hex, currentNode);
					gScore.Add (hex,tempG);
					fScore.Add (hex,gScore[hex]+getHeuristic(hex,nodeTo));
				}
				if(tempG<gScore[hex]){
					if(parentDict.ContainsKey(hex))parentDict.Remove (hex);
					parentDict.Add (hex, currentNode);
					if(gScore.ContainsKey(hex))gScore.Remove (hex);
					gScore.Add (hex,tempG);
					if(fScore.ContainsKey(hex))fScore.Remove (hex);
					fScore.Add (hex,gScore[hex]+getHeuristic(hex,nodeTo));
				}
				if(moveDict[hex.GetComponent<MapHex>().getTerrain()] >900){
					openList.Remove(hex);
					closedList.Add (hex);
				}
			}
		}
		Debug.Log ("Path unreachable.");
		return null;
	}

	private static int getHeuristic(GameObject nodeFrom, GameObject nodeTo){
		return(nodeFrom.GetComponent<MapHex>().getTilesFrom (nodeTo));
	}

	private static GameObject getLowest(List<GameObject> openList, Dictionary<GameObject,int> input){
		GameObject result = openList[0];
		for(int i=0;i<openList.Count;i++){
			if(input[result]>=input[openList[i]]){
				result = openList[i];
			}
		}
		return result;
	}

	private static List<GameObject> bakePath(GameObject start, GameObject goal, Dictionary<GameObject, GameObject>  parentDict){
		List<GameObject> result = new List<GameObject>();
		GameObject currentNode;

		result.Add (goal);
		currentNode = goal;
		while(!currentNode.Equals(start)){
			result.Add (parentDict[currentNode]);
			currentNode = parentDict[currentNode];
		}
		return result;
	}

	private static string printList(List<GameObject> input){
		string result = "";
		for(int i=0;i<input.Count;i++){
			result += (input[i] + ", ");
		}
		return result;
	}

	public static List<GameObject> findMovableTiles(GameObject tile, int maxDist, Dictionary<terrainType,int> moveDict){
		List<GameObject> openList = new List<GameObject>();
		List<GameObject> closedList = new List<GameObject>();
		List<GameObject> result = new List<GameObject>();
		Dictionary <GameObject, int> gScore = new Dictionary<GameObject, int>();
		GameObject currentNode;

		openList.Add(tile);
		result.Add(tile);
		gScore.Add (tile,0);
		while(openList.Count > 0){
			currentNode = openList[0];
			foreach(GameObject hex in currentNode.GetComponent<MapHex>().neighborList){
				int tempG = (gScore[currentNode]+moveDict[hex.GetComponent<MapHex>().getTerrain()]);
				if(!openList.Contains(hex)&&!closedList.Contains(hex)&&tempG<=maxDist){
					gScore.Add (hex,tempG);
					openList.Add (hex);
				}
				else if(tempG<gScore[hex]&&gScore.ContainsKey(hex)){
					gScore.Remove (hex);
					gScore.Add (hex,tempG);
				}
			}
			closedList.Add(currentNode);
			openList.Remove(currentNode);
			if(gScore[currentNode]<=maxDist)result.Add(currentNode);
		}
		return result;
	}

}

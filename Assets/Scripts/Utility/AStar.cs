using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AStar : MonoBehaviour {
	

	public static List<GameObject> aStar(GameObject nodeFrom, GameObject nodeTo, Dictionary<terrainType,int> moveDict){
	//	StartCoroutine(findPath (nodeFrom,nodeTo,moveDict));
		return null;
	}

	public static IEnumerator findPath(GameObject nodeFrom, GameObject nodeTo, Dictionary<terrainType,int> moveDict){
		List<GameObject> openList = new List<GameObject>(); 
		List<GameObject> closedList = new List<GameObject>();
		Dictionary<GameObject,GameObject> parentDict = new Dictionary<GameObject, GameObject>();
		Dictionary<GameObject, int> fScore = new Dictionary<GameObject, int>();
		Dictionary <GameObject, int> gScore = new Dictionary<GameObject, int>();
		GameObject currentNode = nodeFrom;

		gScore.Add (currentNode,0);
		fScore.Add (currentNode,gScore[currentNode]+getHeuristic(nodeFrom,nodeTo));
		openList.Add(currentNode);
		int i=0;
		while(openList.Count >0){
		//for(int i=0;i<30;i++){
			Debug.Log ("Attempt #"+(i+1));
			currentNode = getLowest(openList,fScore);
			Debug.Log ("Current hex is "+currentNode+" with an fScore of "+fScore[currentNode]);
			if(currentNode == nodeTo){
				Debug.Log ("VICTORY.");
				break;
				yield return null;
			}
			openList.Remove (currentNode);
			if(currentNode != nodeTo && currentNode!= nodeFrom)currentNode.GetComponent<SpriteRenderer>().color = new Color(0.75f,0f,0f,0.75f);
			else currentNode.GetComponent<SpriteRenderer>().color = Color.blue;
			closedList.Add (currentNode);
			foreach(GameObject hex in currentNode.GetComponent<MapHex>().neighborList){
				int tempG = (gScore[currentNode]+moveDict[hex.GetComponent<MapHex>().getTerrain()]);
				if(!openList.Contains (hex)||tempG>gScore[hex]){
					if(parentDict.ContainsKey(hex))parentDict.Remove (hex);
					parentDict.Add (hex,currentNode);
					if(gScore.ContainsKey(hex))gScore.Remove (hex);
					gScore.Add (hex,tempG);
					if(fScore.ContainsKey(hex))fScore.Remove (hex);
					fScore.Add (hex,gScore[hex]+getHeuristic(hex,nodeTo));
					if(!openList.Contains (hex)&&!closedList.Contains (hex))openList.Add (hex);
					Debug.Log ("We just looked at "+hex+" and it had an fScore of "+fScore[hex]+" and a gScore of "+gScore[hex]+".");
					Debug.Log ("It's heuristic was calculated as "+getHeuristic(hex,nodeTo));
				}
				yield return new WaitForSeconds(0.001f);
			}
			Debug.Log ("openList count: "+openList.Count);
			Debug.Log ("closedList count:"+closedList.Count);
			Debug.Log ("openList: "+printList (openList));
			Debug.Log ("closedList: "+printList (closedList));
			i++;
			yield return new WaitForSeconds(0.02f);
		}
		Debug.Log ("Haha, A* fail, you suck.");
		yield return null;
	}

	private static int getHeuristic(GameObject nodeFrom, GameObject nodeTo){
		return(nodeFrom.GetComponent<MapHex>().getTilesFrom (nodeTo));
	}

	private static GameObject getLowest(List<GameObject> openList, Dictionary<GameObject,int> input){
		GameObject result = openList[0];
		for(int i=openList.Count;i<0;i--){
			if(input[result]>input[openList[i]]){
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
		Debug.Log("As a reminder, start is: "+start);
		Debug.Log (printList (result));
		return result;
	}

	private static string printList(List<GameObject> input){
		string result = "";
		for(int i=0;i<input.Count;i++){
			result += (input[i] + ", ");
		}
		return result;
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapHex : MonoBehaviour {

	public Sprite[] tileArt;

	public List<GameObject> neighborList;
	private terrainType terrain;
	private GameObject UL,UU,UR,DL,DD,DR;
	public bool isSeed = false;
	private int xCord, yCord;
	public float temperature;
	public int contIndex, occIndex;


	private void Start(){

		chooseSprite();

		//Visualize temp/rainfall.
		//gameObject.GetComponent<SpriteRenderer>().color = new Color(rainfall,1f,1f);
		//gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f-temperature,1f-temperature);
	}

	public GameObject findNearestType(terrainType target){
		List<GameObject> openList = new List<GameObject>();
		List<GameObject> tempAdd = new List<GameObject>();
		List<GameObject> tempRem = new List<GameObject>();
		List<GameObject> closedList = new List<GameObject>();

		foreach(GameObject hex in neighborList){
			if(hex.GetComponent<MapHex>().terrain == target){
				return hex;
			}
			else {
				foreach(GameObject subHex in hex.GetComponent<MapHex>().neighborList){
					if(!openList.Contains (subHex)){
						openList.Add(subHex);
					}
				}
			}
		}

		while(openList.Count > 0){
			foreach(GameObject hex in openList){
				if(hex.GetComponent<MapHex>().terrain == target){
					return hex;
				}
				else{
					tempRem.Add(hex);
					closedList.Add(hex);
					foreach(GameObject subHex in hex.GetComponent<MapHex>().neighborList){
						if(!openList.Contains (subHex) && !closedList.Contains (subHex)){
							tempAdd.Add(subHex);
						}
					}
				}
			}
			foreach(GameObject hex in tempAdd){
				openList.Add (hex);
			}
			foreach(GameObject hex in tempRem){
				openList.Remove (hex);
			}
			tempAdd.Clear ();
			tempRem.Clear ();
		}
		return null;
	}


	public List<GameObject> getTilesWithin(int range){
		List<GameObject> result = new List<GameObject>();
		List<GameObject> temp = new List<GameObject>();
		if(range == 0){
			return result;
		}

		foreach(GameObject hex in neighborList){
			result.Add(hex);
			temp.Add(hex);
		}

		for(int i=1;i<range;i++){
			foreach(GameObject hex in result){
				foreach(GameObject subHex in hex.GetComponent<MapHex>().neighborList){
					if(!temp.Contains(subHex)){
						temp.Add(subHex);
					}
				}
			}
			foreach(GameObject subHex in temp){
				if(!result.Contains (subHex)){
					result.Add (subHex);
				}
			}
			temp.Clear();
		}
		result.Remove (gameObject);
		return result;
	}

	public int getTilesFrom(GameObject other){
		int result = 1;
		if(gameObject == other) return 0;
		if(neighborList.Contains (other)) return result;

		else{
			List<GameObject> openList = new List<GameObject>();
			List<GameObject> temp = new List<GameObject>();
			foreach(GameObject hex in neighborList){
				openList.Add(hex);
				temp.Add(hex);
			}
			while(!openList.Contains(other)){
				foreach(GameObject hex in openList){
					foreach(GameObject subHex in hex.GetComponent<MapHex>().neighborList){
						if(!temp.Contains (subHex)){
							temp.Add (subHex);
						}
					}
				}
				foreach(GameObject subHex in temp){
					if(!openList.Contains(subHex)){
						openList.Add (subHex);
					}
				}
				result++;
			}
			return result;
		}
	}

	public void chooseSprite(){
		SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
		switch (terrain){
		case terrainType.City:
			sprite.sprite = tileArt[0];
			break;
		case terrainType.Desert:
			sprite.sprite = tileArt[1];
			break;
		case terrainType.Forest:
			sprite.sprite = tileArt[2];
			break;
		case terrainType.Hill:
			sprite.sprite = tileArt[3];
			break;
		case terrainType.Jungle:
			sprite.sprite = tileArt[4];
			break;
		case terrainType.Mountain:
			sprite.sprite = tileArt[5];
			break;
		case terrainType.Ocean:
			sprite.sprite = tileArt[6];
			break;
		case terrainType.Path:
			sprite.sprite = tileArt[7];
			break;
		case terrainType.Plain:
			sprite.sprite = tileArt[8];
			break;
		case terrainType.Reef:
			sprite.sprite = tileArt[9];
			break;
		case terrainType.River:
			sprite.sprite = tileArt[10];
			break;
		case terrainType.Tundra:
			sprite.sprite = tileArt[11];
			break;
		case terrainType.Shore:
			sprite.sprite = tileArt[12];
			break;
		}
	}

	public void initializeHex(terrainType input, int x, int y){
		xCord = x;
		yCord = y;
		terrain = input;
		temperature = 1-(Mathf.Abs (yCord-WorldMap.height/2f))/WorldMap.height*2;
		temperature += Random.Range (-0.05f,0.05f);
	}

	public void setTerrain(terrainType input){
		terrain = input;
	}

	public terrainType getTerrain(){
		return terrain;
	}
	
	public void setSeed(){
		isSeed = true;
	}

	public bool freezeEdges(){
		if (temperature <= 0.16f){
			setTerrain(terrainType.Tundra);
			return true;
		}
		else return false;
	}

	public void determineFlora(float forestChance){
		bool nearDesert = false;
		float rand;
		for(int i=0;i<neighborList.Count;i++){
			if(neighborList[i].GetComponent<MapHex>().terrain == terrainType.Desert){
				nearDesert = true;
			}
		}
		rand = Random.value;
		if(rand <= forestChance && !nearDesert){
			setTerrain (terrainType.Forest);
			if(temperature >= 0.85f) setTerrain(terrainType.Jungle);
		}

	}

	public void determineMountain(){
		float mountChance = 0f;
		float rand;
		for(int i=0;i<neighborList.Count;i++){
			if(neighborList[i].GetComponent<MapHex>().contIndex != this.contIndex){
				mountChance += 0.1f;
			}
		}
		rand = Random.value;
		if(rand <= mountChance){
			setTerrain(terrainType.Mountain);
		}
		else if(rand/2f <= mountChance){
			setTerrain(terrainType.Hill);
		}
	}

	public void mapNeighbours(){
		neighborList = new List<GameObject>();

		if(yCord+1 < WorldMap.height){
			UU = WorldMap.mapTiles[xCord,yCord+1];
			neighborList.Add (UU);
		}
		else UU = null;

		if(yCord-1 >= 0){
			DD = WorldMap.mapTiles[xCord,yCord-1];
			neighborList.Add(DD);
		}
		else DD = null;

		//Because hexes aren't squares, the space to the left/right is tricky, and changes every other row.
		if(xCord%2 == 0){
			if(xCord-1 >= 0 && yCord+1 < WorldMap.height){
				UL = WorldMap.mapTiles[xCord-1,yCord+1];
				neighborList.Add (UL);
			}
			else UL = null;
			
			if(xCord-1 >= 0){
				DL = WorldMap.mapTiles[xCord-1,yCord];
				neighborList.Add (DL);
			}
			else DL = null;

			if(xCord+1 < WorldMap.width && yCord+1 < WorldMap.height){
				UR = WorldMap.mapTiles[xCord+1,yCord+1];
				neighborList.Add (UR);
			}
			else UR = null;

			if(xCord+1 < WorldMap.width){
				DR = WorldMap.mapTiles[xCord+1,yCord];
				neighborList.Add(DR);
			}
			else DR = null;
		}

		else{
			if(xCord-1 >= 0){
				UL = WorldMap.mapTiles[xCord-1,yCord];
				neighborList.Add (UL);
			}
			else UL = null;
			
			if(xCord-1 >= 0 && yCord-1 >= 0){
				DL = WorldMap.mapTiles[xCord-1,yCord-1];
				neighborList.Add (DL);
			}
			else DL = null;
			
			if(xCord+1 < WorldMap.width){
				UR = WorldMap.mapTiles[xCord+1,yCord];
				neighborList.Add (UR);
			}
			else UR = null;
			
			if(xCord+1 < WorldMap.width && yCord-1 >= 0){
				DR = WorldMap.mapTiles[xCord+1,yCord-1];
				neighborList.Add(DR);
			}
			else DR = null;
		}
	}

}

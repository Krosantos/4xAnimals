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
	public float temperature, rainfall;
	public int contIndex;


	private void Start(){
		chooseSprite();
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
		rainfall = 0.5f;
		temperature = (Mathf.Abs (yCord-WorldMap.height/2f))/WorldMap.height*2;
		temperature += Random.Range (-0.05f,0.05f);
	}

	public void setTerrain(terrainType input){
		terrain = input;
	}
	
	public void setSeed(){
		isSeed = true;
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

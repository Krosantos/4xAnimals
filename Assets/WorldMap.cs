using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMap : MonoBehaviour {

	public static int width, height, continents, contMaxSize;
	static public GameObject[,] mapTiles;
	static public MapHex[,] mapScripts;
	static public List<GameObject> seedList;
	public GameObject hex;

	private void Awake(){
		width = 56;
		height = 26;
		continents = 2;
		contMaxSize = 40;
		spawnHex();
		chooseSeeds();
	}


	private void spawnHex(){
		GameObject temp;
		mapTiles = new GameObject[width,height];
		mapScripts = new MapHex[width,height];
		for(int x=0;x<width;x++){
			for(int y=0;y<height;y++){
				if(x%2 == 0){
					temp = (GameObject)Instantiate(hex,new Vector3(x*2.88f,y*3.34f,0f),Quaternion.identity);
					temp.transform.parent = transform;
					temp.name = (x+", "+y);
					mapTiles[x,y] = temp;
					mapScripts[x,y] = temp.GetComponent<MapHex>();
					temp.GetComponent<MapHex>().initializeHex (terrainType.Ocean,x,y);
				}
				else{
					temp = (GameObject)Instantiate(hex,new Vector3(x*2.88f,(y*3.34f)-1.67f,0f),Quaternion.identity);
					temp.transform.parent = transform;
					temp.name = (x+", "+y);
					mapTiles[x,y] = temp;
					mapScripts[x,y] = temp.GetComponent<MapHex>();
					temp.GetComponent<MapHex>().initializeHex (terrainType.Ocean,x,y);
				}
			}
		}
		foreach(MapHex hex in mapScripts){
			hex.mapNeighbours();
		}
	}

	private void chooseSeeds(){
		seedList = new List<GameObject>();
		int randX, randY;
		for(int i=0;i<continents;i++){
			randX = Random.Range ((width/continents)*i,(width/continents)*(i+1));
			randY = Random.Range ((height/10),height-(height/10));
			mapScripts[randX,randY].setSeed();
			seedList.Add (mapTiles[randX,randY]);
			mapScripts[randX,randY].setTerrain(terrainType.Plain);
		}
	}

	private void growContinents(){
		foreach(GameObject seed in seedList){

		}
	}

}

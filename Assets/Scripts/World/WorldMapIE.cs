using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMapIE : MonoBehaviour {

	public static int width, height, continents, contMaxSize, islands, islMaxSize, riverCount;
	static public GameObject[,] mapTiles;
	static public MapHex[,] mapScripts;
	static public List<MapHex> seedList, landTiles, seaTiles;
	public GameObject hex;
	public float landChance, forestChance;


	private void Awake(){
		seedList = new List<MapHex>();
		landTiles = new List<MapHex>();
		seaTiles = new List<MapHex>();
		width = 60;
		height = 30;
		continents = 4;
		contMaxSize = 60;
		islands = 10;
		islMaxSize = 20;
		riverCount = 4;

		StartCoroutine ("buildWorld");

		//spawnHex();
		//chooseSeeds();
		//growContinents();
		//sprinkleIslands();
		//checkMountains();
		//checkTundra();
		//addLandFeatures (landTiles, terrainType.Desert,3,15,0.1f);
		//checkFlora();
		//addSeaFeatures (seaTiles, terrainType.Reef, 10,5, 0.07f);
		//Make rivers

	}

	private IEnumerator buildWorld(){
		yield return spawnHex ();
		yield return new WaitForSeconds(7f);
		chooseSeeds ();
		yield return growContinents ();
		yield return sprinkleIslands ();
		checkMountains();
		updateTiles();
		checkTundra();
		updateTiles ();
		yield return addLandFeatures (landTiles, terrainType.Desert,3,15,0.1f);
		checkFlora ();
		updateTiles ();
		yield return addSeaFeatures (seaTiles, terrainType.Reef, 10,5, 0.07f);
	}

	private IEnumerator spawnHex(){
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
					seaTiles.Add (temp.GetComponent<MapHex>());
					temp.GetComponent<MapHex>().initializeHex (terrainType.Ocean,x,y);
				}
				else{
					temp = (GameObject)Instantiate(hex,new Vector3(x*2.88f,(y*3.34f)-1.67f,0f),Quaternion.identity);
					temp.transform.parent = transform;
					temp.name = (x+", "+y);
					mapTiles[x,y] = temp;
					mapScripts[x,y] = temp.GetComponent<MapHex>();
					seaTiles.Add (temp.GetComponent<MapHex>());
					temp.GetComponent<MapHex>().initializeHex (terrainType.Ocean,x,y);
				}
				yield return new WaitForSeconds(0.01f);
			}
		}
		foreach(MapHex hex in mapScripts){
			hex.mapNeighbours();
		}
	}

	public void updateTiles(){
		for(int x=0;x<mapScripts.GetLength(0);x++){
			for(int y=0;y<mapScripts.GetLength(1);y++){
				mapScripts[x,y].chooseSprite();
			}
		}
	}

	private void chooseSeeds(){
		int randX, randY;
		for(int i=0;i<continents;i++){
			randX = Random.Range ((width/continents)*i,(width/continents)*(i+1));
			randY = Random.Range ((height/10),height-(height/10));
			mapScripts[randX,randY].setSeed();
			mapScripts[randX,randY].contIndex = i+1;
			seedList.Add (mapScripts[randX,randY]);
			landTiles.Add (mapScripts[randX,randY]);
			seaTiles.Remove(mapScripts[randX,randY]);
			mapScripts[randX,randY].setTerrain(terrainType.Plain);
		}
	}

	private IEnumerator growContinents(){
		List<MapHex> openList = new List<MapHex>();
		List<MapHex> closedList = new List<MapHex>();

		//Add the intial seed's neighbours.
		for(int i=0;i<continents;i++){
			addNeighbours (seedList[i],openList, closedList);
		}

		for(int i=0;i<contMaxSize;i++){ //Max passes

			//Go through all potential land tiles
			for(int j=0;j<openList.Count;j++){ //Every tile in openlist

				if(Random.value <= landChance){
					openList[j].setTerrain(terrainType.Plain);
					closedList.Add(openList[j]);
					landTiles.Add (openList[j]);
					seaTiles.Remove(openList[j]);
					addNeighbours(openList[j],openList,closedList);

				}
			}
			updateTiles();
			yield return new WaitForSeconds (0.01f);
		}
		
	}

	private IEnumerator sprinkleIslands(){
		List<MapHex> openList = new List<MapHex>();
		List<MapHex> closedList = new List<MapHex>();
		int rand;

		//Choose a few seeds from seaTiles.
		for(int i=0;i<islands;i++){
			rand = Random.Range(0,seaTiles.Count);
			seaTiles[rand].setTerrain (terrainType.Plain);
			landTiles.Add (seaTiles[rand]);
			addNeighbours(seaTiles[rand],openList,closedList);
			seaTiles.Remove(seaTiles[rand]);
		}

		//Grow 'em like continents, but tinier.
		for(int i=0;i<islMaxSize;i++){
			for(int j=0;j<openList.Count;j++){
				
				if(Random.value <= landChance){
					openList[j].setTerrain(terrainType.Plain);
					closedList.Add(openList[j]);
					landTiles.Add (openList[j]);
					seaTiles.Remove(openList[j]);
					addNeighbours(openList[j],openList,closedList);
					
				}
			}
			updateTiles ();
			yield return new WaitForSeconds(0.01f);
		}

	}

	public void checkMountains(){
		for(int i=0;i<landTiles.Count;i++){
			landTiles[i].determineMountain();
		}
	}

	private void checkTundra(){
		List<MapHex> temp = new List<MapHex>();
		for(int i=0;i<landTiles.Count;i++){
			if(landTiles[i].freezeEdges()){
				temp.Add(landTiles[i]);
			}
		}
		for(int i=0;i<temp.Count;i++){
			landTiles.Remove(temp[i]);
		}
		landTiles.TrimExcess();
	}

	private void checkFlora(){
		for(int i=0;i<landTiles.Count;i++){
			landTiles[i].determineFlora(forestChance);
		}
	}

	private void addNeighbours(MapHex input, List<MapHex> targetList, List<MapHex> blackList){
		foreach(GameObject Hex in input.neighborList){
			if(!blackList.Contains(Hex.GetComponent<MapHex>())){
				targetList.Add (Hex.GetComponent<MapHex>());
				Hex.GetComponent<MapHex>().contIndex = input.contIndex;
			}
		}
	}

	private void addNeighbours(MapHex input, List<MapHex> targetList, List<MapHex> blackList, List<MapHex> proofList){
		foreach(GameObject Hex in input.neighborList){
			if(!blackList.Contains(Hex.GetComponent<MapHex>()) && proofList.Contains(Hex.GetComponent<MapHex>())){
				targetList.Add (Hex.GetComponent<MapHex>());
				Hex.GetComponent<MapHex>().contIndex = input.contIndex;
			}
		}
	}

	private IEnumerator addLandFeatures(List<MapHex> sourceList, terrainType terrain, int seedCount, int passes, float convChance){
		List<MapHex> openList = new List<MapHex>();
		List<MapHex> closedList = new List<MapHex>();
		int rand;

		sourceList.TrimExcess();
		//Choose a few seeds.
		for(int i=0;i<seedCount;i++){
			rand = Random.Range(0,sourceList.Count);
			sourceList[rand].setTerrain (terrain);
			addNeighbours(sourceList[rand],openList,closedList,landTiles);
			sourceList.Remove(sourceList[rand]);
			updateTiles ();
			yield return new WaitForSeconds(0.01f);
		}
		
		//Grow 'em like continents.
		for(int i=0;i<passes;i++){
			for(int j=0;j<openList.Count;j++){
				
				if(Random.value <= convChance){
					openList[j].setTerrain(terrain);
					addNeighbours(openList[j],openList,closedList,landTiles);
					
				}
				closedList.Add(openList[j]);
				sourceList.Remove(openList[j]);
			}
			updateTiles ();
			yield return new WaitForSeconds(0.01f);
		}
	}

	private IEnumerator addSeaFeatures(List<MapHex> sourceList, terrainType terrain, int seedCount, int passes, float convChance){
		List<MapHex> openList = new List<MapHex>();
		List<MapHex> closedList = new List<MapHex>();
		int rand;
		
		//Choose a few seeds.
		for(int i=0;i<seedCount;i++){
			rand = Random.Range(0,sourceList.Count);
			sourceList[rand].setTerrain (terrain);
			addNeighbours(sourceList[rand],openList,closedList,seaTiles);
			sourceList.Remove(sourceList[rand]);
			updateTiles ();
			yield return new WaitForSeconds(0.01f);
		}
		
		//Grow 'em like continents.
		for(int i=0;i<passes;i++){
			for(int j=0;j<openList.Count;j++){
				
				if(Random.value <= convChance){
					openList[j].setTerrain(terrain);
					addNeighbours(openList[j],openList,closedList,seaTiles);
				}
				closedList.Add(openList[j]);
				sourceList.Remove(openList[j]);
			}
			updateTiles ();
			yield return new WaitForSeconds(0.01f);
		}
	}
}

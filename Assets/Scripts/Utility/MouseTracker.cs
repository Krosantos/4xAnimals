using UnityEngine;
using System.Collections;

public class MouseTracker : MonoBehaviour {

	public static GameObject selected;

	public void Update(){
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit = new RaycastHit();
		if(Input.GetMouseButtonDown (0)){
			if(Physics.Raycast(mouseRay, out rayHit, 1000f)){
				if(selected == rayHit.transform.gameObject){
					selected = null;
				}
				else{
					selected = rayHit.transform.gameObject;
					Debug.Log ("Selected object is "+selected);
				}
			}
			else{
				Debug.Log ("CLICK MISFIRE!!");
				selected = null;
			}
		}
	}

	public static bool isHover(GameObject self){
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit = new RaycastHit();
		if(Physics.Raycast(mouseRay, out rayHit, 100f)){
			GameObject beam = rayHit.transform.gameObject;
			if(beam == self) return true;
			else return false;
		}
		return false;
	}

	public static bool isClick(GameObject self){
		if(isHover(self)&&Input.GetMouseButtonDown(0)){
			return true;
		}
		else return false;
	}


}

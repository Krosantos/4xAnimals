using UnityEngine;
using System.Collections;

public class MouseClicker : MonoBehaviour {
	
	void Update () {
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit = new RaycastHit();
		if(Input.GetMouseButtonDown (0)){
			if(Physics.Raycast(mouseRay, out rayHit, 1000f)){
				Clickable click = (Clickable)rayHit.transform.gameObject.GetComponent (typeof(Clickable));
				if(click != null){
					click.onClick();
				}
			}
		}
	}
}

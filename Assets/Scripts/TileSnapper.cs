using UnityEngine;
using System.Collections;

public class TileSnapper : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDrag() {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		if(!collider2D.OverlapPoint(new Vector2(mousePos.x, mousePos.y)))
		{
			Vector3 bounds = collider2D.bounds.size;

			float x = (mousePos.x % bounds.x);
			float y = (mousePos.y % bounds.y);
			
			if(x > bounds.x/2)
				mousePos.x += bounds.x - x;
			else
				mousePos.x -= x;
			
			if(y > bounds.y/2)
				mousePos.y += bounds.y - y;
			else
				mousePos.y -= y;

			bool hit = false;
			Vector3 newPos = new Vector3(mousePos.x, mousePos.y, transform.position.z);

			foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Conveyor"))
			{
				if(obj.transform.position == newPos)
					hit = true;
			}

			if(!hit)
				transform.position = newPos;
		}
	}
}

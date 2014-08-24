using UnityEngine;
using System.Collections;

public class Conveyor : MonoBehaviour {
	
	public DIRECTIONS 	mDirection;
	private Vector3		mLastLoc;

	// Use this for initialization
	void Start () {
		mDirection = DIRECTIONS.NORTH;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		mLastLoc = transform.position;
	}

	void OnMouseUp() {
		if(mLastLoc == transform.position)
		{
			if(mDirection == DIRECTIONS.WEST)
				mDirection = DIRECTIONS.NORTH;
			else
				mDirection++;

			transform.rotation *= Quaternion.AngleAxis(-90, Vector3.forward);
		}
	}

	public DIRECTIONS getDirection() {
		return mDirection;
	}
}

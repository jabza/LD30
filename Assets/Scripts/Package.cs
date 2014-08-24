using UnityEngine;
using System.Collections;

public enum DIRECTIONS
{
	NORTH = 0,
	EAST,
	SOUTH,
	WEST,
	TOTAL
};

public enum DESTINATION
{
	A = 0,
	B,
	C,
	D,
	TOTAL
};

public class Package : MonoBehaviour {

	public Sprite spriteA, spriteB, spriteC, spriteD;

	private float		mSpeed = 1.0f;
	private DIRECTIONS 	mDirection = DIRECTIONS.SOUTH;
	private DESTINATION mDestination = DESTINATION.A;
	private int			mColliderCount = 0;

	// Use this for initialization
	void Start () {
		updateSprite();
	}

	// Update is called once per frame
	void Update () {
		Vector3 moveVector = new Vector3(0, 0, 0);
		
		//Move based on direction.
		switch(mDirection)
		{
		case DIRECTIONS.NORTH 	: moveVector.y = mSpeed; 	break;
		case DIRECTIONS.EAST 	: moveVector.x = mSpeed;	break;
		case DIRECTIONS.SOUTH 	: moveVector.y = -mSpeed;	break;
		case DIRECTIONS.WEST 	: moveVector.x = -mSpeed;	break;
		}

		transform.position += (moveVector*Time.deltaTime);


	}

	public void setDirection(DIRECTIONS direction) {
		mDirection = direction;
	}

	public void setDestination(DESTINATION destination) {
		mDestination = destination;
		updateSprite();
	}

	void updateSprite() {
		SpriteRenderer childSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
		switch(mDestination)
		{
		case DESTINATION.A : childSprite.sprite = spriteA; break;
		case DESTINATION.B : childSprite.sprite = spriteB; break;
		case DESTINATION.C : childSprite.sprite = spriteC; break;
		case DESTINATION.D : childSprite.sprite = spriteD; break;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {

		if(other.tag.Contains("Conveyor"))
			mColliderCount++;
	}

	void OnTriggerStay2D(Collider2D other) {

		if(other.tag.Contains("Conveyor"))
		{
			Conveyor con = other.GetComponent<Conveyor>();
			if(mDirection == DIRECTIONS.NORTH)
			{
				if(transform.position.y >= other.transform.position.y && transform.position.x > other.transform.position.x-0.5 && transform.position.x < other.transform.position.x+0.5)
					mDirection = con.getDirection();
			}
			else if(mDirection == DIRECTIONS.EAST)
			{
				if(transform.position.x >= other.transform.position.x && transform.position.y > other.transform.position.y-0.5 && transform.position.y < other.transform.position.y+0.5)
					mDirection = con.getDirection();
			}
			else if(mDirection == DIRECTIONS.SOUTH)
			{
				if(transform.position.y <= other.transform.position.y && transform.position.x > other.transform.position.x-0.5 && transform.position.x < other.transform.position.x+0.5)
					mDirection = con.getDirection();
			}
			else if(mDirection == DIRECTIONS.WEST)
			{
				if(transform.position.x <= other.transform.position.x && transform.position.y > other.transform.position.y-0.5 && transform.position.y < other.transform.position.y+0.5)
					mDirection = con.getDirection();
			}

		}
	}

	void OnTriggerExit2D(Collider2D other) {

		if(other.tag.Contains("Conveyor"))
			mColliderCount--;

		if(mColliderCount <= 0)
			Destroy(gameObject);
	}

}

using UnityEngine;
using System.Collections;

public class ExitShaft : MonoBehaviour {

	public Sprite spriteA, spriteB, spriteC, spriteD;
	public DIRECTIONS 	entryDirection;
	public DESTINATION 	destination = DESTINATION.A;
	public ShipmentHandler shipmentHandler;

	// Use this for initialization
	void Start () {
		transform.rotation *= Quaternion.AngleAxis((-90)*(int)entryDirection, Vector3.forward);
		updateSprite();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void updateSprite() {
		SpriteRenderer childSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
		switch(destination)
		{
		case DESTINATION.A : childSprite.sprite = spriteA; break;
		case DESTINATION.B : childSprite.sprite = spriteB; break;
		case DESTINATION.C : childSprite.sprite = spriteC; break;
		case DESTINATION.D : childSprite.sprite = spriteD; break;
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		
		if(other.tag == "Package")
		{
			PackageMover pkg = other.GetComponent<PackageMover>();

			if(pkg && shipmentHandler != null)
			{
				if(pkg.getDestination() == destination)
					shipmentHandler.gotGoodPackage();
				else
					shipmentHandler.gotBadPackage();
			}

			Destroy(other.gameObject);
		}
		else if(other.tag == "DestroyedPackage")
		{
			shipmentHandler.gotBadPackage();
			Destroy(other.gameObject);
		}
	}

}

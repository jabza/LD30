using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shipment
{
	public float 	timeTillArrival;
	public int[] 	packageCounts;

	public Shipment(float tta, int a, int b, int c, int d)
	{
		timeTillArrival = tta;

		packageCounts = new int[(int)DESTINATION.TOTAL];
		packageCounts[(int)DESTINATION.A] = a;
		packageCounts[(int)DESTINATION.B] = b;
		packageCounts[(int)DESTINATION.C] = c;
		packageCounts[(int)DESTINATION.D] = d;
	}
};

public class Main : MonoBehaviour {

	public GameObject		packagePrefab;
	private List<Shipment> 	mShipments;
	private int[]			mTotalPackages;

	// Use this for initialization
	void Start () {
		mTotalPackages = new int[(int)DESTINATION.TOTAL];

		mShipments = new List<Shipment>();
		mShipments.Add(new Shipment(5.0f, 3, 3, 3, 3));
	}

	void Update() {
		updateShipments();
		pushPackages();
	}

	void updateShipments() {
		for(int s = 0; s < mShipments.Count; s++)
		{
			Shipment ship = mShipments[s];
			ship.timeTillArrival -= Time.deltaTime;
			mShipments[s] = ship;
			
			if(ship.timeTillArrival <= 0.0f)
			{
				unloadShipment(ship);
				mShipments.Remove(ship);
			}
		}
	}

	void pushPackages() {

		//If there are packages to send.
		if(mTotalPackages[(int)DESTINATION.A] > 0 || mTotalPackages[(int)DESTINATION.B] > 0 
		   || mTotalPackages[(int)DESTINATION.C] > 0 || mTotalPackages[(int)DESTINATION.D] > 0)
		{
			EntryShaft openShaft = null;

			//Find a free shaft.
			foreach(GameObject shaft in GameObject.FindGameObjectsWithTag("Shaft"))
			{
				if(shaft)
				{
					if(!shaft.GetComponent<EntryShaft>().inUse())
					{
						openShaft = shaft.GetComponent<EntryShaft>();
						break;
					}
				}
			}

			if(openShaft != null)
			{
				openShaft.setContact();

				GameObject packageObj = (GameObject) Instantiate(packagePrefab, openShaft.transform.position, Quaternion.identity);
				
				Package package = packageObj.GetComponent<Package>();
				package.setDirection(openShaft.exitDirection);

				//Select a random destination.
				int dest = -1;
				while(dest == -1)
				{
					dest = Random.Range(0, (int)DESTINATION.TOTAL);
					if(mTotalPackages[dest] > 0)
					{
						package.setDestination((DESTINATION) dest);
						mTotalPackages[dest]--;
					}
					else
						dest = -1;
				}
			}
		}
	}

	void unloadShipment(Shipment shipment) {
		mTotalPackages[(int)DESTINATION.A] += shipment.packageCounts[(int)DESTINATION.A];
		mTotalPackages[(int)DESTINATION.B] += shipment.packageCounts[(int)DESTINATION.B];
		mTotalPackages[(int)DESTINATION.C] += shipment.packageCounts[(int)DESTINATION.C];
		mTotalPackages[(int)DESTINATION.D] += shipment.packageCounts[(int)DESTINATION.D];
	}
	
}
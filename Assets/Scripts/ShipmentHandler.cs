using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameOverCause
{
	INCOMPLETE
};

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

public class ShipmentHandler : MonoBehaviour {

	public GameObject		packagePrefab;
	private Queue<Shipment> mShipments;

	private Shipment		mCurrentShipment;
	private int				mShipmentTotalPackages;

	private int				mGoodPackages, mBadPackages;

	// Use this for initialization
	void Start () {
		mShipments = new Queue<Shipment>();
		mShipments.Enqueue(new Shipment(10.0f, 2, 1, 1, 1));
	}

	void Update() {
		updateShipments();
		pushPackages();
	}

	void updateShipments() {
		if(mShipments.Count > 0)
		{
			Shipment ship = mShipments.Peek();
			ship.timeTillArrival -= Time.deltaTime;
			
			if(ship.timeTillArrival <= 0.0f)
				unloadShipment(mShipments.Dequeue());
		}

		if(mShipmentTotalPackages != 0)
		{
			if(mGoodPackages + mBadPackages + GameObject.FindGameObjectsWithTag("DestroyedPackage").Length >= mShipmentTotalPackages)
				shipmentComplete();
		}
	}

	public float getTTA() {
		if(mShipments.Count > 0)
			return mShipments.Peek().timeTillArrival;
		else
			return 0;
	}

	void pushPackages() {

		if(mShipmentTotalPackages != 0)
		{
			//If there are packages to send.
			if(mCurrentShipment.packageCounts[(int)DESTINATION.A] > 0 || mCurrentShipment.packageCounts[(int)DESTINATION.B] > 0 
			   || mCurrentShipment.packageCounts[(int)DESTINATION.C] > 0 || mCurrentShipment.packageCounts[(int)DESTINATION.D] > 0)
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
					
					PackageMover package = packageObj.GetComponent<PackageMover>();
					package.setDirection(openShaft.exitDirection);

					//Select a random destination.
					int dest = -1;
					while(dest == -1)
					{
						dest = Random.Range(0, (int)DESTINATION.NONE);
						if(mCurrentShipment.packageCounts[dest] > 0)
						{
							package.setDestination((DESTINATION) dest);
							mCurrentShipment.packageCounts[dest]--;
						}
						else
							dest = -1;
					}
				}
			}
		}
	}

	void unloadShipment(Shipment shipment) {
		if((mGoodPackages + mBadPackages + GameObject.FindGameObjectsWithTag("DestroyedPackage").Length) < mShipmentTotalPackages)
			gameOver(GameOverCause.INCOMPLETE);
		else
		{
			mGoodPackages = 0;
			mBadPackages = 0;

			mShipmentTotalPackages = 0;
			mShipmentTotalPackages += shipment.packageCounts[(int)DESTINATION.A];
			mShipmentTotalPackages += shipment.packageCounts[(int)DESTINATION.B];
			mShipmentTotalPackages += shipment.packageCounts[(int)DESTINATION.C];
			mShipmentTotalPackages += shipment.packageCounts[(int)DESTINATION.D];

			mCurrentShipment = shipment;
		}
	}

	public void gotGoodPackage() {
		mGoodPackages++;
	}

	public void gotBadPackage() {
		mBadPackages++;
	}

	void shipmentComplete() {
		PlayerState player = GetComponent<PlayerState>();
		player.addCredits(10*mGoodPackages);

		mShipmentTotalPackages = 0;
	}

	void gameOver(GameOverCause cause) {
	}
}
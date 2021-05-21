using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BeaconData : MonoBehaviour
{
	// Start is called before the first frame update
	public float latitude;
	public float longitude;
	public GameObject beacon;
	public bool Selected;
	public String[] paths;
	// public bool IsSelected { get; set; }

	public BeaconData(GameObject obj)
	{
		latitude = createBeacon.Latitude;
		longitude = createBeacon.Longitude;
		beacon = obj;
		paths = createBeacon.imagePaths;


		// paths = createBeacon.imagePaths;
	}
	public BeaconData()
	{
		latitude = createBeacon.Latitude;
		longitude = createBeacon.Longitude;
		beacon = null;
		// paths = createBeacon.imagePaths;
	}
}

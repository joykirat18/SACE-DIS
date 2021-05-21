using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ARLocation;
using UnityEngine.Events;
using Proyecto26;
using NativeGalleryNamespace;


public class createBeacon : MonoBehaviour
{
	// protected FirebaseStorage storage;
	// protected StorageReference storage_ref;
	public static float Latitude;
	public static float Longitude;
	public static string[] imagePaths = null;
	private Texture2D[] textures;
	private GameObject[] quadsList;
	public int count = 0;
	private Vector2 touchPosition = default;
	public GameObject deleteButton;
	public GameObject createBeaconButton;
	public GameObject CreateMarkerButton;
	public GameObject RemoveMarkerButton;
	public GameObject Canvas;
	// private Camera arCamera;
	private bool displayOverlay = false;
	[SerializeField]
	private Camera arCamera;
	[SerializeField]
	private Color inactiveColor = Color.gray;
	[SerializeField]
	private Color activeColor = Color.red;
	BeaconData beaconData = new BeaconData();
	public Text infoText;
	// public InputField latitudeText;
	// public InputField lonitudeText;
	public GameObject hexagonButton;
	public GameObject heartButton;
	public GameObject diamondButton;
	public GameObject starbutton;

	public GameObject hexagon;
	public GameObject heart;
	public GameObject diamond;
	public GameObject star;
	public GameObject hexagonBeacon;
	public GameObject heartBeacon;
	public GameObject diamondBeacon;
	public GameObject starBeacon;
	public GameObject Beacon;
	public GameObject Beacon1;
	// public GameObject Beacon3;
	public GameObject Beacon2;
	public GameObject Beacon3;
	public GameObject PlacementIndicator;
	public GameObject objectToSpawn;
	private GameObject indicator;

	public static List<BeaconData> beacons;
	public static List<PlacementObject> placedObjects;
	public bool isHamclicked;
	private PlacementIndicator placementIndicator;
	private int num;
	// Start is called before the first frame update
	private bool isSelectBeaconClicked;
	void Start()
	{
		// placementIndicator = FindObjectOfType<PlacementIndicator>();
		infoText.text = "";
		isSelectBeaconClicked = false;
		isHamclicked = false;
		hexagonButton.SetActive(false);
		starbutton.SetActive(false);
		diamondButton.SetActive(false);
		heartButton.SetActive(false);
		createBeaconButton.SetActive(false);
		CreateMarkerButton.SetActive(false);
		RemoveMarkerButton.SetActive(false);
		deleteButton.SetActive(false);
	}

	void Update()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			touchPosition = touch.position;

			if (touch.phase == TouchPhase.Began)
			{
				Ray ray = arCamera.ScreenPointToRay(touch.position);
				RaycastHit hitObject;
				if (Physics.Raycast(ray, out hitObject))
				{
					BeaconData placedBeacon = hitObject.transform.GetComponent<BeaconData>();
					if (placedBeacon != null)
					{
						ChangeSelectedObject(touchPosition, placedBeacon);
					}
				}
			}
		}
	}


	void ChangeSelectedObject(Vector3 touchPosition, BeaconData selected)
	{
		foreach (BeaconData current in beacons)
		{
			MeshRenderer meshRenderer = current.beacon.GetComponent<MeshRenderer>();
			// Color defaultColor = meshRenderer.material.color;
			if (selected.beacon != current.beacon)
			{
				current.Selected = false;

				meshRenderer.material.color = inactiveColor;
			}
			else
			{
				current.Selected = true;
				// infoText.text = selected.ToString() + " selected";
				meshRenderer.material.color = activeColor;
				onShowImages();

			}

		}
	}


	private IEnumerator StartLocationService()
	{
		if (!Input.location.isEnabledByUser)
		{
			Debug.Log("user has not enabled GPS");
			yield break;
		}

		Input.location.Start();
		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			print("Timed out");
			yield break;
		}
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			print("Unable to determine device location");
			yield break;
		}

		Latitude = Input.location.lastData.latitude;
		Longitude = Input.location.lastData.longitude;

		yield break;

	}
	private void getLocationCount()
	{
		RestClient.GetArray<BeaconData>("https://beacon-b8b9d-default-rtdb.firebaseio.com/.json").Then(response =>
		{
			num = response.Length;

		});
	}
	public void onStart()
	{
		Instantiate(Beacon1, transform.position, transform.rotation);
		Instantiate(Beacon2, transform.position, transform.rotation);
		Instantiate(Beacon3, transform.position, transform.rotation);
		// Instantiate(Beacon3, transform.position, transform.rotation);
		getLocationCount();
		count = num;
		infoText.text = "Data Fetched From server";
		beacons = new List<BeaconData>();
		placedObjects = new List<PlacementObject>();
		for (int i = 0; i < count; i++)
		{
			RetrieveFromDataBase(i);
		}

	}
	public void onHamClick()
	{
		if (isHamclicked)
		{
			createBeaconButton.SetActive(false);
			CreateMarkerButton.SetActive(false);
			RemoveMarkerButton.SetActive(false);
			isHamclicked = false;
		}
		else
		{
			createBeaconButton.SetActive(true);
			CreateMarkerButton.SetActive(true);
			RemoveMarkerButton.SetActive(true);
			isHamclicked = true;
		}
	}
	public void onCreateMarker()
	{
		indicator = Instantiate(PlacementIndicator, transform.position, transform.rotation);
	}

	public void onRemoveMarker()
	{
		Destroy(indicator);
	}

	public void onCreateBeacon()
	{
		placementIndicator = FindObjectOfType<PlacementIndicator>();
		GameObject obj = Instantiate(objectToSpawn, placementIndicator.transform.position, placementIndicator.transform.rotation);
		StartCoroutine(StartLocationService());
		PostToDataBase(obj);
		infoText.text = "Beacon created at Lat : " + Latitude.ToString() + " Long : " + Longitude.ToString();

	}
	public void onCreateBeaconWithImages()
	{
		placementIndicator = FindObjectOfType<PlacementIndicator>();
		GameObject obj = Instantiate(objectToSpawn, placementIndicator.transform.position, placementIndicator.transform.rotation);
		StartCoroutine(StartLocationService());
		pickMultipleImages();
		PostToDataBase(obj);
		infoText.text = "Beacon created at Lat : " + Latitude.ToString() + " Long : " + Longitude.ToString();



	}
	public void onSelectBeacon()
	{
		if (isSelectBeaconClicked)
		{
			hexagonButton.SetActive(false);
			starbutton.SetActive(false);
			diamondButton.SetActive(false);
			heartButton.SetActive(false);
			isSelectBeaconClicked = false;
		}
		else
		{
			hexagonButton.SetActive(true);
			starbutton.SetActive(true);
			diamondButton.SetActive(true);
			heartButton.SetActive(true);
			isSelectBeaconClicked = true;
		}

	}

	public void onHexagonClick()
	{
		Beacon = hexagonBeacon;
		objectToSpawn = hexagon;
		setButtonOff();
	}
	public void onStarClick()
	{
		Beacon = starBeacon;
		objectToSpawn = star;
		setButtonOff();
	}
	public void onDiamondClick()
	{
		Beacon = diamondBeacon;
		objectToSpawn = diamond;
		setButtonOff();
	}
	public void onHeartClick()
	{
		Beacon = heartBeacon;
		objectToSpawn = heart;
		setButtonOff();
	}
	private void setButtonOff()
	{
		hexagonButton.SetActive(false);
		starbutton.SetActive(false);
		diamondButton.SetActive(false);
		heartButton.SetActive(false);

	}
	private void onShowImages()
	{
		deleteButton.SetActive(true);
		// RetrieveFromDataBase(count - 1);
		string[] paths;
		// paths = beaconData.paths;
		int length;
		// int length = paths.Length;
		// if (paths == null)
		// {
		paths = imagePaths;
		length = imagePaths.Length;
		// }
		textures = new Texture2D[length];
		quadsList = new GameObject[length];
		int counter = 0;
		infoText.text = length.ToString();
		for (int i = 0; i < length; i++)
		{
			textures[i] = NativeGallery.LoadImageAtPath(paths[i], 512);
			if (textures[i] == null)
			{
				Debug.Log("Couldn't load texture from " + paths[i]);
				return;
			}
			counter += textures[i].width / 2;
			GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
			quadsList[i] = quad;
			if (length == 1)
			{
				quadsList[0].transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
			}
			if (length == 2)
			{
				quadsList[1].transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
				quadsList[0].transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
			}


			// quadsList[i].transform.position = new Vector3(Camera.main.transform.position.x + Vector3.right * counter, Camera.main.transform.position.y, Camera.main.transform.position.z + Camera.main.transform.forward.z * 2.5f);
			quadsList[i].transform.forward = Camera.main.transform.forward;
			quadsList[i].transform.localScale = new Vector3(1f, textures[i].height / (float)textures[i].width, 1f);
			Material material = quadsList[i].GetComponent<Renderer>().material;
			if (!material.shader.isSupported) // happens when Standard shader is not included in the build
				material.shader = Shader.Find("Legacy Shaders/Diffuse");

			material.mainTexture = textures[i];

		}



	}
	private void pickMultipleImages()
	{
		if (NativeGallery.IsMediaPickerBusy())
		{
			return;
		}
		// string[] paths;
		NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((images) =>
	   {
		   imagePaths = images;
		   foreach (string path in images)
		   {
			   if (path != null)
			   {

			   }
		   }
	   }, "Select a PNG image", "image/png");
	}

	public void onDeleteImages()
	{
		foreach (Texture2D t in textures)
		{
			Destroy(t);
		}
		foreach (GameObject gameObject in quadsList)
		{
			Destroy(gameObject);
		}
		deleteButton.SetActive(false);
	}
	public void onPickImage()
	{
		if (NativeGallery.IsMediaPickerBusy())
			return;
		PickImage(512);
	}

	// Update is called once per frame

	private void makeBeacon(int number)
	{
		var loc = new Location()
		{
			Latitude = beaconData.latitude,
			Longitude = beaconData.longitude,
			Altitude = 0,
			AltitudeMode = AltitudeMode.GroundRelative
		};
		var opts = new PlaceAtLocation.PlaceAtOptions()
		{
			HideObjectUntilItIsPlaced = true,
			MaxNumberOfLocationUpdates = 2,
			MovementSmoothing = 0.1f,
			UseMovingAverage = false
		};
		GameObject obj = PlaceAtLocation.CreatePlacedInstance(Beacon, loc, opts, true);
		beaconData.beacon = obj;
		beacons.Add(beaconData);
		// placedObjects.Add(obj);
		// obj.name = number.ToString();
	}
	private void PickImage(int maxSize)
	{
		NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
   {
	   Debug.Log("Image path: " + path);
	   if (path != null)
	   {
		   // Create Texture from selected image
		   Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
		   if (texture == null)
		   {
			   Debug.Log("Couldn't load texture from " + path);
			   return;
		   }

		   // Assign texture to a temporary quad and destroy it after 5 seconds
		   GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		   quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
		   quad.transform.forward = Camera.main.transform.forward;
		   quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

		   Material material = quad.GetComponent<Renderer>().material;
		   if (!material.shader.isSupported) // happens when Standard shader is not included in the build
			   material.shader = Shader.Find("Legacy Shaders/Diffuse");

		   material.mainTexture = texture;

		   Destroy(quad, 5f);

		   // If a procedural texture is not destroyed manually, 
		   // it will only be freed after a scene change
		   Destroy(texture, 5f);
	   }
   }, "Select a PNG image", "image/png");

		Debug.Log("Permission result: " + permission);
	}

	private void PostToDataBase(GameObject obj)
	{
		obj.name = count.ToString();
		BeaconData beaconData = new BeaconData(obj);
		beacons.Add(beaconData);
		// placedObjects.Add(obj);
		RestClient.Put("https://beacon-b8b9d-default-rtdb.firebaseio.com/" + count.ToString() + ".json", beaconData);
		count++;
	}
	private void RetrieveFromDataBase(int number)
	{
		// BeaconLocation beaconLocation = new BeaconLocation();
		RestClient.Get<BeaconData>("https://beacon-b8b9d-default-rtdb.firebaseio.com/" + number.ToString() + ".json").Then(response =>
		{
			beaconData = response;
			makeBeacon(number);
		});
	}


}

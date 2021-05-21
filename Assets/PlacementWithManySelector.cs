using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementWithManySelector : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField]
	private GameObject welcomePanel;
	public Text infoText;

	[SerializeField]
	private List<BeaconData> beaconsList = createBeacon.beacons;
	// public List<PlacementObject> placedObjects;

	[SerializeField]
	private Color activeColor = Color.red;

	[SerializeField]
	private Color inactiveColor = Color.gray;

	// [SerializeField]
	// private Button dismissButton;

	[SerializeField]
	private Camera arCamera;

	private Vector2 touchPosition = default;

	[SerializeField]
	private bool displayOverlay = false;

	// void Awake()
	// {
	// 	dismissButton.onClick.AddListener(Dismiss);
	// }

	void Start()
	{
		ChangeSelectedObject(beaconsList[0]);
	}

	// private void Dismiss() => welcomePanel.SetActive(false);

	void Update()
	{
		// do not capture events unless the welcome panel is hidden
		// if (welcomePanel.activeSelf)
		// 	return;

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
					BeaconData placementObject = hitObject.transform.GetComponent<BeaconData>();
					if (placementObject != null)
					{
						ChangeSelectedObject(placementObject);
					}
				}
			}
		}
	}

	void ChangeSelectedObject(BeaconData selected)
	{
		foreach (BeaconData current in beaconsList)
		{
			MeshRenderer meshRenderer = current.GetComponent<MeshRenderer>();
			if (selected != current)
			{
				current.Selected = false;
				meshRenderer.material.color = inactiveColor;
			}
			else
			{
				current.Selected = true;
				infoText.text = current.ToString() + " selected";
				meshRenderer.material.color = activeColor;
			}

			// if (displayOverlay)
			// current.beacon.ToggleOverlay();
		}
	}
}

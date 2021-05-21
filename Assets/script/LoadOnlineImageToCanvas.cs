using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadOnlineImageToCanvas : MonoBehaviour
{
	public string TextureURL;
	public GameObject rawImage;
	public Text infoText;
	private bool isLoaded = false;
	private Texture2D texture1;
	// public string TextureURL = "http://maps.google.com/maps/api/staticmap?&zoom=4&size=512x312&maptype=roadmap&markers=color:red|label:S|10.5466976,77.2522699&markers=color:red|label:G|24.5466976,77.2722699&markers=color:red|label:C|28.5466976,77.2722699&sensor=false&key=AIzaSyDfzPQSyJEov2pNQELey3g56OUilFiyNUY";
	// Start is called before the first frame update
	void Start()
	{
		// Debug.Log(apifetch.lat1);
		// Debug.Log(apifetch.lat2);
		// Debug.Log(apifetch.lang1);
		// Debug.Log(apifetch.lang2);
		// if (apifetch.lat1 != null)


		// StartCoroutine(ProcessRequest(URL));


	}

	// Update is called once per frame
	void Update()
	{
		if (apifetch.lang1 != null && !isLoaded)
		{
			Debug.Log(apifetch.lat1);
			// Debug.Log("ajay");
			TextureURL = "http://maps.google.com/maps/api/staticmap?&zoom=15&size=512x312&maptype=roadmap&markers=color:red|label:L|" + apifetch.lat1 + "" + "," + "" + apifetch.lang1 + " " + "&markers=color:red|label:G|" + apifetch.lat2 + "" + "," + "" + apifetch.lang2 + "" + "&sensor=false&key=AIzaSyDfzPQSyJEov2pNQELey3g56OUilFiyNUY";
			// http://maps.google.com/maps/api/staticmap?&zoom=1&size=512x312&maptype=roadmap&markers=color:red|label:L|:30.319599151611328,,:76.83067321777344}]%20&markers=color:red|label:G|:30.319599151611328,,:76.83067321777344}]&sensor=false&key=AIzaSyDfzPQSyJEov2pNQELey3g56OUilFiyNUY
			Debug.Log(TextureURL);
			StartCoroutine(DownloadImage(TextureURL));
			isLoaded = true;
		}
	}

	IEnumerator DownloadImage(string MediaUrl)
	{
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(MediaUrl);
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			infoText.text = "image not found";
			Debug.Log(www.error);
		}
		else
		{
			Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			// GUI.DrawTexture(new Rect(10, 10, 60, 60), myTexture, ScaleMode.ScaleToFit, true, 10.0F);
			texture1 = (Texture2D)myTexture;
			texture1.Apply();
			rawImage.GetComponent<RawImage>().texture = texture1;
			infoText.text = "image added";
		}
	}

	Sprite SpriteFromTexture2D(Texture2D texture)
	{
		return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
	}


}
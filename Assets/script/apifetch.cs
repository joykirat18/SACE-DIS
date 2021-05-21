using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
// using SimpleJSON;

public class apifetch : MonoBehaviour
{
	private const string URL = "https://beacon-b8b9d-default-rtdb.firebaseio.com/.json";
	static public List<string> latitude = new List<string>();
	static public List<string> longitude = new List<string>();
	public GameObject rawImage;
	private GameObject image;
	static public string lang1;
	static public string lang2;
	static public string lat1;
	static public string lat2;
	// public static GameObject image;

	void start()
	{
		rawImage.SetActive(false);
	}
	public void GenerateRequest()
	{
		// StartCoroutine(ProcessRequest(URL));
		rawImage.SetActive(true);
		StartCoroutine(ProcessRequest(URL));
		image = Instantiate(rawImage, transform.position, transform.rotation);
	}

	private IEnumerator ProcessRequest(string uri)
	{
		using (UnityWebRequest request = UnityWebRequest.Get(uri))
		{
			yield return request.SendWebRequest();
			if (request.isNetworkError)
			{
				Debug.Log(request.error);
			}
			else
			{
				string jsoni = request.downloadHandler.text;
				int n = 10;
				int[] arr = new int[n];
				List<string> json = new List<string>(jsoni.Split('"'));
				for (int i = 0; i < json.Count; i++)
				{
					int st = 0;
					string ajay = "latitude";
					st = string.Compare(ajay, json[i]);
					if (st == 0)
					{
						latitude.Add(json[i + 1]);

						longitude.Add(json[i + 3]);
					}
				}
				// Debug.Log("kumar");

				lang1 = longitude[longitude.Count - 1];
				lang2 = longitude[longitude.Count - 2];
				lat1 = latitude[latitude.Count - 1];
				lat2 = latitude[latitude.Count - 2];

				// Debug.Log("1");
				// Debug.Log(lang1);
				// Debug.Log(lang2);
				// Debug.Log(lat1);
				// Debug.Log(lat2);
				// Debug.Log("1");
				lang1 = lang1.Substring(1, lang1.Length - 3);
				lang2 = lang2.Substring(1, lang2.Length - 2);
				lat1 = lat1.Substring(1, lat1.Length - 2);
				lat2 = lat2.Substring(1, lat2.Length - 2);

				// Debug.Log("2");
				// Debug.Log(lang1);
				// Debug.Log(lang2);
				// Debug.Log(lat1);
				// Debug.Log(lat2);
				// Debug.Log("2");
			}
		}
	}

}
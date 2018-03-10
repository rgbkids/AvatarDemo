using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Uniduino;

public class Avatar : MonoBehaviour {

	private Animation animation;
	private string animeName = "";
	private long prevMilliTime = 0;
	public Arduino arduino;

	// Use this for initialization
	void Start () {
		//
		animation = GetComponent<Animation> ();
		animation.Stop ();
		//		StartCoroutine (GetHttpText ("mode=set&id=avatar001&anime=normal"));

		//
		arduino = Arduino.global;
		arduino.Setup(ConfigurePins);   
		StartCoroutine(BlinkLoop());
	}
	
	// Update is called once per frame
	void Update () {
		//
		long currentMilliTime = DateTime.Now.Hour * 60 * 60 * 1000 + DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
		if (currentMilliTime - prevMilliTime > 1 * 1000) {
//			StartCoroutine (GetHttpText ("mode=set&id=avatar001&anime=hold"));
			StartCoroutine (GetHttpText ("mode=get&id=avatar001"));
			prevMilliTime = currentMilliTime;
		}

		//
		if (animeName == "hold") {
			transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * 5, 0.7f), transform.position.z);
		} else {
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
	}

	//
	IEnumerator GetHttpText(string query) {
		UnityWebRequest request = UnityWebRequest.Get("http://ik1-325-22908.vs.sakura.ne.jp/?" + query);
		yield return request.Send();

		if (request.isError) {
			Debug.Log(request.error);
		} else {
			if (request.responseCode == 200) {
				string text = request.downloadHandler.text;
				byte[] results = request.downloadHandler.data;

				if (text == "hold") {
					animation.Play();
					animeName = text;
				} else {
					animation.Stop ();
					animeName = "";
				}
			}
		}
	}

	//
	void ConfigurePins( )
	{
		arduino.pinMode(13, PinMode.OUTPUT);
	}

	//
	IEnumerator BlinkLoop() {
		while(true) {
			Debug.Log ("BlinkLoop " + Time.deltaTime);

			if (animeName == "hold") {
				arduino.digitalWrite(13, Arduino.HIGH); // led ON
				yield return new WaitForSeconds(1);
			} else {
				arduino.digitalWrite(13, Arduino.LOW); // led OFF
				yield return new WaitForSeconds(2);
			}
		}           
	}
}

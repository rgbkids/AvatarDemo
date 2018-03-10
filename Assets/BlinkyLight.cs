using UnityEngine;
using System.Collections;
using Uniduino;

public class BlinkyLight : MonoBehaviour {

	public Arduino arduino;
	// Use this for initialization
	void Start () {
		arduino = Arduino.global;
		arduino.Setup(ConfigurePins);   
		StartCoroutine(BlinkLoop());
	}

	void ConfigurePins( )
	{
		arduino.pinMode(13, PinMode.OUTPUT);
	}

	IEnumerator BlinkLoop() {
		while(true) {
			Debug.Log ("BlinkLoop " + Time.deltaTime);

			arduino.digitalWrite(13, Arduino.HIGH); // led ON
			yield return new WaitForSeconds(1);
			arduino.digitalWrite(13, Arduino.LOW); // led OFF
			yield return new WaitForSeconds(2);
		}           
	}

	// Update is called once per frame
	void Update () {

	}
}

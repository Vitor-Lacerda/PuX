using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaidaFase : MonoBehaviour {


	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag ("Player")) {
			GameObject.FindObjectOfType<LevelManager> ().TerminarFase ();
		}
	}


}

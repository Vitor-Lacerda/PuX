using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public Transform jogador;
	public List<GameObject> fases;

	int faseAtual;
	GameObject objetoFaseAtual;

	// Use this for initialization
	void Start () {
		faseAtual = 0;
		CarregarFase ();
	}

	void CarregarFase(){
		if (faseAtual < fases.Count) {
			objetoFaseAtual = GameObject.Instantiate (fases [faseAtual]) as GameObject;
			jogador.transform.position = GameObject.FindWithTag ("Spawn").transform.position;
		}
	}

	void DestruirFase(){
		if (objetoFaseAtual != null) {
			Destroy (objetoFaseAtual);
			objetoFaseAtual = null;
		}
	}

	public void PassarDeFase(){
		faseAtual++;
		DestruirFase ();
		CarregarFase ();
	}

}

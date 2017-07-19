using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GUIManager guiManager;
	public ControlePersonagem jogador;
	public List<GameObject> fases;

	int faseAtual;
	GameObject objetoFaseAtual;

	// Use this for initialization
	void Start () {
		faseAtual = 0;
	}

	public void CarregarFase(int i){
		if (i < fases.Count) {
			objetoFaseAtual = GameObject.Instantiate (fases [i]) as GameObject;
			jogador.Renascer (GameObject.FindWithTag ("Spawn").transform.position);
			faseAtual = i;
		} else {
			guiManager.SelecaoDeFase.SetActive (true);
		}
	}

	public void TerminarFase(){
		jogador.gameObject.SetActive (false);
		DestruirFase ();
		guiManager.FinalDaFase.SetActive (true);
	}

	public void ProximaFase(){
		faseAtual++;
		RepetirFase ();
	}

	public void RepetirFase(){
		DestruirFase ();
		CarregarFase (faseAtual);
		guiManager.FinalDaFase.SetActive (false);
	}


	void DestruirFase(){
		if (objetoFaseAtual != null) {
			Destroy (objetoFaseAtual);
			objetoFaseAtual = null;
		}
	}


}

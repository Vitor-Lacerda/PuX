using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlePersonagem : MonoBehaviour {

	const int DIREITA = 1;
	const int ESQUERDA = -1;

	[Header("Posicoes")]
	public Transform origemCorda;
	public Transform[] posicoesRaycast;
	public Transform indicadorD;
	public Transform indicadorE;
	public LineRenderer lineRenderer;

	[Header("Atributos")]
	public float alcanceRaycasts = 0.2f;
	public float velocidade_horizontal = 1;
	public float velocidade_corda = 2;
	public float angulo_corda = 45;
	public float alcance_corda = 3;
	public float aceleracao = 1;
	public float desaceleracao = 1;
	public float velocidade_soltura = 0.2f;


	Rigidbody2D m_rigidBody;
	bool puxando = false;
	bool grudouParede = false;
	Vector2 direcao_movimento;
	Vector2 vetor_movimento;
	Vector2 posicaoSpawn;



	int sinal_direcao_horizontal;
	int Direcao_Horizontal;
	int direcao_horizontal { 
		get{ return Direcao_Horizontal; }
		set { 
			Direcao_Horizontal = value;
			sinal_direcao_horizontal = value / Mathf.Abs (value);
			direcao_movimento = new Vector2 (sinal_direcao_horizontal, 0);
		} 
	}


	void Start () {

		m_rigidBody = GetComponent<Rigidbody2D> ();

		posicaoSpawn = transform.position;

		direcao_horizontal = DIREITA;
		vetor_movimento = Vector2.zero;

	}

	void Update(){

		LancaCorda (DIREITA);
		LancaCorda (ESQUERDA);

		if (!puxando) {
			MovimentoHorizontal (NoChao ());

			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				Gancho (DIREITA);
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				Gancho (ESQUERDA);
			}
		} else {
			m_rigidBody.velocity = vetor_movimento;
			lineRenderer.SetPosition (0, origemCorda.position);
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			SoltarGancho ();
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			SoltarGancho ();
		}


	}

	void MovimentoHorizontal(bool noChao){
		Vector2 temp = m_rigidBody.velocity;
		temp.x = vetor_movimento.x;
		m_rigidBody.velocity = temp;

		if (noChao) {
			if (Mathf.Abs (vetor_movimento.x) < Mathf.Abs (direcao_movimento.x) * velocidade_horizontal) {
				vetor_movimento.x += aceleracao * sinal_direcao_horizontal * Time.deltaTime;

			}
			if (Mathf.Abs (vetor_movimento.x) > velocidade_horizontal) {
				vetor_movimento.x = velocidade_horizontal * sinal_direcao_horizontal;
			}
		} else {
			if (Mathf.Abs (vetor_movimento.x) > 0) {
				float fator = desaceleracao * sinal_direcao_horizontal * Time.deltaTime;
				if (Mathf.Abs(vetor_movimento.x) - Mathf.Abs(fator) >= 0) {
					vetor_movimento.x -= fator;
				} else {
					vetor_movimento.x = 0;
				}

			}
		}

	}


	void Gancho(int direcao){
		if (LancaCorda(direcao)) {
			grudouParede = false;
			Transform indicador = direcao == DIREITA ? indicadorD : indicadorE;
			m_rigidBody.gravityScale = 0;
			Vector2 hitCorda = indicador.position;

			lineRenderer.SetPosition (0, origemCorda.position);
			lineRenderer.SetPosition (1, hitCorda);
			lineRenderer.enabled = true;

			puxando = true;
			vetor_movimento = (hitCorda - (Vector2)origemCorda.position) * velocidade_corda;
			direcao_horizontal = direcao;
			direcao_movimento = new Vector2 (sinal_direcao_horizontal, 0);


		}
	}

	void SoltarGancho(){
		puxando = false;
		m_rigidBody.gravityScale = 1;
		lineRenderer.enabled = false;
		if (grudouParede) {
			vetor_movimento = direcao_movimento.normalized * velocidade_soltura;
			grudouParede = false;
		}

	}

	bool LancaCorda(int direcao){

		Quaternion rot = Quaternion.AngleAxis (angulo_corda*direcao, Vector3.forward);
		Vector2 v = new Vector2 (direcao, 0);
		v = rot*v;
		Transform indicador = direcao == DIREITA ? indicadorD : indicadorE;

		Debug.DrawRay (origemCorda.position, v*alcance_corda, Color.red);

		RaycastHit2D hit = Physics2D.Raycast (origemCorda.position, v,alcance_corda);
		if (hit.collider != null) {
			indicador.position = hit.point;
			indicador.gameObject.SetActive (true);
			return true;
		} else {
			indicador.gameObject.SetActive (false);
		}
		return false;

	}

	bool NoChao(){
		foreach (Transform pRay in posicoesRaycast) {
			Debug.DrawRay (pRay.position, Vector2.down*alcanceRaycasts, Color.white);
			RaycastHit2D hit = Physics2D.Raycast (pRay.position, Vector2.down, alcanceRaycasts);
			if (hit.collider != null) {
				return true;
			}
		}
		return false;
	}

	void InverterMovimento(bool imediato){
		direcao_horizontal *= -1;
		if (imediato) {
			vetor_movimento *= -1;
			if (Mathf.Abs (vetor_movimento.x) > velocidade_horizontal) {
				vetor_movimento.x = velocidade_horizontal * sinal_direcao_horizontal;
			}

		}
	}

	public void Renascer(Vector2 position){
		direcao_horizontal = DIREITA;
		vetor_movimento = Vector2.zero;

		posicaoSpawn = position;
		//Toca a animacao e pans
		gameObject.SetActive(true);
		transform.position = posicaoSpawn;
	}

	void Morrer(){
		gameObject.SetActive (false);
		//Toca a animacao e faz um comportamento
		Renascer(posicaoSpawn);
	}

	/*IEnumerator AnimaLinha(Vector2 pos, int direcao){

		lineRenderer.enabled = true;
		lineRenderer.SetPosition (0, origemCorda.position);
		lineRenderer.SetPosition (1, origemCorda.position);

		Vector2 linePos = lineRenderer.GetPosition (1);
		Vector2 diferenca = pos - linePos;

		Vector2 vetor_antigo = vetor_movimento;
		vetor_movimento = Vector2.zero;

		while (Vector2.Distance (linePos, pos) > 0.1f && lineRenderer.enabled) {
			linePos += diferenca * 8 * Time.deltaTime;
			lineRenderer.SetPosition (1, linePos);
			yield return null;
		}
		if (lineRenderer.enabled) {
			
		}


		yield return null;

	}*/


	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag ("Morte")) {
			Morrer ();
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		foreach (var ponto in col.contacts) {
			Debug.Log (ponto.normal.y);
			if (!puxando) {
				if (Mathf.Abs(Mathf.Abs(ponto.normal.y) - 1) > 0.01f) {
					InverterMovimento (true);
					break;
				} 
			}
			else {
				if (!NoChao ()) {
					vetor_movimento = Vector2.zero;
				}
				if (Mathf.Abs(Mathf.Abs(ponto.normal.y) - 1) > 0.01f) {
					InverterMovimento (false);
					grudouParede = true;
					break;
				}
			}
		}

	}






}

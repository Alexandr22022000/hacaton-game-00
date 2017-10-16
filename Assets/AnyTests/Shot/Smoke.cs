using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour {
	[SerializeField]
	ParticleSystem particle;

	void Start () {
		particle = GetComponent<ParticleSystem> ();
		particle.Stop ();
	}

	void Update () {

	}
	public void shot () {
		particle.Play();
		particle.Emit (5);
	}
}

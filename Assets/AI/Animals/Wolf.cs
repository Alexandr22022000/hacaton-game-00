using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent (typeof(Rigidbody))]

public class Wolf : MonoBehaviour {
	NavMeshAgent self;
	[SerializeField]
	GameObject target; 
	Vector3 targetPos;

	[SerializeField] [Range (0,200f)]
	float hp = 200f;
	[SerializeField]
	float dmg = 10f;
	[SerializeField] [Range(0f,20f)]
	float fear = 10f;
	[SerializeField]
	const float fearToEscape = 15f;
	[SerializeField]
	const float fearTo2 = 7f;
	[SerializeField]
	const float fearForAttack = 5f;

	[SerializeField]
	float[] speed = new float[4] {2f, 3f, 25f, 30f} ;
	[SerializeField]
	const float radius = 10f;
	[SerializeField]
	const float trigerDistance = 20f; 

	[SerializeField] [Range (0,3)]
	int state = 0;
	bool hasAttacked = false;
	float timer;
	[SerializeField]
	const float timerKD = 1f;
	[SerializeField]
	int folkCount;

	Vector3 radiusPos = Vector3.zero;
	float angle;
	Vector3 randomPath;


	void Start () {
		randomPath = transform.position;
		angle = Random.value * 360;
		self = GetComponent < NavMeshAgent> ();
		self.speed = speed[state];
		target = GameObject.FindWithTag ("Player");
		state = 0;
	}

    public void SetDamag (float damag)
    {
        hp -= damag;
    }
	

	void Update () {
		if (hp<=0) GameObject.Destroy(gameObject);
		self.speed = speed [state];
		targetPos = target.transform.position;
		Timer ();
		fearCalculate();

		switch (state) {
		case 0:
			randomWalk ();
			state =  (Vector3.Distance (targetPos, transform.position) < trigerDistance)? 1:0;
			break;
		case 1:
			waiting ();
			if (fear <= fearTo2) {
				state = 2;
				break;
			}			
			if (Vector3.Distance (targetPos, transform.position) > trigerDistance * 1.5f) {
				state = 0;
				break;
			}
			break;
		case 2:
			attack ();
			if (fear >= fearTo2)
				state = 1;
			break;
		case 3:
			run ();
			break;
		}

	}



	void waiting () {
		if (Vector3.Distance(radiusPos,transform.position)<1)
			angle += 1f;
			 
		radiusPos = targetPos + new Vector3 (Mathf.Cos (angle) * radius, 0, Mathf.Sin (angle) * radius);
		GetComponent<Animator> ().SetInteger ("State", 0);
		self.SetDestination (radiusPos);
		}


	void attack () {
		self.SetDestination (targetPos);
		if (Vector3.Distance (transform.transform.position, targetPos) < 8f)
		{	
			if (hasAttacked == false) {
				GetComponent<Animator> ().SetInteger ("State", 2);
				target.GetComponent<PlayerStatus>().SetDamag(dmg);
				fear += fearForAttack;
				hasAttacked = true;
				timer = 0f;
			}
		}
	}


	void Timer () {
		if (hasAttacked) {
			if (timer >= timerKD)
				hasAttacked = false;
			else
				timer += Time.deltaTime;
		}
	}


	void fearCalculate () 
	{
		if (state != 0 && state != 3) {
			fear -= 0.01f;
			if (target.GetComponent<PlayerStatus> ().shot)
				fear += 3f;
			if (target.GetComponent<PlayerStatus> ().torch)
				fear += 0.009f;
		}
		if (fear >= fearToEscape)
			state = 3;

	}


	void run()
	{
		GetComponent<Animator> ().SetInteger ("State", 1);
		Vector3 runDir =(transform.position + (transform.position-targetPos));
		self.SetDestination (runDir);
		print (state);
		if (Vector3.Distance (transform.position, targetPos) > 30f)
			fear = 10f;
			state = 0;
	}


	void randomWalk () {
		GetComponent<Animator> ().SetInteger ("State", 0);
		self.SetDestination (randomPath);
		if (Vector3.Distance(randomPath, transform.position)<3f || self.isStopped)
			randomPath = transform.position + new Vector3((Random.value-Random.value)* radius, 0, (Random.value-Random.value)* radius);
		

	}
}

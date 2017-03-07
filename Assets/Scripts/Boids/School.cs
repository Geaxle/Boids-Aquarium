using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aquab.boid {
	
	public class School : MonoBehaviour {

		[SerializeField] GameObject fish;
		[SerializeField] int popuplation = 10;
		[SerializeField] static int aquariumSize = 5;
		public static int SqrAquariumSize {get {return aquariumSize * aquariumSize;} }

		Boid[] fishes;
		Collider[] boidPool;


		// Use this for initialization
		void Start () {
			CreatePopulation();

		}
		
		// Update is called once per frame
		void Update () {
			UpdateBoidsVelocity ();
			UpdateBoidsPosition ();
		}

		void OnDrawGizmos() {
			Gizmos.color = Color.green;
        	Gizmos.DrawWireSphere(Vector3.zero, aquariumSize);
		}

		void CreatePopulation () {
			fishes = new Boid[popuplation];
			boidPool = new Collider[popuplation];

			for(int i = 0; i < popuplation; i++) {
				GameObject newFish = Instantiate(fish);
				newFish.transform.parent = transform;
				newFish.transform.position = Random.insideUnitSphere * aquariumSize;
				newFish.transform.rotation = Quaternion.Euler(Random.Range(-20f, 20f), Random.Range(0f, 360f), 0f );
				fishes[i] = newFish.GetComponent<Boid>();
				fishes[i].InitialiseBoidMemory(popuplation);
			}
		}

		void UpdateBoidsVelocity() {
			for(int i = 0; i < popuplation; i++) {
				fishes[i].ComputeVelocityFromSurroundings(boidPool);
			}
		}

		void UpdateBoidsPosition () {
			for(int i = 0; i < popuplation; i++) {
				fishes[i].UpdatePosition ();

				if(i==0) {
					Debug.Log(fishes[i].Velocity);
				}
			}
		}

	}

}

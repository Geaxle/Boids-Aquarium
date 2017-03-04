using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aquab.boid {
	
	public class School : MonoBehaviour {

		[SerializeField] GameObject fish;
		[SerializeField] int popuplation = 100;

		[SerializeField] int aquariumSize = 5;
		Vector3 aquariumDimensions;

		Boid[] fishes;
		Collider[] temporaryLocalBoidsReference;


		// Use this for initialization
		void Start () {
			CreatePopulation();
			aquariumDimensions = new Vector3(aquariumSize*2f, aquariumSize*2f, aquariumSize*2f);

		}
		
		// Update is called once per frame
		void Update () {
			UpdateBoidsVelocity ();
			UpdateBoidsPosition ();
		}

		void CreatePopulation () {
			fishes = new Boid[popuplation];
			temporaryLocalBoidsReference = new Collider[popuplation];

			for(int i = 0; i < popuplation; i++) {
				GameObject newFish = Instantiate(fish);
				newFish.transform.parent = transform;
				newFish.transform.position = Random.insideUnitSphere * aquariumSize;
				newFish.transform.rotation = Quaternion.Euler(Random.Range(-20f, 20f), Random.Range(0f, 360f), 0f );
				fishes[i] = newFish.GetComponent<Boid>();
			}
		}

		void UpdateBoidsVelocity() {
			for(int i = 0; i < popuplation; i++) {
				fishes[i].ComputeVelocityFromSurroundings(temporaryLocalBoidsReference);
			}
		}

		void UpdateBoidsPosition () {
			for(int i = 0; i < popuplation; i++) {
				fishes [i].UpdatePosition ();
			}
		}

	}

}

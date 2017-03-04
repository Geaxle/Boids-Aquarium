using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aquab.boid{
	
	public class Boid : MonoBehaviour {

		static float visionRadius = 1f;
		static float personalSpace = 0.1f;

		int localBoidsNumber;
		Vector3[] localBoidsPos;

		Vector3 velocity = Vector3.zero;


		public void UpdatePosition () {
			transform.position += velocity;
		}
			
		public void InitialiseBoidMemory(int populationSize) {
			localBoidsPos = new Vector3[populationSize];
		}

		public void ComputeVelocityFromSurroundings(Collider[] localBoids) {
			CheckSuroundings (localBoids);
			ComputeVelocity ();
		}

		void CheckSuroundings(Collider[] localBoids) {

			localBoidsNumber = Physics.OverlapSphereNonAlloc (transform.position, visionRadius, localBoids);

			int j = 0;
			for (int i = 0; i < localBoidsNumber; i++) {
				
				if (localBoids [i] != null && localBoids [i].gameObject != this.gameObject) {
					localBoidsPos[j] = localBoids [i].transform.position;
					j++;

				} else if (localBoids [i] == null) {
					break;
				}
			}
			 
			localBoidsNumber--; //-1 as the number includes this boid as well
		}

		void ComputeVelocity() {
			velocity += SeparationRule() + AlignmentRule() + CohesionRule();
		}

		Vector3 SeparationRule() {
			return Vector3.zero;
		}
		
		Vector3 AlignmentRule () {
			return Vector3.zero;
		}
		
		Vector3 CohesionRule () {
			return Vector3.zero;
		}

	}
}

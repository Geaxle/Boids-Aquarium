using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aquab.boid{
	
	public class Boid : MonoBehaviour {

		const float visionRadius = 1f;
		const float personalSpace = 0.2f; // sqr the distance for computation reason
		float SqrPersonalSpace {get {return personalSpace * personalSpace;} }
		const float reactionFactor = 0.05f;
		const float speed = 0.5f;
		const float noiseIntensity = 0.6f;
		const float noiseProbability = 0.01f;
		const float noiseWeight = 0.5f;
		const float boundaryWeight = 0.01f;

		int localBoidsNumber;
		Boid[] localBoids;

		public Vector3 Velocity {private set; get;}
		Vector3 targetVelocity;


		void Start() {
			Velocity = transform.forward;
			targetVelocity = transform.forward;
		}

		public void UpdatePosition () {
			Velocity = Vector3.Slerp(Velocity, targetVelocity, reactionFactor);
			//Velocity = Vector3.Lerp(Velocity, targetVelocity, reactionFactor);
			
			transform.forward = Velocity.normalized;
			transform.position += Velocity * Time.deltaTime * speed;
		}
			
		public void InitialiseBoidMemory(int populationSize) {
			localBoids = new Boid[populationSize];
		}

		public void ComputeVelocityFromSurroundings(Collider[] localBoids) {
			CheckSuroundings (localBoids);
			ComputeVelocity ();
		}

		void CheckSuroundings(Collider[] boidPool) {

			localBoidsNumber = Physics.OverlapSphereNonAlloc (transform.position, visionRadius, boidPool);
			
			if(localBoidsNumber > localBoids.Length || localBoidsNumber > boidPool.Length){
				Debug.LogError("localBoidsNumber is bigger than the array length it's going to itterate through (localBoids or boidPool). Exiting method.");
				return;
			} else if (localBoids.Length != boidPool.Length) {
				Debug.LogError("localBoids length is not equal to boidPool lenth, this should not happen. Exiting method.");
				return;
			}
			
			int j = 0;
			for (int i = 0; i < localBoidsNumber; i++) {
				
				if (boidPool[i] != null && boidPool[i].gameObject != this.gameObject) {
					localBoids[j] = boidPool[i].GetComponent<Boid>();
					j++;

				} else if (boidPool [i] == null) {
					break;
				}
			}
			 
			localBoidsNumber--; //-1 as the number includes this boid as well
		}

		void ComputeVelocity() {
			Vector3 personalSpaceCenter = Vector3.zero;
			Vector3 alignmentCenter = Vector3.zero;
			Vector3 cohesionCenter = Vector3.zero;

			for (int i=0; i<localBoidsNumber; i++) {
				// SeparationRule
				if((localBoids[i].transform.position - transform.position).sqrMagnitude < SqrPersonalSpace) {
					personalSpaceCenter += -(localBoids[i].transform.position - transform.position);
				}

				// AlignmentRule
				alignmentCenter += localBoids[i].transform.forward;

				// CohesionRule
				cohesionCenter += (localBoids[i].transform.position - transform.position);
			}

			if (localBoidsNumber>0) {
				alignmentCenter /= localBoidsNumber;
				alignmentCenter = alignmentCenter.normalized *0.01f;

				cohesionCenter /= localBoidsNumber;
				cohesionCenter *= 0.05f;

			}

			Vector3 noise = GetNoiseVector();

			Vector3 aquariumCenter = Vector3.zero;
			if(transform.position.sqrMagnitude > School.SqrAquariumSize) {
				aquariumCenter = -transform.position.normalized * boundaryWeight;
			}
			
			targetVelocity = noise + aquariumCenter + cohesionCenter + alignmentCenter;
			
		}

		Vector3 GetNoiseVector() {
			
			if(Random.value > noiseProbability) {
				return targetVelocity;
			} else {
				float noiseX = Random.Range(transform.forward.x - noiseIntensity, transform.forward.x + noiseIntensity);
				float noiseY = Random.Range(transform.forward.y - noiseIntensity, transform.forward.y + noiseIntensity);
				float noiseZ = Random.Range(transform.forward.z - noiseIntensity, transform.forward.z + noiseIntensity);
				return new Vector3(noiseX, noiseY, noiseZ).normalized * noiseWeight;
			}
		}

	}
}

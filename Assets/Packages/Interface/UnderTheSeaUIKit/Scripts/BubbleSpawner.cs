using UnityEngine;

namespace HungryCannibal.UnderTheSeaUIKit {
	[System.Serializable]
	public class BubbleSpawner : MonoBehaviour {
		public GameObject bubblePrefab;
		public Transform spawnPointContainer;

		[System.Serializable]
		public class SpawnFrequency {
			public float start = 1;
			public float end = 5;
		}
		public SpawnFrequency spawnFrequency;

		private RectTransform[] _spawnPoints;
		private float _nextSpawnTime;

		private void Awake() {
			//Get all possible spawn points
			_spawnPoints = new RectTransform[spawnPointContainer.childCount];
			for(int i = 0; i < spawnPointContainer.childCount; i++) {
				_spawnPoints[i] = spawnPointContainer.GetChild(i) as RectTransform;
			}
		}

		private void Update() {
			//Should we spawn another bubble?
			if(Time.time >= _nextSpawnTime) {
				//Get a random spawn point
				var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

				//Create a bubble at the point
				var bubbleInstance = Instantiate(bubblePrefab, transform, false) as GameObject;
				float scale = Random.Range(0.7f, 1f);
				bubbleInstance.transform.localScale = new Vector3(scale, scale, 1);
				bubbleInstance.transform.position = spawnPoint.position;
				if(Random.Range(0f, 1f) >= 0.5f) {
					bubbleInstance.transform.localScale = new Vector3(-bubbleInstance.transform.localScale.x, bubbleInstance.transform.localScale.y, 1);
				}

				//Get the next spawn time
				_nextSpawnTime = Time.time + Random.Range(spawnFrequency.start, spawnFrequency.end);
			}
		}
	}
}
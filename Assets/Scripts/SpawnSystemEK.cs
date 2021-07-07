using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystemEK : MonoBehaviour
{
	[SerializeField]
	private int defaultSpawnIndex;
	[SerializeField]
	private GameObject playerPrefab;
	[SerializeField]
	private Transform[] spawnLocationArray;

	// Start is called before the first frame update
	void Start()
    {
		Spawn(defaultSpawnIndex);
    }

	private void Spawn(int spawnIndex)
	{
		Transform spawnPos = GetSpawnLocation(spawnIndex, spawnLocationArray);
		Instantiate(playerPrefab, spawnPos);
	}

	private Transform GetSpawnLocation(int index, Transform[] locationArray)
	{
		index = Mathf.Clamp(index, 0, locationArray.Length - 1);
		return locationArray[index];
	}
}

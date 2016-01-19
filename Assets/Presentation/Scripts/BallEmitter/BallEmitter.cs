using UnityEngine;

public class BallEmitter : MonoBehaviour {
    // sphere setup
    public GameObject miniSpherePrefab;
    public float minScale = 0.1f;
    public float maxScale = 0.5f;
    Color[] colors = { Color.yellow, Color.blue, Color.red, Color.magenta, Color.green, Color.cyan };

    // spawning setup
    public float spawnChance = 0.1f;
    public int minSpawns = 1;
    public int maxSpawns = 4;
    public float spawnRadius = 5;
    public float timeOfLastSpawn = 0;
    public float respawnDelay = 0.5f;
    	
	// Update is called once per frame
	void Update ()
    {
	    if ((timeOfLastSpawn + respawnDelay < Time.time) && (spawnChance < Random.Range(0f, 1f)))
        {
            EmitBalls();
            timeOfLastSpawn = Time.time;
        }    
	}

    void EmitBalls()
    {
        int nSpawns = Random.Range(minSpawns, maxSpawns);
        for (int i = 0; i < nSpawns; ++i)
        {
            Vector3 spawnOffset = Random.insideUnitSphere * spawnRadius;
            spawnOffset.y = 0;
            Vector3 spawnLocation = gameObject.transform.position + spawnOffset;
            Color spawnColor = colors[Random.Range(0, colors.Length)];
            GameObject spawnedBall = (GameObject)Instantiate(miniSpherePrefab, spawnLocation, Quaternion.identity);
            spawnedBall.GetComponent<Renderer>().material.color = spawnColor;
            spawnedBall.transform.localScale = Random.Range(minScale, maxScale)*Vector3.one;
        }
    }
}

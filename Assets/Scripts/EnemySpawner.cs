using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float cooldown = 10f;
    public float progressionFactor = 0.95f;
    public int playingFieldDimensions = 8;

    float timer = 0;

    public GameObject[] enemiesPrefabs;
    public GameObject carrotPrefab;

    float difficulty = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            cooldown *= progressionFactor;
            timer = cooldown;
            difficulty += 0.2f;
            TriggerSpawn();
        }
    }

    void TriggerSpawn()
    {
        int randomDifficulty = Mathf.FloorToInt(Random.Range(0f, difficulty));
        randomDifficulty = Mathf.Clamp(randomDifficulty, 0, enemiesPrefabs.Length - 1);
        SpawnEnemy(randomDifficulty);
    }

    void SpawnEnemy(int type)
    {
        Vector3 enemyPos = new Vector3(
            Random.Range(-playingFieldDimensions, playingFieldDimensions),
            Random.Range(-playingFieldDimensions, playingFieldDimensions),
            0);
        Vector3 carrotPos = new Vector3(
            Random.Range(-playingFieldDimensions, playingFieldDimensions),
            Random.Range(-playingFieldDimensions, playingFieldDimensions),
            0);
        Instantiate(enemiesPrefabs[type], enemyPos, Quaternion.identity);
        Instantiate(carrotPrefab, carrotPos, Quaternion.identity);
    }
}

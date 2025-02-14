using UnityEngine;

public class Bunny : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Feed()
    {
        Vector2 spawnPosition = transform.position;
        spawnPosition += Random.insideUnitCircle;
        Instantiate(gameObject, spawnPosition, Quaternion.identity);
    }
}

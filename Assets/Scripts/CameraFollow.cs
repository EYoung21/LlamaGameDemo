using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5;

    void Start()
    {
        if(target == null)
        {
            target = FindAnyObjectByType<Player>().transform;
        }
    }

    void Update()
    {
        Vector3 targetPos = target.position;
        targetPos.z = -10;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothing);
    }
}

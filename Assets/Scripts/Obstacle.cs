using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);

        if(transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class BallColor : MonoBehaviour
{
    void Start()
{
    float r = Random.Range(0.2f, 1f);
    float g = Random.Range(0.2f, 1f);
    float b = Random.Range(0.2f, 1f);
    GetComponent<SpriteRenderer>().color = new Color(r,g,b);
}
}
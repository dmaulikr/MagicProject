using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DynamicRendering : MonoBehaviour {

    private SpriteRenderer spriteRenderer_;
    private float spriteHeight;

    void Start()
    {
        spriteRenderer_ = GetComponent<SpriteRenderer>();
        spriteHeight = spriteRenderer_.sprite.bounds.size.y;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.z = pos.y - spriteHeight/2f;
        transform.position = pos;
    }
}

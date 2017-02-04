using UnityEngine;
using System.Collections;

[RequireComponent( typeof(Rigidbody2D), typeof(CircleCollider2D) )]
public class Attract : MonoBehaviour {

    [SerializeField]
    private float maxAttractionVelocity_;
    [SerializeField]
    private float attractionExponent_;
    [SerializeField]
    private bool constAttractionVelocity_;
    [SerializeField]
    private float attractionVelocity_;

    private Rigidbody2D rb2D_;

    void Start()
    {
        rb2D_ = GetComponent<Rigidbody2D>();
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            Vector2 dir = coll.transform.position - transform.position;
            Vector2 normalizedDir = Vector3.Normalize(dir);

            if (constAttractionVelocity_) rb2D_.velocity = normalizedDir * attractionVelocity_;
            else rb2D_.velocity = normalizedDir * Mathf.Clamp(1 / Mathf.Pow(dir.magnitude, attractionExponent_), 0f, maxAttractionVelocity_);
        }
    }
}

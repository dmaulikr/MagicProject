using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {

    private PlayerActions movement_;
    private PlayerAnimation animation_;
    private InputHandler input_;
    private List<IComponent> components_ = new List<IComponent>();

    private Rigidbody2D rb2D_;
    private Animator animator_;

    [SerializeField]
    private float speed_;

	void Start ()
    {
        rb2D_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();

        movement_ = new PlayerActions(transform, speed_, rb2D_);
        animation_ = new PlayerAnimation(animator_, rb2D_);
        input_ = new InputHandler(movement_);

        movement_.addObserver(animation_);
        animation_.addObserver(movement_);

        components_.Add(animation_);
        components_.Add(input_);
	}
	
	void Update ()
    {
        foreach (IComponent comp in components_)
            comp.update();
	}

    void FixedUpdate()
    {
        movement_.fixedUpdate();
    }
}

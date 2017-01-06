using UnityEngine;
using System.Collections;

[RequireComponent( typeof(SpriteRenderer) )]
public class Chest : MonoBehaviour, IInteractable {

    [SerializeField]
    private Sprite openSprite_;

    private SpriteRenderer spriteRend_;

    void Start()
    {
        spriteRend_ = GetComponent<SpriteRenderer>();
    }

    public void interact()
    {
        spriteRend_.sprite = openSprite_;
    }
}

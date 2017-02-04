using UnityEngine;
using System.Collections;

[System.Serializable]
struct Drop
{
    public GameObject item;
    public int quantity;
    [Range(0f, 100f)]
    public float chance;
}

[RequireComponent( typeof(SpriteRenderer) )]
public class Chest : MonoBehaviour, IInteractable {

    [SerializeField]
    private Sprite openSprite_;
    [SerializeField]
    private Transform dropSpawn_;
    [SerializeField]
    private float noiseRange_;


    [SerializeField]
    private Drop[] drops;

    private SpriteRenderer spriteRend_;
    private bool isOpen = false;

    void Start()
    {
        spriteRend_ = GetComponent<SpriteRenderer>();
    }

    public void interact()
    {
        if (isOpen) return;
        spriteRend_.sprite = openSprite_;
        isOpen = true;
        foreach(Drop drop in drops)
        {
            for (int i = 0; i < drop.quantity; i++)
            {
                float rnd = Random.Range(0f, 100f);
                if (rnd > drop.chance) continue;

                Vector3 noise = new Vector2(0, 0);
                noise.x = Random.Range(-noiseRange_, noiseRange_);
                noise.y = Random.Range(-noiseRange_, noiseRange_);
                Instantiate(drop.item, dropSpawn_.position + noise, Quaternion.identity);
            }
        }
    }
}

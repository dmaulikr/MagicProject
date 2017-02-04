using UnityEngine;

public enum ItemType
{
    POTION, COINS
}

[System.Serializable]
public class ItemObject
{
    public ItemType type;
    public int itemQuantity;
    public Potion potionColor;
}

public class Item : MonoBehaviour {

    [SerializeField]
    private ItemObject obj_;
    [SerializeField]
    private bool isAttractable;

    private Inventory playerInventory_;

    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerInventory_ = player.GetComponent<Inventory>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            playerInventory_.AddItem(obj_);
            if (isAttractable) Destroy(transform.parent.gameObject);
            else Destroy(gameObject);
        }
    }
}

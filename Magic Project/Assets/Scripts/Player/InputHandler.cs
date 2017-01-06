using UnityEngine;

public class InputHandler : MonoBehaviour {

    private IGameActor playerActions_;
    private ICommand zKey_;
    private ICommand xKey_;
    private ICommand spaceKey_;

    void Start()
    {
        playerActions_ = GetComponent<PlayerActions>();

        zKey_ = new AttackCommand(playerActions_);
        xKey_ = new BuffCommand(playerActions_, Buff.BUFF_HEAL);
        spaceKey_ = new InteractCommand(playerActions_);
    }

    void Update()
    {
        if(playerActions_ != null) handleInput();
    }

    void handleInput()
    {
        float horzAxis = Input.GetAxisRaw("Horizontal");
        float vertAxis = Input.GetAxisRaw("Vertical");
        playerActions_.move(vertAxis, horzAxis);

        if (Input.GetKeyDown(KeyCode.Z)) zKey_.execute();
        if (Input.GetKeyDown(KeyCode.X)) xKey_.execute();
        if (Input.GetKeyDown(KeyCode.Space)) spaceKey_.execute();
    }

}

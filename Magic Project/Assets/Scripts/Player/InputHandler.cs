using UnityEngine;

public class InputHandler : IComponent {

    private PlayerActions playerMovement_;
    private ICommand zKey_;
    private ICommand xKey_;

    public InputHandler(PlayerActions playerMovement)
    {
        playerMovement_ = playerMovement;

        zKey_ = new AttackCommand(playerMovement_);
        xKey_ = new BuffCommand(playerMovement_, Buff.BUFF_HEAL);
    }

    public void update()
    {
        handleInput();

        //KILL ME
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    void handleInput()
    {
        float horzAxis = Input.GetAxisRaw("Horizontal");
        float vertAxis = Input.GetAxisRaw("Vertical");
        playerMovement_.Move(vertAxis, horzAxis);

        if (Input.GetKeyDown(KeyCode.Z)) zKey_.execute();
        if (Input.GetKeyDown(KeyCode.X)) xKey_.execute();

    }

}

using UnityEngine;
using System.Collections;

public class InputHandler : IComponent {

    private PlayerMovement playerMovement_;
    private ICommand zKey_;
    private ICommand xKey_;

    public InputHandler(PlayerMovement playerMovement)
    {
        playerMovement_ = playerMovement;

        zKey_ = new AttackCommand(playerMovement_);
        xKey_ = new AttackCommand(playerMovement_);
    }

    public void update()
    {
        handleInput();
    }

    void handleInput()
    {
        float horzAxis = Input.GetAxis("Horizontal");
        float vertAxis = Input.GetAxis("Vertical");
        playerMovement_.Move(vertAxis, horzAxis);

        if (Input.GetKeyDown(KeyCode.Z)) zKey_.execute();
        if (Input.GetKeyDown(KeyCode.X)) xKey_.execute();

    }

}

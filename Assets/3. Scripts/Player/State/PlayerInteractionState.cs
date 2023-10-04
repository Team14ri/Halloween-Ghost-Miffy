using DS.Core;
using DS.Runtime;
using UnityEngine;

public class PlayerInteractionState : IState
{
    private PlayerController player;
    private StateMachine stateMachine;

    private DialogueHandler dialogueHandler;
    private DialogueContainer dialogueContainer;

    public PlayerInteractionState(PlayerController player, StateMachine stateMachine, DialogueHandler dialogueHandler, DialogueContainer dialogueContainer)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.dialogueHandler = dialogueHandler;
        this.dialogueContainer = dialogueContainer;
    }

    public void Enter()
    {
        // Debug.Log(dialogueContainer.);
    }

    public void Execute()
    {
        Debug.Log(player.InteractionInput);
        player.InteractionInput = false;
    }

    public void Exit() { /* 정리 코드 */ }
}

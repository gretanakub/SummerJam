using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private bool keyboardJoined = false;
    private HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();
    private int playerCount = 0;

    void Update()
    {
        if (Keyboard.current == null) return;

        if (!keyboardJoined && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            var player = PlayerInput.Instantiate(playerPrefab,
                controlScheme: "KeyboardMouse",
                pairWithDevice: Keyboard.current);

            if (playerCount < spawnPoints.Length)
                player.transform.position = spawnPoints[playerCount].position;

            keyboardJoined = true;
            playerCount++;
        }

        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame && !joinedGamepads.Contains(gamepad) && playerCount < 4)
            {
                var player = PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "Gamepad",
                    pairWithDevice: gamepad);

                if (playerCount < spawnPoints.Length)
                    player.transform.position = spawnPoints[playerCount].position;

                joinedGamepads.Add(gamepad);
                playerCount++;
            }
        }
    }
}
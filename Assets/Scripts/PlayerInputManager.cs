using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private bool wasdJoined = false;
    private bool arrowsJoined = false;
    private HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();
    private int playerCount = 0;

    void Update()
    {
        if (Keyboard.current == null) return;

        if (!wasdJoined && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            var player = PlayerInput.Instantiate(playerPrefab,
                controlScheme: "WASD",
                pairWithDevice: Keyboard.current);

            if (playerCount < spawnPoints.Length)
                player.transform.position = spawnPoints[playerCount].position;

            wasdJoined = true;
            playerCount++;
        }

        if (!arrowsJoined && Keyboard.current.rightCtrlKey.wasPressedThisFrame)
        {
            var player = PlayerInput.Instantiate(playerPrefab,
                controlScheme: "Arrows",
                pairWithDevice: Keyboard.current);

            if (playerCount < spawnPoints.Length)
                player.transform.position = spawnPoints[playerCount].position;

            arrowsJoined = true;
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
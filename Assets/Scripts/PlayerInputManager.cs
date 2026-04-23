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

        int maxPlayers = PlayerSelectMenu.numberOfPlayers;

        if (!keyboardJoined && playerCount < maxPlayers && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            var player = PlayerInput.Instantiate(playerPrefab,
                controlScheme: "KeyboardMouse",
                pairWithDevice: Keyboard.current);

            if (playerCount < spawnPoints.Length)
                player.transform.position = spawnPoints[playerCount].position;

            SetupPlayer(player.gameObject, playerCount);
            keyboardJoined = true;
            playerCount++;
        }

        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame && !joinedGamepads.Contains(gamepad) && playerCount < maxPlayers)
            {
                var player = PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "Gamepad",
                    pairWithDevice: gamepad);

                if (playerCount < spawnPoints.Length)
                    player.transform.position = spawnPoints[playerCount].position;

                SetupPlayer(player.gameObject, playerCount);
                joinedGamepads.Add(gamepad);
                playerCount++;
            }
        }
    }

    void SetupPlayer(GameObject player, int index)
    {
        if (CharacterSelector.Instance == null) return;

        CharacterData data = CharacterSelector.Instance.GetCharacterForPlayer(index);
        if (data == null) return;

        WeaponSystem weapon = player.GetComponent<WeaponSystem>();
        if (weapon != null && data.weapon != null)
            weapon.SetWeapon(data.weapon);

        PlayerHealthSystem health = player.GetComponent<PlayerHealthSystem>();
        if (health != null)
        {
            health.maxHearts = data.maxHearts;
            health.currentHearts = data.maxHearts;
        }
    }
}
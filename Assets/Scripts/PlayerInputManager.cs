using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject defaultPlayerPrefab; // fallback ถ้าไม่มี playerPrefab ใน CharacterData
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
            CharacterData data = CharacterSelector.Instance?.GetCharacterForPlayer(playerCount);
            GameObject prefabToSpawn = (data != null && data.playerPrefab != null) ? data.playerPrefab : defaultPlayerPrefab;

            var player = PlayerInput.Instantiate(prefabToSpawn,
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
                CharacterData data = CharacterSelector.Instance?.GetCharacterForPlayer(playerCount);
                GameObject prefabToSpawn = (data != null && data.playerPrefab != null) ? data.playerPrefab : defaultPlayerPrefab;

                var player = PlayerInput.Instantiate(prefabToSpawn,
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
        if (CharacterSelector.Instance == null)
        {
            Debug.LogError("CharacterSelector.Instance เป็น NULL!");
            return;
        }

        CharacterData data = CharacterSelector.Instance.GetCharacterForPlayer(index);
        Debug.Log($"SetupPlayer index {index} ได้ character: {(data != null ? data.characterName : "NULL")}");

        if (data == null) return;

        PlayerSetup setup = player.GetComponent<PlayerSetup>();
        if (setup != null)
            setup.playerIndex = index;

        WeaponSystem weapon = player.GetComponent<WeaponSystem>();
        if (weapon != null && data.weapon != null)
            weapon.SetWeapon(data.weapon);

        PlayerHealthSystem health = player.GetComponent<PlayerHealthSystem>();
        if (health != null)
        {
            health.maxHearts = data.maxHearts;
            health.currentHearts = data.maxHearts;
        }

        if (data.weapon != null && data.weapon.weaponModelPrefab != null)
        {
            Transform handPoint = player.transform.Find("HandPoint");
            if (handPoint != null)
                Instantiate(data.weapon.weaponModelPrefab, handPoint);
        }
    }
}
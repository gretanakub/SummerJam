// ================================================================
//  GameSessionData.cs
//  Static class กลาง — รับ Input จาก Scene อื่น / ส่ง Output ไป GameScene
//  ไม่ต้อง DontDestroyOnLoad เพราะ static ค้างอยู่ตลอด runtime
// ================================================================
using System.Collections.Generic;
using UnityEngine;

public static class GameSessionData
{
    // ──────────────────────────────────────────────────────────
    //  INPUT  —  ตั้งค่าจาก Scene ก่อนหน้า (MainMenu / Lobby)
    // ──────────────────────────────────────────────────────────
    private static int _playerCount = 0;

    /// <summary>
    /// เรียกก่อน LoadScene("CharacterSelect")
    ///   GameSessionData.SetPlayerCount(2);
    ///   SceneManager.LoadScene("CharacterSelect");
    /// </summary>
    public static void SetPlayerCount(int count)
    {
        _playerCount = Mathf.Clamp(count, 1, 4);
        Debug.Log($"[GameSessionData] INPUT  → PlayerCount = {_playerCount}");
    }

    public static int PlayerCount => _playerCount;

    // ──────────────────────────────────────────────────────────
    //  OUTPUT  —  ผลลัพธ์หลังผู้เล่นเลือกตัวละครครบทุกคน
    // ──────────────────────────────────────────────────────────
    private static List<PlayerData> _players = new List<PlayerData>();

    /// <summary>CharacterSelectionManager เรียกอัตโนมัติเมื่อทุกคนเลือกครบ</summary>
    public static void SetPlayers(List<PlayerData> players)
    {
        _players = new List<PlayerData>(players);
        PrintOutput();
    }

    /// <summary>ดึง List ผู้เล่นทั้งหมด → ใช้ใน GameScene</summary>
    public static List<PlayerData> GetPlayers() => _players;

    /// <summary>ดึงตัวละครผู้เล่นตาม index (0-based)</summary>
    public static CharacterType GetCharacter(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < _players.Count)
            return _players[playerIndex].character;
        return CharacterType.None;
    }

    private static void PrintOutput()
    {
        Debug.Log($"[GameSessionData] OUTPUT → จำนวนผู้เล่น: {_players.Count}");
        foreach (var p in _players)
            Debug.Log($"[GameSessionData]          {p.playerName} → {p.character}");
    }
}

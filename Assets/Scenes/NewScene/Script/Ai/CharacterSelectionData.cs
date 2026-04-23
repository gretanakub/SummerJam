// ================================================================
//  CharacterSelectionData.cs
//  Data types ที่ใช้ร่วมกันทั้งระบบ
// ================================================================
using UnityEngine;

public enum CharacterType
{
    None,
    Commando,
    Swordman,
    Gunslinger,
    Vanguard
}

[System.Serializable]
public class PlayerData
{
    public int playerIndex;
    public CharacterType character = CharacterType.None;
    public string playerName => $"Player {playerIndex + 1}";

    public PlayerData(int index) { playerIndex = index; }
}

[System.Serializable]
public class CharacterInfo
{
    public CharacterType type;
    public string displayName;
    public Sprite portrait;
    public Color accentColor;
}

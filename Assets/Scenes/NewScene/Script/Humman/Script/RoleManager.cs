using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerRole { None, Commando, Swordman, Gunslinger, Vanguard }

public class RoleManager : MonoBehaviour
{
    public static RoleManager Instance;

    // เก็บข้อมูลแยกตาม ID ของผู้เล่น (Key = playerId, Value = Role)
    public Dictionary<int, PlayerRole> playerRoles = new Dictionary<int, PlayerRole>();

    // ส่ง Event โดยระบุ playerId ที่ทำการอัปเดต
    public static event Action<int, PlayerRole> OnRoleSelected;
    // Event สำหรับบอกให้ทุกคนอัปเดตปุ่มสีเทา
    public static event Action OnAnyRoleChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        // เคลียร์ค่าเริ่มต้นเพื่อป้องกันข้อมูลค้างจากรอบที่แล้ว
        playerRoles.Clear();
    }

    public void SelectRole(int playerId, PlayerRole role)
    {
        playerRoles[playerId] = role;

        // แจ้งเฉพาะเจ้าของ ID ให้เปลี่ยนรูป
        OnRoleSelected?.Invoke(playerId, role);
        // แจ้งทุกคนให้เช็คว่า Role ไหนถูกจองไปแล้วบ้าง
        OnAnyRoleChanged?.Invoke();
    }

    public bool IsRoleTaken(PlayerRole role)
    {
        if (role == PlayerRole.None) return false;
        foreach (var pair in playerRoles)
        {
            if (pair.Value == role) return true;
        }
        return false;
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("最高移動速度")]
    public float maxMoveSpeed = 0;
    [Header("加速度")]
    public float acceleration = 0;
    [Header("減速度")]
    public float deceleration = 0;
    [Header("ジャンプ力")]
    public float jumpPower = 0;
    [Header("重力")]
    public float gravityPower = 0;
    [Header("前ジャンプ力")]
    public float frontJumpPower = 0;
    [Header("前ジャンプ時間")]
    public float frontJumpTime = 0;
    [Header("回転速度")]
    public float rotationSpeed = 0;
}
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("移動")]
    [Header("最高移動速度")]
    public float MaxMoveSpeed = 0;
    [Header("加速度")]
    public float Acceleration = 0;
    [Header("減速度")]
    public float Deceleration = 0;
    [Header("回転速度")]
    public float RotationSpeed = 0;

    [Header("ジャンプ")]
    [Header("ジャンプ力")]
    public float JumpPower = 0;
    [Header("重力")]
    public float GravityPower = 0;
    [Header("前方向へのジャンプ力")]
    public float FrontJumpPower = 0;
    [Header("前方向へのジャンプ上昇力")]
    public float FrontJumpUpPower = 0;
    [Header("前方向のジャンプ時間")]
    public float FrontJumpTime = 0;
    [Header("前ジャンプ中の移動速度倍率")]
    public float frontJumpingMoveRate = 0;

    [Header("ノックバック")]
    [Header("ノックバック距離")]
    public float KnockbackDistance = 0;
    [Header("ノックバック力")]
    public float KnockbackPower = 0;
    [Header("ノックバック時間")]
    public float KnockbackTime = 0;
    [Header("ノックバック中の移動速度倍率")]
    public float knockbackMoveRate = 0;
}
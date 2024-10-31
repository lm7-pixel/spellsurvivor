﻿using fms.Projectile;
using Godot;

namespace fms.Weapon;

/// <summary>
///     ガンタレットを生成する
/// </summary>
public partial class GunTurretGen : WeaponBase
{
    [Export]
    private PackedScene _turret = null!;
    
    private uint _baseCoolDownFrame = 10u;

    // Projectileじゃないけど、WeaponBaseの機構にタダ乗りするためにSpawnProjectileメソッドを利用
    private protected override void SpawnProjectile(uint level)
    {
        // GunTurretを生成する。
        // 暫定的にturretはprojectileとして実装している  
        var prj = _turret.Instantiate<BaseProjectile>();
       
        // プレイヤーの位置にスポーンさせる
        AddProjectile(prj, GlobalPosition);
    }
}
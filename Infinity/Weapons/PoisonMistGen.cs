﻿using fms.Projectile;
using Godot;

namespace fms.Weapon;

/// <summary>
///     ポイズンミストを生成する
/// </summary>
public partial class PoisonMistGen : WeaponBase
{
    [ExportGroup("Internal Reference")]
    [Export]
    private PackedScene _bulletPackedScene = null!;

    [Export]
    private Node _bulletSpawnNode = null!;

    private int _trickshotBounceCount;
    private float _trickshotBounceDamageMultiplier;

    private protected override void DoAttack(uint level)
    {
        SpawnBullet(level);
    }

    private void SpawnBullet(uint level)
    {
        if (level == 1)
        {
            var bullet = _bulletPackedScene.Instantiate<PoisonMist>();
            {
                bullet.InitialPosition = Main.PlayerNode.GlobalPosition;
                bullet.CoolDown = 10; // 10 フレームに一回敵にダメージ
                bullet.BaseDamage = 2; // 10 フレームに1ダメージ
                bullet.Radius = 100; // 100 ピクセル以内の敵にダメージ
                bullet.LifeFrame = 300; // 300 フレーム後に消滅
            }
            _bulletSpawnNode.AddChild(bullet);
        }
        else if (level == 2)
        {
            var bullet = _bulletPackedScene.Instantiate<PoisonMist>();
            {
                bullet.InitialPosition = Main.PlayerNode.GlobalPosition;
                bullet.CoolDown = 10; // 10 フレームに一回敵にダメージ
                bullet.BaseDamage = 2; // 10 フレームに1ダメージ
                bullet.Radius = 200; // 200 ピクセル以内の敵にダメージ
                bullet.LifeFrame = 300; // 300 フレーム後に消滅
            }
            _bulletSpawnNode.AddChild(bullet);
        }
        else if (level == 3)
        {
            var bullet = _bulletPackedScene.Instantiate<PoisonMist>();
            {
                bullet.InitialPosition = Main.PlayerNode.GlobalPosition;
                bullet.CoolDown = 10; // 10 フレームに一回敵にダメージ
                bullet.BaseDamage = 2; // 10 フレームに1ダメージ
                bullet.Radius = 300; // 200 ピクセル以内の敵にダメージ
                bullet.LifeFrame = 400; // 400 フレーム後に消滅
            }
            _bulletSpawnNode.AddChild(bullet);
        }
    }
}
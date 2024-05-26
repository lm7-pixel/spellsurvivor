using Godot;
using R3;

namespace fms;

public partial class Enemy : RigidBody2D, IEntity
{
    private readonly Subject<DeadReason> _deadSubject = new();

    private bool _isHitStopping;

    [Export(PropertyHint.Range, "0,1000,1")]
    public float MoveSpeed { get; set; } = 50f;

    [Export(PropertyHint.Range, "0,100,1")]
    public float MaxHealth { get; private set; } = 100f;

    [Export(PropertyHint.Range, "0,100,1")]
    public float Health { get; private set; } = 100f;


    public Observable<DeadReason> Dead => _deadSubject;

    public Race Race => Race.Slime;

    void IEntity.TakeDamage(float amount, IEntity? instigator)
    {
        if (instigator is null)
        {
            return;
        }

        if (instigator.Race != Race.Player)
        {
            return;
        }

        Health -= amount;
        if (Health <= 0)
        {
            Health = 0;
            Deth(new DeadReason(instigator.Race.ToString(), "Projectile"));
        }
        else
        {
            TakeDamageAnimationAsync();
            UpdateHealthBar();
        }
    }

    private async void TakeDamageAnimationAsync()
    {
        var tex = GetNode<TextureRect>("Texture");
        if (tex.Material is not ShaderMaterial sm)
        {
            return;
        }

        // Hitstop and blink shader
        sm.SetShaderParameter("hit", 1.0f);
        _isHitStopping = true;
        LinearVelocity = Vector2.Zero;

        await this.WaitForSecondsAsync(0.1f);

        _isHitStopping = false;
        sm.SetShaderParameter("hit", 0.0f);
    }

    private void Deth(in DeadReason reason)
    {
        _deadSubject.OnNext(reason);
        QueueFree();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var notifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        notifier.ScreenExited += () => { Deth(new DeadReason("Screen", "OutOfScreen")); };
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        var healthBar = GetNode<ProgressBar>("HealthBar");
        healthBar.MaxValue = MaxHealth;
        healthBar.SetValueNoSignal(Health);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_isHitStopping)
        {
            return;
        }

        var playerPosition = Main.GetPlayerGlobalPosition();

        // Move to player
        var direction = playerPosition - GlobalPosition;
        direction = direction.Normalized();
        var force = direction * MoveSpeed;
        LinearVelocity = force;
    }

    public override void _ExitTree()
    {
        _deadSubject.Dispose();
        base._ExitTree();
    }
}
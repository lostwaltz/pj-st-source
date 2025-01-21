public interface IDamagable
{
    bool IsDead { get; }
    void TakeDamage(int damage);
}
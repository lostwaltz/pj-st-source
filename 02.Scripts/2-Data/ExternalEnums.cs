using System;

public static class ExternalEnums
{
    public enum Grade
    {
        Common = 0,
        Rare = 1,
        Epic = 2,
        Legendary = 3,
    }
    public enum Speciality
    {
        Vanguard = 0,
        Striker = 1,
        Supporter = 2,
        Defender = 3,
    }
    public enum WeaponType
    {
        AssaultRifle = 0,
        SniperRifle = 1,
        Pistol = 2,
        MachineGun = 3,
        SubMachineGun = 4,
        Blade = 5,
        ShotGun = 6,
        Bow = 7,
    }
    public enum AmmoType
    {
        LightWeight = 0,
        HeavyWeight = 1,
        MediumBomb = 2,
        HeavyBomb = 3,
    }
    public enum AchActionType
    {
        Kill = 0,
        Score = 1,
        Upgrade = 2,
        Clear = 3,
    }
    public enum AchTargetType
    {
        Monster = 0,
        Damage = 1,
        Spine = 2,
        Stage = 3,
    }
}

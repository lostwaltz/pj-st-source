using System;

namespace Constants
{
    public static class Path
    {
        public const string UIPath = "Prefabs/UI/";
        public const string DataPath = "Data/";
        public const string Prefabs = "Data/JSON/";
        public const string Indicators = "Prefabs/Indicators/";
        public const string Units = "Prefabs/Units/";
        public const string Obstacles = "Prefabs/Stage/Obstacle/";

        public const string Particles = "Prefabs/Particles/";
        public const string Effects = "Prefabs/Effects/";
        public const string SkillAction = "Prefabs/SkillAction/";
        public const string HitEffects = Particles + "HitEffect/";
        public const string FireEffects = Particles + "FireEffect/";
        public const string Projectiles = Particles + "Projectile/";
        public const string Reward = "Prefabs/Reward/";

        public const string GameManager = "Prefabs/GameManager";
    }

    public static class Sound
    {
        private const string BASE_PATH = "Sounds/";
        public const string BATTLE_PATH = BASE_PATH + "Battle/";
        public const string UISYSTEM_PATH = BASE_PATH + "UI/System/";
        public const string UIBATTLE_PATH = BASE_PATH + "UI/Battle/";
        public const string GACHASOUND_PATH = BASE_PATH + "UI/Gacha/";
        public const string BGM_PATH = BASE_PATH + "BGM/";
        public const string BGE_PATH = BASE_PATH + "BGE/";

        public static class Character
        {
            private const string CHARACTER_PATH = BATTLE_PATH + "Character/";
            public const string FOOTSTEPS = CHARACTER_PATH + "Footsteps/";
            public const string GUNGETS = CHARACTER_PATH + "GUNGETS/";
            public const string NEMESIS = CHARACTER_PATH + "Nemesis/";
        }

        public static class Gun
        {
            private const string GUN_PATH = BATTLE_PATH + "Gun/";
            public const string AR_PATH = GUN_PATH + "AR/";
            public const string HG_PATH = GUN_PATH + "HG/";
            public const string MG_PATH = GUN_PATH + "MG/";
            public const string RF_PATH = GUN_PATH + "RF/";
            public const string SG_PATH = GUN_PATH + "SG/";
            public const string SMG_PATH = GUN_PATH + "SMG/";
        }


    }

    public static class CommonUI
    {
        public const string CONFIRM = "확인";
        public const string RETRY = "재시도";

        public const string WARNING_ON_COMPLETED_UNIT = "이미 행동이 완료된 유닛입니다.";
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Trainer
{
    public enum GameStats
    {
        MyPkmLevel,
        MyPkmAttack,
        MyPkmMoves,
        MyPkmType1,
        MyPkmType2,
        MyPkmHealth,
        OpponentDefense,
        OpponentType1,
        OpponentType2
        
    }

    /// <summary>
    /// In index order
    /// </summary>
    public enum PokemonMoves
    {
        None = 0,
        Pound = 1,
        KarateChop,
        DoubleSlap,
        CometPunch,
        MegaPunch,
        PayDay,
        FirePunch,
        IcePunch,
        ThunderPunch,
        Scratch,
        ViceGrip,
        Guillotine,
        RazorWind,
        SwordsDance,
        Cut,
        Gust,
        WingAttack,
        Whirlwind,
        Fly,
        Bind,
        Slam,
        VineWhip,
        Stomp,
        DoubleKick,
        MegaKick,
        JumpKick,
        RollingKick,
        SandAttack,
        Headbutt,
        HornAttack,
        FuryAttack,
        HornDrill,
        Tackle,
        BodySlam,
        Wrap,
        TakeDown,
        Thrash,
        DoubleEdge,
        TailWhip,
        PoisonSting,
        Twineedle,
        PinMissile,
        Leer,
        Bite,
        Growl,
        Roar,
        Sing,
        Supersonic,
        SonicBoom,
        Disable,
        Acid,
        Ember,
        Flamethrower,
        Mist,
        WaterGun,
        HydroPump,
        Surf,
        IceBeam,
        Blizzard,
        Psybeam,
        BubbleBeam,
        AuroraBeam,
        HyperBeam,
        Peck,
        DrillPeck,
        Submission,
        LowKick,
        Counter,
        SeismicToss,
        Strength,
        Absorb,
        MegaDrain,
        LeechSeed,
        Growth,
        RazorLeaf,
        SolarBeam,
        PoisonPowder,
        StunSpore,
        SleepPowder,
        PetalDance,
        StringShot,
        DragonRage,
        FireSpin,
        ThunderShock,
        Thunderbolt,
        ThunderWave,
        Thunder,
        RockThrow,
        Earthquake,
        Fissure,
        Dig,
        Toxic,
        Confusion,
        Psychic,
        Hypnosis,
        Meditate,
        Agility,
        QuickAttack,
        Rage,
        Teleport,
        NightShade,
        Mimic,
        Screech,
        DoubleTeam,
        Recover,
        Harden,
        Minimize,
        Smokescreen,
        ConfuseRay,
        Withdraw,
        DefenseCurl,
        Barrier,
        LightScreen,
        Haze,
        Reflect,
        FocusEnergy,
        Bide,
        Metronome,
        MirrorMove,
        SelfDestruct,
        EggBomb,
        Lick,
        Smog,
        Sludge,
        BoneClub,
        FireBlast,
        Waterfall,
        Clamp,
        Swift,
        SkullBash,
        SpikeCannon,
        Constrict,
        Amnesia,
        Kinesis,
        SoftBoiled,
        HighJumpKick,
        Glare,
        DreamEater,
        PoisonGas,
        Barrage,
        LeechLife,
        LovelyKiss,
        SkyAttack,
        Transform,
        Bubble,
        DizzyPunch,
        Spore,
        Flash,
        Psywave,
        Splash,
        AcidArmor,
        Crabhammer,
        Explosion,
        FurySwipes,
        Bonemerang,
        Rest,
        RockSlide,
        HyperFang,
        Sharpen,
        Conversion,
        TriAttack,
        SuperFang,
        Slash,
        Substitute,
        Struggle
    }

    /// <summary>
    /// In Hex index order
    /// </summary>
    public enum PokemonTypes
    {
        Normal = 0,
        Fighting,
        Flying,
        Poison,
        Ground,
        Rock,
        Bug,
        Ghost,
        Fire,
        Water,
        Grass,
        Electric,
        Psychic,
        Ice,
        Dragon
    }

    /// <summary>
    /// Type of move to do.
    /// </summary>
    public enum ActionTypes
    {
        Attack = 0,
        Switch,
        Item,
        Escape
    }

    /// <summary>
    /// Holds the information about a certain move
    /// </summary>
    public class MoveStatistic
    {
        public PokemonTypes MoveType;
        public float AttackPower;
        public float Accuracy;

        public MoveStatistic(PokemonTypes t, int at, float ac)
        {
            this.MoveType = t;
            this.AttackPower = at;
            this.Accuracy = ac;
        }
    }

    /// <summary>
    /// Contains data needed to calculate battle damage
    /// </summary>
    public class Maps
    {
        public Dictionary<GameStats, int> StatAddressMap; //Maps Stat to its location in VBA WRAM and the length of the stat
        public Dictionary<GameStats, int> StatLengthMap; //Maps Stat to its length in VBA WRAM
        public Dictionary<PokemonMoves, MoveStatistic> MoveStatsMap; // Maps Pokemon moves to its <Attack type, power, accuracy>
        public float[,] TypeModiferChart;  

        public Maps()
        {
            populateStatAddresses();
            populateStatLengths();
            populateTypeModifiers();
            populateMoveStats();
            Debug.WriteLine(MoveStatsMap.Count);
        }

        private void populateStatAddresses()
        {
            StatAddressMap = new Dictionary<GameStats, int>();
            StatAddressMap.Add(GameStats.MyPkmAttack, 0x1025);
            StatAddressMap.Add(GameStats.MyPkmLevel, 0x1022);
            StatAddressMap.Add(GameStats.MyPkmMoves, 0x101C);
            StatAddressMap.Add(GameStats.MyPkmType1, 0x1019);
            StatAddressMap.Add(GameStats.MyPkmType2, 0x101A);
            StatAddressMap.Add(GameStats.MyPkmHealth, 0x1015);
            StatAddressMap.Add(GameStats.OpponentDefense, 0x0ff8);
            StatAddressMap.Add(GameStats.OpponentType1, 0x0fea);
            StatAddressMap.Add(GameStats.OpponentType2, 0x0feb); 

        }

        private void populateStatLengths()
        {
            StatLengthMap = new Dictionary<GameStats, int>();
            StatLengthMap.Add(GameStats.MyPkmAttack, 2);
            StatLengthMap.Add(GameStats.MyPkmLevel, 1);
            StatLengthMap.Add(GameStats.MyPkmMoves, 4);
            StatLengthMap.Add(GameStats.MyPkmType1, 1);
            StatLengthMap.Add(GameStats.MyPkmType2, 1);
            StatLengthMap.Add(GameStats.MyPkmHealth, 2);
            StatLengthMap.Add(GameStats.OpponentDefense,2);
            StatLengthMap.Add(GameStats.OpponentType1, 1);
            StatLengthMap.Add(GameStats.OpponentType2, 1); 
        }


        /// <summary>
        /// Data is from https://www.math.miami.edu/~jam/azure/compendium/typechart.htm
        /// first index is attack type, second index is opponent pokemon type
        /// </summary>
        private void populateTypeModifiers()
        {
            TypeModiferChart = new float[15, 15];
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    TypeModiferChart[i, j] = 1;
                }
            }
            //Fire attack
            TypeModiferChart[(int) PokemonTypes.Fire, (int) PokemonTypes.Fire] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Fire, (int)PokemonTypes.Water] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Fire, (int)PokemonTypes.Grass] = 2f;
            TypeModiferChart[(int)PokemonTypes.Fire, (int)PokemonTypes.Ice] = 2;
            TypeModiferChart[(int)PokemonTypes.Fire, (int)PokemonTypes.Rock] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Fire, (int)PokemonTypes.Bug] = 2;
            TypeModiferChart[(int)PokemonTypes.Fire, (int)PokemonTypes.Dragon] = 0.5f;

            //Water attack
            TypeModiferChart[(int)PokemonTypes.Water, (int)PokemonTypes.Water] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Water, (int)PokemonTypes.Grass] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Water, (int)PokemonTypes.Dragon] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Water, (int)PokemonTypes.Fire] = 2;
            TypeModiferChart[(int)PokemonTypes.Water, (int)PokemonTypes.Ground] = 2;
            TypeModiferChart[(int)PokemonTypes.Water, (int)PokemonTypes.Rock] = 2;

            //Grass attack
            TypeModiferChart[(int)PokemonTypes.Grass, (int)PokemonTypes.Fire] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Grass, (int)PokemonTypes.Water] = 2f;
            TypeModiferChart[(int)PokemonTypes.Grass, (int)PokemonTypes.Grass] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Grass, (int)PokemonTypes.Flying] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Grass, (int)PokemonTypes.Ground] = 2;
            TypeModiferChart[(int)PokemonTypes.Grass, (int)PokemonTypes.Rock] = 2;
            TypeModiferChart[(int)PokemonTypes.Grass, (int)PokemonTypes.Bug] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Grass, (int)PokemonTypes.Poison] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Grass, (int)PokemonTypes.Dragon] = 0.5f;

            //Electric Attack
            TypeModiferChart[(int)PokemonTypes.Electric, (int)PokemonTypes.Water] = 2;
            TypeModiferChart[(int)PokemonTypes.Electric, (int)PokemonTypes.Grass] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Electric, (int)PokemonTypes.Flying] = 2;
            TypeModiferChart[(int)PokemonTypes.Electric, (int)PokemonTypes.Ground] = 0;
            TypeModiferChart[(int)PokemonTypes.Electric, (int)PokemonTypes.Dragon] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Electric, (int)PokemonTypes.Electric] = 0.5f;

            //Ice attack
            TypeModiferChart[(int)PokemonTypes.Ice, (int)PokemonTypes.Dragon] = 2;
            TypeModiferChart[(int)PokemonTypes.Ice, (int)PokemonTypes.Grass] = 2;
            TypeModiferChart[(int)PokemonTypes.Ice, (int)PokemonTypes.Flying] = 2;
            TypeModiferChart[(int)PokemonTypes.Ice, (int)PokemonTypes.Ground] = 2;
            TypeModiferChart[(int)PokemonTypes.Ice, (int)PokemonTypes.Water] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Ice, (int)PokemonTypes.Ice] = 0.5f;

            //Psychic Attack
            TypeModiferChart[(int)PokemonTypes.Psychic, (int)PokemonTypes.Psychic] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Psychic, (int)PokemonTypes.Fighting] =2;
            TypeModiferChart[(int)PokemonTypes.Psychic, (int)PokemonTypes.Poison] = 2;

            //Normal attack
            TypeModiferChart[(int)PokemonTypes.Normal, (int)PokemonTypes.Rock] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Ghost, (int)PokemonTypes.Poison] = 0;

            //fighting
            TypeModiferChart[(int)PokemonTypes.Fighting, (int)PokemonTypes.Ice] = 2;
            TypeModiferChart[(int)PokemonTypes.Fighting, (int)PokemonTypes.Psychic] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Fighting, (int)PokemonTypes.Normal] = 2;
            TypeModiferChart[(int)PokemonTypes.Fighting, (int)PokemonTypes.Flying] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Fighting, (int)PokemonTypes.Bug] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Fighting, (int)PokemonTypes.Poison] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Fighting, (int)PokemonTypes.Ghost] = 0;

            //flying attack
            TypeModiferChart[(int)PokemonTypes.Flying, (int)PokemonTypes.Electric] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Flying, (int)PokemonTypes.Rock] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Flying, (int)PokemonTypes.Grass] = 2;
            TypeModiferChart[(int)PokemonTypes.Flying, (int)PokemonTypes.Fighting] = 2;
            TypeModiferChart[(int)PokemonTypes.Flying, (int)PokemonTypes.Bug] = 2;
            

            //Ground attack
            TypeModiferChart[(int)PokemonTypes.Ground, (int)PokemonTypes.Electric] = 2;
            TypeModiferChart[(int)PokemonTypes.Ground, (int)PokemonTypes.Grass] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Ground, (int)PokemonTypes.Fire] = 2;
            TypeModiferChart[(int)PokemonTypes.Ground, (int)PokemonTypes.Flying] = 0;
            TypeModiferChart[(int)PokemonTypes.Ground, (int)PokemonTypes.Rock] = 2;
            TypeModiferChart[(int)PokemonTypes.Ground, (int)PokemonTypes.Bug] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Ground, (int)PokemonTypes.Poison] = 2;

            //Rock attack
            TypeModiferChart[(int)PokemonTypes.Rock, (int)PokemonTypes.Fire] = 2;
            TypeModiferChart[(int)PokemonTypes.Rock, (int)PokemonTypes.Ice] = 2;
            TypeModiferChart[(int)PokemonTypes.Rock, (int)PokemonTypes.Flying] = 2;
            TypeModiferChart[(int)PokemonTypes.Rock, (int)PokemonTypes.Poison] = 2;
            TypeModiferChart[(int)PokemonTypes.Rock, (int)PokemonTypes.Fighting] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Rock, (int)PokemonTypes.Ground] = 0.5f;

            //Bug attack
            TypeModiferChart[(int)PokemonTypes.Bug, (int)PokemonTypes.Fire] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Bug, (int)PokemonTypes.Grass] = 2;
            TypeModiferChart[(int)PokemonTypes.Bug, (int)PokemonTypes.Psychic] = 2;
            TypeModiferChart[(int)PokemonTypes.Bug, (int)PokemonTypes.Fighting] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Bug, (int)PokemonTypes.Flying] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Bug, (int)PokemonTypes.Poison] = 2;

            //Poison attack
            TypeModiferChart[(int)PokemonTypes.Poison, (int)PokemonTypes.Grass] = 2;
            TypeModiferChart[(int)PokemonTypes.Poison, (int)PokemonTypes.Bug] = 2;
            TypeModiferChart[(int)PokemonTypes.Poison, (int)PokemonTypes.Rock] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Poison, (int)PokemonTypes.Ground] = 0.5f ;
            TypeModiferChart[(int)PokemonTypes.Poison, (int)PokemonTypes.Poison] = 0.5f;
            TypeModiferChart[(int)PokemonTypes.Poison, (int)PokemonTypes.Ghost] = 0.5f;

            //Ghost attack
            TypeModiferChart[(int)PokemonTypes.Poison, (int)PokemonTypes.Psychic] = 0;
            TypeModiferChart[(int)PokemonTypes.Poison, (int)PokemonTypes.Normal] = 0;
            TypeModiferChart[(int)PokemonTypes.Poison, (int)PokemonTypes.Ghost] = 2;

            //Dragon attack
            TypeModiferChart[(int)PokemonTypes.Dragon, (int)PokemonTypes.Dragon] = 2;
         
        }

        /// <summary>
        /// Data is from http://bulbapedia.bulbagarden.net/wiki/List_of_moves#Generation_I
        /// </summary>
        private void populateMoveStats()
        {
            MoveStatsMap = new Dictionary<PokemonMoves, MoveStatistic>();
            MoveStatsMap.Add(PokemonMoves.Pound, new MoveStatistic(PokemonTypes.Normal, 40, 1));
            MoveStatsMap.Add(PokemonMoves.KarateChop, new MoveStatistic(PokemonTypes.Fighting, 50, 1));
            MoveStatsMap.Add(PokemonMoves.DoubleSlap, new MoveStatistic(PokemonTypes.Normal, 15, 0.85f));
            MoveStatsMap.Add(PokemonMoves.CometPunch, new MoveStatistic(PokemonTypes.Normal, 18, 0.85f));
            MoveStatsMap.Add(PokemonMoves.MegaPunch, new MoveStatistic(PokemonTypes.Normal, 80, 0.85f));
            MoveStatsMap.Add(PokemonMoves.PayDay, new MoveStatistic(PokemonTypes.Normal, 40, 1));
            MoveStatsMap.Add(PokemonMoves.FirePunch, new MoveStatistic(PokemonTypes.Fire, 75, 1));
            MoveStatsMap.Add(PokemonMoves.IcePunch, new MoveStatistic(PokemonTypes.Ice, 75, 1));
            MoveStatsMap.Add(PokemonMoves.ThunderPunch, new MoveStatistic(PokemonTypes.Electric, 75, 1));
            MoveStatsMap.Add(PokemonMoves.Scratch, new MoveStatistic(PokemonTypes.Normal, 40, 1));
            MoveStatsMap.Add(PokemonMoves.ViceGrip, new MoveStatistic(PokemonTypes.Normal, 55, 1));
            MoveStatsMap.Add(PokemonMoves.Guillotine, new MoveStatistic(PokemonTypes.Normal, 50, 1)); //***************
            MoveStatsMap.Add(PokemonMoves.RazorWind, new MoveStatistic(PokemonTypes.Normal, 80, 1));
            MoveStatsMap.Add(PokemonMoves.SwordsDance, new MoveStatistic(PokemonTypes.Normal, 0, 0));
            MoveStatsMap.Add(PokemonMoves.Cut, new MoveStatistic(PokemonTypes.Normal, 50, .95f));
            MoveStatsMap.Add(PokemonMoves.Gust, new MoveStatistic(PokemonTypes.Flying, 40, 1));
            MoveStatsMap.Add(PokemonMoves.WingAttack, new MoveStatistic(PokemonTypes.Flying, 60, 1));
            MoveStatsMap.Add(PokemonMoves.Whirlwind, new MoveStatistic(PokemonTypes.Normal, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Fly, new MoveStatistic(PokemonTypes.Flying, 90, 0.95f));
            MoveStatsMap.Add(PokemonMoves.Bind, new MoveStatistic(PokemonTypes.Normal, 15, 0.85f));
            MoveStatsMap.Add(PokemonMoves.Slam, new MoveStatistic(PokemonTypes.Flying, 80, 0.75f));
            MoveStatsMap.Add(PokemonMoves.VineWhip, new MoveStatistic(PokemonTypes.Grass, 45, 1));
            MoveStatsMap.Add(PokemonMoves.Stomp, new MoveStatistic(PokemonTypes.Flying, 65, 1));
            MoveStatsMap.Add(PokemonMoves.DoubleKick, new MoveStatistic(PokemonTypes.Fighting, 30, 1));
            MoveStatsMap.Add(PokemonMoves.MegaKick, new MoveStatistic(PokemonTypes.Normal, 120, 0.75f));
            MoveStatsMap.Add(PokemonMoves.JumpKick, new MoveStatistic(PokemonTypes.Fighting, 100, .95f));
            MoveStatsMap.Add(PokemonMoves.RollingKick, new MoveStatistic(PokemonTypes.Fighting, 60, .85f));
            MoveStatsMap.Add(PokemonMoves.SandAttack, new MoveStatistic(PokemonTypes.Ground, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Headbutt, new MoveStatistic(PokemonTypes.Normal, 70, 1));
            MoveStatsMap.Add(PokemonMoves.HornAttack, new MoveStatistic(PokemonTypes.Normal, 65, 1));
            MoveStatsMap.Add(PokemonMoves.FuryAttack, new MoveStatistic(PokemonTypes.Normal, 15, .85f));
            MoveStatsMap.Add(PokemonMoves.HornDrill, new MoveStatistic(PokemonTypes.Normal, 0, 0));
            MoveStatsMap.Add(PokemonMoves.Tackle, new MoveStatistic(PokemonTypes.Normal, 50, 1));
            MoveStatsMap.Add(PokemonMoves.BodySlam, new MoveStatistic(PokemonTypes.Normal, 85, 1));
            MoveStatsMap.Add(PokemonMoves.Wrap, new MoveStatistic(PokemonTypes.Normal, 15, .9f));
            MoveStatsMap.Add(PokemonMoves.TakeDown, new MoveStatistic(PokemonTypes.Normal, 90, .85f));
            MoveStatsMap.Add(PokemonMoves.Thrash, new MoveStatistic(PokemonTypes.Normal, 120, 1));
            MoveStatsMap.Add(PokemonMoves.DoubleEdge, new MoveStatistic(PokemonTypes.Normal, 120, 1));
            MoveStatsMap.Add(PokemonMoves.TailWhip, new MoveStatistic(PokemonTypes.Normal, 0, 1));
            MoveStatsMap.Add(PokemonMoves.PoisonSting, new MoveStatistic(PokemonTypes.Poison, 15, 1));
            MoveStatsMap.Add(PokemonMoves.Twineedle, new MoveStatistic(PokemonTypes.Bug, 25, 1));
            MoveStatsMap.Add(PokemonMoves.PinMissile, new MoveStatistic(PokemonTypes.Bug, 25, .95f));
            MoveStatsMap.Add(PokemonMoves.Leer, new MoveStatistic(PokemonTypes.Normal, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Bite, new MoveStatistic(PokemonTypes.Normal, 60, 1));
            MoveStatsMap.Add(PokemonMoves.Growl, new MoveStatistic(PokemonTypes.Normal, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Roar, new MoveStatistic(PokemonTypes.Normal, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Sing, new MoveStatistic(PokemonTypes.Normal, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Supersonic, new MoveStatistic(PokemonTypes.Normal, 0, 1));
            MoveStatsMap.Add(PokemonMoves.SonicBoom, new MoveStatistic(PokemonTypes.Normal, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Disable, new MoveStatistic(PokemonTypes.Normal, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Acid, new MoveStatistic(PokemonTypes.Poison, 40, 1));
            MoveStatsMap.Add(PokemonMoves.Ember, new MoveStatistic(PokemonTypes.Fire, 40, 1));
            MoveStatsMap.Add(PokemonMoves.Flamethrower, new MoveStatistic(PokemonTypes.Fire, 90, 1));
            MoveStatsMap.Add(PokemonMoves.Mist, new MoveStatistic(PokemonTypes.Ice, 0, 1));
            MoveStatsMap.Add(PokemonMoves.WaterGun, new MoveStatistic(PokemonTypes.Water, 40, 1));
            MoveStatsMap.Add(PokemonMoves.HydroPump, new MoveStatistic(PokemonTypes.Water, 110, .8f));
            MoveStatsMap.Add(PokemonMoves.Surf, new MoveStatistic(PokemonTypes.Water, 90, 1));
            MoveStatsMap.Add(PokemonMoves.IceBeam, new MoveStatistic(PokemonTypes.Ice, 90, 1));
            MoveStatsMap.Add(PokemonMoves.Blizzard, new MoveStatistic(PokemonTypes.Ice, 110, .7f));
            MoveStatsMap.Add(PokemonMoves.Psybeam, new MoveStatistic(PokemonTypes.Psychic, 65, 1));
            MoveStatsMap.Add(PokemonMoves.BubbleBeam, new MoveStatistic(PokemonTypes.Water, 65, 1));
            MoveStatsMap.Add(PokemonMoves.AuroraBeam, new MoveStatistic(PokemonTypes.Ice, 65, 1));
            MoveStatsMap.Add(PokemonMoves.HyperBeam, new MoveStatistic(PokemonTypes.Normal, 150, 0.9f));
            MoveStatsMap.Add(PokemonMoves.Peck, new MoveStatistic(PokemonTypes.Flying, 35, 1));
            MoveStatsMap.Add(PokemonMoves.DrillPeck, new MoveStatistic(PokemonTypes.Flying, 80, 1));
            MoveStatsMap.Add(PokemonMoves.Submission, new MoveStatistic(PokemonTypes.Fighting, 80, 0.8f));
            MoveStatsMap.Add(PokemonMoves.LowKick, new MoveStatistic(PokemonTypes.Fighting, 0, 0.8f));
            MoveStatsMap.Add(PokemonMoves.Counter, new MoveStatistic(PokemonTypes.Fighting, 0, 0.8f));
            MoveStatsMap.Add(PokemonMoves.SeismicToss, new MoveStatistic(PokemonTypes.Fighting, 0, 0.8f));
            MoveStatsMap.Add(PokemonMoves.Strength, new MoveStatistic(PokemonTypes.Normal, 80, 1));
            MoveStatsMap.Add(PokemonMoves.Absorb, new MoveStatistic(PokemonTypes.Grass, 20, 1));
            MoveStatsMap.Add(PokemonMoves.MegaDrain, new MoveStatistic(PokemonTypes.Grass, 40, 1));
            MoveStatsMap.Add(PokemonMoves.LeechSeed, new MoveStatistic(PokemonTypes.Grass, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Growth, new MoveStatistic(PokemonTypes.Grass, 0, 1));
            MoveStatsMap.Add(PokemonMoves.RazorLeaf, new MoveStatistic(PokemonTypes.Grass, 55, .95f));
            MoveStatsMap.Add(PokemonMoves.SolarBeam, new MoveStatistic(PokemonTypes.Grass, 120, 1));
            MoveStatsMap.Add(PokemonMoves.PoisonPowder, new MoveStatistic(PokemonTypes.Grass, 0, 1));
            MoveStatsMap.Add(PokemonMoves.StunSpore, new MoveStatistic(PokemonTypes.Grass, 0, 1));
            MoveStatsMap.Add(PokemonMoves.SleepPowder, new MoveStatistic(PokemonTypes.Grass, 0, 1));
            MoveStatsMap.Add(PokemonMoves.PetalDance, new MoveStatistic(PokemonTypes.Grass, 0, 1));
            MoveStatsMap.Add(PokemonMoves.StringShot, new MoveStatistic(PokemonTypes.Grass, 0, 1));
            MoveStatsMap.Add(PokemonMoves.DragonRage, new MoveStatistic(PokemonTypes.Dragon, 40, 1));
            MoveStatsMap.Add(PokemonMoves.FireSpin, new MoveStatistic(PokemonTypes.Fire, 35, 1));
            MoveStatsMap.Add(PokemonMoves.ThunderShock, new MoveStatistic(PokemonTypes.Electric, 40, 1));
            MoveStatsMap.Add(PokemonMoves.Thunderbolt, new MoveStatistic(PokemonTypes.Electric, 90, 1));
            MoveStatsMap.Add(PokemonMoves.ThunderWave, new MoveStatistic(PokemonTypes.Electric, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Thunder, new MoveStatistic(PokemonTypes.Electric, 110, .7f));
            MoveStatsMap.Add(PokemonMoves.RockThrow, new MoveStatistic(PokemonTypes.Rock, 50, .9f));
            MoveStatsMap.Add(PokemonMoves.Earthquake, new MoveStatistic(PokemonTypes.Ground, 100, 1));
            MoveStatsMap.Add(PokemonMoves.Fissure, new MoveStatistic(PokemonTypes.Ground, 300, .8f));
            MoveStatsMap.Add(PokemonMoves.Dig, new MoveStatistic(PokemonTypes.Ground, 80, 1));
            MoveStatsMap.Add(PokemonMoves.Toxic, new MoveStatistic(PokemonTypes.Poison, 0, .8f));
            MoveStatsMap.Add(PokemonMoves.Confusion, new MoveStatistic(PokemonTypes.Psychic, 50, 1));
            MoveStatsMap.Add(PokemonMoves.Psychic, new MoveStatistic(PokemonTypes.Psychic, 90, 1));
            MoveStatsMap.Add(PokemonMoves.Hypnosis, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Meditate, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Agility, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.QuickAttack, new MoveStatistic(PokemonTypes.Normal,  40, 1));
            MoveStatsMap.Add(PokemonMoves.Rage, new MoveStatistic(PokemonTypes.Normal, 20, 1));
            MoveStatsMap.Add(PokemonMoves.Teleport, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.NightShade, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Mimic, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Screech, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.DoubleTeam, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Recover, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Harden, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Minimize, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Smokescreen, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.ConfuseRay, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Withdraw, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.DefenseCurl, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Barrier, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.LightScreen, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Haze, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Reflect, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.FocusEnergy, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Bide, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Metronome, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.MirrorMove, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.SelfDestruct, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.EggBomb, new MoveStatistic(PokemonTypes.Normal, 100, .75f));
            MoveStatsMap.Add(PokemonMoves.Lick, new MoveStatistic(PokemonTypes.Ghost, 30, 1));
            MoveStatsMap.Add(PokemonMoves.Smog, new MoveStatistic(PokemonTypes.Poison, 30, .7f));
            MoveStatsMap.Add(PokemonMoves.Sludge, new MoveStatistic(PokemonTypes.Poison, 65, 1));
            MoveStatsMap.Add(PokemonMoves.BoneClub, new MoveStatistic(PokemonTypes.Ground, 65, .85f));
            MoveStatsMap.Add(PokemonMoves.FireBlast, new MoveStatistic(PokemonTypes.Ground, 110, .85f));
            MoveStatsMap.Add(PokemonMoves.Waterfall, new MoveStatistic(PokemonTypes.Water, 80, 1));
            MoveStatsMap.Add(PokemonMoves.Clamp, new MoveStatistic(PokemonTypes.Water, 35, .85f));
            MoveStatsMap.Add(PokemonMoves.Swift, new MoveStatistic(PokemonTypes.Normal, 60, 1.05f));
            MoveStatsMap.Add(PokemonMoves.SkullBash, new MoveStatistic(PokemonTypes.Normal, 130, 1));
            MoveStatsMap.Add(PokemonMoves.SpikeCannon, new MoveStatistic(PokemonTypes.Normal, 20, 1));
            MoveStatsMap.Add(PokemonMoves.Constrict, new MoveStatistic(PokemonTypes.Normal, 10, 1));
            MoveStatsMap.Add(PokemonMoves.Amnesia, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Kinesis, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.SoftBoiled, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.HighJumpKick, new MoveStatistic(PokemonTypes.Fighting, 130, 1));
            MoveStatsMap.Add(PokemonMoves.Glare, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.DreamEater, new MoveStatistic(PokemonTypes.Psychic, 100, 1));
            MoveStatsMap.Add(PokemonMoves.PoisonGas, new MoveStatistic(PokemonTypes.Poison, 20, 1));
            MoveStatsMap.Add(PokemonMoves.Barrage, new MoveStatistic(PokemonTypes.Normal, 15, .85f));
            MoveStatsMap.Add(PokemonMoves.LeechLife, new MoveStatistic(PokemonTypes.Bug, 20, 1));
            MoveStatsMap.Add(PokemonMoves.LovelyKiss, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.SkyAttack, new MoveStatistic(PokemonTypes.Flying, 140, .9f));
            MoveStatsMap.Add(PokemonMoves.Transform, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Bubble, new MoveStatistic(PokemonTypes.Water, 40, 1));
            MoveStatsMap.Add(PokemonMoves.DizzyPunch, new MoveStatistic(PokemonTypes.Normal, 60, 1));
            MoveStatsMap.Add(PokemonMoves.Spore, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Flash, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Psywave, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Splash, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.AcidArmor, new MoveStatistic(PokemonTypes.Psychic, 0, 1));
            MoveStatsMap.Add(PokemonMoves.Crabhammer, new MoveStatistic(PokemonTypes.Water, 100, .9f));
            MoveStatsMap.Add(PokemonMoves.Explosion, new MoveStatistic(PokemonTypes.Normal, 80, 100));
            MoveStatsMap.Add(PokemonMoves.FurySwipes, new MoveStatistic(PokemonTypes.Normal, 18, .8f));
            MoveStatsMap.Add(PokemonMoves.Bonemerang, new MoveStatistic(PokemonTypes.Ground, 50, .9f));
            MoveStatsMap.Add(PokemonMoves.Rest, new MoveStatistic(PokemonTypes.Psychic, 80, 1));
            MoveStatsMap.Add(PokemonMoves.RockSlide, new MoveStatistic(PokemonTypes.Rock, 75, .9f));
            MoveStatsMap.Add(PokemonMoves.HyperFang, new MoveStatistic(PokemonTypes.Normal, 80, .9f));
            MoveStatsMap.Add(PokemonMoves.Sharpen, new MoveStatistic(PokemonTypes.Normal, 0, .9f));
            MoveStatsMap.Add(PokemonMoves.Conversion, new MoveStatistic(PokemonTypes.Normal, 0, .9f));
            MoveStatsMap.Add(PokemonMoves.TriAttack, new MoveStatistic(PokemonTypes.Normal, 80, 1));
            MoveStatsMap.Add(PokemonMoves.SuperFang, new MoveStatistic(PokemonTypes.Normal, 80, .9f));
            MoveStatsMap.Add(PokemonMoves.Slash, new MoveStatistic(PokemonTypes.Normal, 70, .9f));
            MoveStatsMap.Add(PokemonMoves.Substitute, new MoveStatistic(PokemonTypes.Normal, 0, .9f));
            MoveStatsMap.Add(PokemonMoves.Struggle, new MoveStatistic(PokemonTypes.Normal, 50, 1));

        }
    }


}

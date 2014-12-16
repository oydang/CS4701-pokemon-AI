using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Trainer
{
    public class Control
    {
        private BinaryReader bReader;
        private Maps utilMaps;
        const string RAM_FILENAME = "..\\..\\..\\..\\..\\..\\Tracer-VisualboyAdvance1.7.1\\Tracer-VisualboyAdvance1.7.1\\tracer\\Pokemon Red\\cgb_wram.bin";

        public Control()
        {
            utilMaps = new Maps();        
        }

        /// <summary>
        /// Run the algorithm here
        /// </summary>
        public void Run()
        {
            if (GetIsInBattle() != 0)
            {
                AITrainer.DumpRAM();
                int bestmove = calculateBestMove();
                Debug.WriteLine(bestmove);
                AITrainer.doMove(ActionTypes.Attack, bestmove);
            }

        }

        /// <summary>
        /// 0 if player is not currently in battle, otherwise is in battle!
        /// </summary>
        /// <returns></returns>
        public int GetIsInBattle()
        {
            int addr = utilMaps.StatAddressMap[GameStats.IsInBattle];
            int length = utilMaps.StatLengthMap[GameStats.IsInBattle];
            byte[] b = readBin(addr, length);
            return (int)b[0];
        }

        /// <summary>
        /// The level of my current pokemon in battle, read from cgb_wram.bin
        /// </summary>
        /// <returns></returns>
        public int GetMyPkmLevel()
        {
            int addr = utilMaps.StatAddressMap[GameStats.MyPkmLevel];
            int length = utilMaps.StatLengthMap[GameStats.MyPkmLevel];
            byte[] b = readBin(addr, length);
            return (int)b[0];
        }

        /// <summary>
        /// Returns the health of the current Pokemon in battle
        /// </summary>
        /// <returns></returns>
        public int GetMyPkmHealth()
        {
            int addr = utilMaps.StatAddressMap[GameStats.MyPkmHealth];
            int length = utilMaps.StatLengthMap[GameStats.MyPkmHealth];
            byte[] b = readBin(addr, length);
            return ((int)b[0]) + ((int) b[1]);
        }

        /// <summary>
        /// Returns the VIABLE moves that my Pokemon in battle has. All moves with PP=0 are not counted.
        /// </summary>
        /// <returns></returns>
        public Tuple<PokemonMoves,int>[] GetMyPkmMoves()
        {
            Tuple<PokemonMoves,int>[] moves = new Tuple<PokemonMoves,int>[4];
            byte[] bMoves = readBin(utilMaps.StatAddressMap[GameStats.MyPkmMoves], utilMaps.StatLengthMap[GameStats.MyPkmMoves]);
            for (int i = 0; i < bMoves.Length; i++)
            {
                int pp = readBin(i + 0x102D, 1)[0]; //The pp left for this move
                moves[i] = new Tuple<PokemonMoves,int>((PokemonMoves) bMoves[i],pp);
            }
            return moves;
        }

        /// <summary>
        /// Returns the first type of my current pokemon in battle. If both types are the same, the Pokemon has only one type.
        /// </summary>
        /// <returns></returns>
        public PokemonTypes GetMyPkmType1()
        {
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.MyPkmType1], utilMaps.StatLengthMap[GameStats.MyPkmType1]);
            int index = (int)b[0];
            //Weird indices for types... trying to map to [0, 14] but see http://bulbapedia.bulbagarden.net/wiki/Pok%C3%A9mon_data_structure_in_Generation_I
            if (index == 7 || index == 8)
            {
                index -= 1;
            }
            if (index >= 20)
            {
                index -= 12;
            }
            return (PokemonTypes)index;
        }

        /// <summary>
        /// Returns the second type of my current pokemon in battle. If its both types are the same, it has only one type.
        /// </summary>
        /// <returns></returns>
        public PokemonTypes GetMyPkmType2()
        {
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.MyPkmType2], utilMaps.StatLengthMap[GameStats.MyPkmType2]);
            int index = (int)b[0];
            //Weird indices for types... trying to map to [0, 14] but see http://bulbapedia.bulbagarden.net/wiki/Pok%C3%A9mon_data_structure_in_Generation_I
            if (index == 7 || index == 8)
            {
                index -= 1;
            }
            if (index >= 20)
            {
                index -= 12;
            }
            return (PokemonTypes)index;
        }

        /// <summary>
        /// Returns my current pokemon in battle's attack stat
        /// </summary>
        /// <returns></returns>
        public int GetMyPkmAttack()
        {
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.MyPkmAttack], utilMaps.StatLengthMap[GameStats.MyPkmAttack]);
            return (256 * (int)b[0]) + ((int)b[1]);
        }

        /// <summary>
        /// Returns my current pokemon in battle's special attack/defense stat
        /// </summary>
        /// <returns></returns>
        public int GetMyPkmSpecial()
        {
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.MyPkmSpecial], utilMaps.StatLengthMap[GameStats.MyPkmSpecial]);
            return (256 * (int)b[0]) + ((int)b[1]);
        }

        /// <summary>
        /// Returns the Opponent pokemon's current special attack/defense in a battle
        /// </summary>
        /// <returns></returns>
        public int GetOpponentDefense()
        {
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.OpponentDefense], utilMaps.StatLengthMap[GameStats.OpponentDefense]);
            return (256 * (int)b[0]) + ((int) b[1]);
        }

        public int GetOpponentSpecial()
        {
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.OpponentSpecial], utilMaps.StatLengthMap[GameStats.OpponentSpecial]);
            return (256 * (int)b[0]) + ((int)b[1]);
        }

        /// <summary>
        /// Returns the opponent's Pokemon's first type. If both types are the same it has only one type.
        /// </summary>
        /// <returns></returns>
        public PokemonTypes GetOpponentType1(){
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.OpponentType1], utilMaps.StatLengthMap[GameStats.OpponentType1]);
            int index = (int)b[0];
            //Weird indices for types... trying to map to [0, 14] but see http://bulbapedia.bulbagarden.net/wiki/Pok%C3%A9mon_data_structure_in_Generation_I
            if (index == 7 || index == 8)
            {
                index -= 1;
            }
            if (index >= 20)
            {
                index -= 12;
            }
            return (PokemonTypes)index;
        }

        /// <summary>
        /// Returns the opponent's Pokemon's second type. If both types are the same it has only one type.
        /// </summary>
        /// <returns></returns>
        public PokemonTypes GetOpponentType2()
        {
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.OpponentType2], utilMaps.StatLengthMap[GameStats.OpponentType2]);
            int index = (int)b[0];
            //Weird indices for types... trying to map to [0, 14] but see http://bulbapedia.bulbagarden.net/wiki/Pok%C3%A9mon_data_structure_in_Generation_I
            if (index == 7 || index == 8)
            {
                index -= 1;
            }
            if (index >= 20)
            {
                index -= 12;
            }
            return (PokemonTypes)index;
        }
        
        /// <summary>
        /// Gets the best move index that my current pokemon can do
        /// </summary>
        private int calculateBestMove()
        {
            int bestmove = -1;
            double bestattack = -1;
            int bestpp = -1;

            Tuple<PokemonMoves,int>[] moves = GetMyPkmMoves();
            for (int move = 0; move < 4; move++)
            {
                int pp = moves[move].Item2;
                if (pp > 0)
                {
                    double attack = calculateDamage(moves[move].Item1, true);
                    Debug.WriteLine(attack);
                    if (attack > bestattack || (attack == bestattack && pp > bestpp))
                    {
                        bestmove = move;
                        bestattack = attack;
                        bestpp = pp;
                    }
                }
            }
            return bestmove;
        }

        /// <summary>
        /// Caclulates the approx damage to opponent. Else needs to modified to include opponent stats, currently does nothing different.
        /// Equation ignoring last Z from: https://www.math.miami.edu/~jam/azure/compendium/battdam.htm
        /// </summary>
        /// 
        private double calculateDamage(PokemonMoves move, bool myPokemon)
        {
            double A,B,C,D,X,Y,Z;
            A = B = C = D = X = Y = Z = 1;

            if (myPokemon)
            {
                A = GetMyPkmAttack();
                if (IsSpecialMove(move))
                {
                    B = GetMyPkmSpecial();
                }
                else
                {
                    B = GetMyPkmAttack();
                }
                MoveStatistic ms = utilMaps.MoveStatsMap[move];
                C = ms.AttackPower;
                D = GetOpponentDefense();
                PokemonTypes myt1 = GetMyPkmType1();
                PokemonTypes myt2 = GetMyPkmType2();
                PokemonTypes opt1 = GetOpponentType1();
                PokemonTypes opt2 = GetOpponentType2();
                if (myt1 == ms.MoveType || myt2 == ms.MoveType)
                {
                    X = 1.5;
                }
                Y = utilMaps.TypeModiferChart[(int)ms.MoveType, (int)opt1];
                if (opt1 != opt2)
                {
                    Y *= utilMaps.TypeModiferChart[(int)ms.MoveType, (int)opt2];
                }
                Z = ms.Accuracy;
            }
            else
            {
                A = GetMyPkmLevel();
                B = GetMyPkmAttack();
                MoveStatistic ms = utilMaps.MoveStatsMap[move];
                C = ms.AttackPower;
                D = GetOpponentDefense();
                PokemonTypes myt1 = GetMyPkmType1();
                PokemonTypes myt2 = GetMyPkmType2();
                PokemonTypes opt1 = GetOpponentType1();
                PokemonTypes opt2 = GetOpponentType2();
                if (myt1 == ms.MoveType || myt2 == ms.MoveType)
                {
                    X = 1.5;
                }
                Y = utilMaps.TypeModiferChart[(int)ms.MoveType, (int)opt1];
                if (opt1 != opt2)
                {
                    Y *= utilMaps.TypeModiferChart[(int)ms.MoveType, (int)opt2];
                }
                Z = ms.Accuracy;
            }
            return ((((2 * A / 5 + 2) * B * C / D) / 50) + 2) * X * Y * Z;
        }

        public bool IsSpecialMove(PokemonMoves m)
        {
            MoveStatistic info = utilMaps.MoveStatsMap[m];
            return info.MoveType == PokemonTypes.Water ||
                    info.MoveType == PokemonTypes.Grass ||
                    info.MoveType == PokemonTypes.Ice ||
                    info.MoveType == PokemonTypes.Fire ||
                    info.MoveType == PokemonTypes.Electric ||
                    info.MoveType == PokemonTypes.Psychic ||
                    info.MoveType == PokemonTypes.Dragon;
        }

        /// <summary>
        /// Returns a the bytes read from cgb_wram.bin at the specified address and length.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] readBin(int address, int length)
        {
            this.bReader = new BinaryReader(File.Open(RAM_FILENAME, FileMode.Open));
            bReader.BaseStream.Position = address;
            //for (int i = address; i <= 0x115E + length; i++)
            //{
            //    charName += bReader.ReadByte().ToString("X2");
            byte[] b = bReader.ReadBytes(length);            
            bReader.Close();
            return b;
        }
    }
}

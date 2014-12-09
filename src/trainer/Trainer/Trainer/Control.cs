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
        private AITrainer trainer;
        private BinaryReader bReader;
        private Maps utilMaps;
        private Random generator; //For the equation we will use from https://www.math.miami.edu/~jam/azure/compendium/battdam.htm
        const string RAM_FILENAME = "C:\\Users\\Olivia\\Dropbox\\College\\Junior\\CS4700\\pokeBot\\Tracer-VisualboyAdvance1.7.1\\Tracer-VisualboyAdvance1.7.1\\tracer\\Pokemon Red\\cgb_wram.bin";


        public Control()
        {
            trainer = new AITrainer();
            utilMaps = new Maps();
            generator = new Random();
            
        }

        /// <summary>
        /// Run the algorithm here
        /// </summary>
        public void Run()
        {
            trainer.DumpRAM();
            Debug.WriteLine(GetMyPkmLevel());
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
        public PokemonMoves[] GetMyPkmMoves()
        {
            PokemonMoves[] moves = new PokemonMoves[4];
            byte[] bMoves = readBin(utilMaps.StatAddressMap[GameStats.MyPkmMoves], utilMaps.StatLengthMap[GameStats.MyPkmMoves]);
            for (int i = 0; i < bMoves.Length; i++)
            {
                int pp = readBin(i + 0x102D, 1)[0]; //The pp left for this move
                moves[i] = (PokemonMoves.None);
                if(pp > 0){
                    moves[i] = (PokemonMoves) bMoves[i];
                }
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
            return (PokemonTypes) (int) b[0];
        }

        /// <summary>
        /// Returns the second type of my current pokemon in battle. If its both types are the same, it has only one type.
        /// </summary>
        /// <returns></returns>
        public PokemonTypes GetMyPkmType2()
        {
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.MyPkmType2], utilMaps.StatLengthMap[GameStats.MyPkmType2]);
            return (PokemonTypes)(int)b[0];
          
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
        /// Returns the Opponent pokemon's current defensive stat in a battle
        /// </summary>
        /// <returns></returns>
        public int GetOpponentDefense()
        {
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.OpponentDefense], utilMaps.StatLengthMap[GameStats.OpponentDefense]);
            return (256 * (int)b[0]) + ((int) b[1]);
        }

        /// <summary>
        /// Returns the opponent's Pokemon's first type. If both types are the same it has only one type.
        /// </summary>
        /// <returns></returns>
        public PokemonTypes GetOpponentType1(){
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.OpponentType1], utilMaps.StatLengthMap[GameStats.OpponentType1]);
            return (PokemonTypes)(int)b[0];
        }

        /// <summary>
        /// Returns the opponent's Pokemon's second type. If both types are the same it has only one type.
        /// </summary>
        /// <returns></returns>
        public PokemonTypes GetOpponentType2()
        {
            byte[] b = readBin(utilMaps.StatAddressMap[GameStats.OpponentType2], utilMaps.StatLengthMap[GameStats.OpponentType2]);
            return (PokemonTypes)(int)b[0];
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

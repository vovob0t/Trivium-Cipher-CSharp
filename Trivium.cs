using System.Collections;
using System.Collections.Specialized;
using System.Numerics;
using System.Text;

namespace Trivium;

public class Trivium
{
    private BitArray keyBits;
    private BitArray IVBits;
    private BitArray internalState;

    public Trivium(string key, string IV)
    {
        this.stateInitialization(key, IV);
        return;
    }

    public void encryption(){

    }


    private void stateInitialization(string key, string IV)
    {

        List<bool> keyBits = this.hexToBitsInBool(key).ToList();
        List<bool> IVBits = this.hexToBitsInBool(IV).ToList();

        List<bool> allBitsState = new();

        allBitsState.AddRange(keyBits);
        allBitsState.AddRange(Enumerable.Repeat(false, 13));

        allBitsState.AddRange(IVBits);
        allBitsState.AddRange(Enumerable.Repeat(false, 4));

        allBitsState.AddRange(Enumerable.Repeat(false, 108));
        allBitsState.AddRange(Enumerable.Repeat(true, 3));

        this.internalState = new BitArray(allBitsState.ToArray());

        this.keyGeneration();
        return;
    }


    private List<bool> keyGeneration(int size = 4 * 288)
    {
        List<bool> z = new();
        bool t1, t2, t3;

        for (int i = 0; i < size; i++)
        {
            t1 = this.internalState[65] ^ this.internalState[92];

            t2 = this.internalState[161] ^ this.internalState[176];

            t3 = this.internalState[242] ^ this.internalState[287];

            z.Add(t1 ^ t2 ^ t3);

            t1 = t1 ^ (this.internalState[90] & this.internalState[91]) ^ this.internalState[170];

            t2 = t2 ^ (this.internalState[174] & this.internalState[175]) ^ this.internalState[263];

            t3 = t3 ^ (this.internalState[285] & this.internalState[286]) ^ this.internalState[68];

            this.internalState.LeftShift(1);

            this.internalState[0] = t1;
            this.internalState[93] = t2;
            this.internalState[177] = t3;
        }
        return z;
    }
    public bool[] hexToBitsInBool(string hexValue)
    {

        BitArray bitArray = new(
            (BigInteger.Parse(
            hexValue, System.Globalization.NumberStyles.HexNumber)
            ).ToByteArray()
            );

        bool[] bits = new bool[bitArray.Length];

        bitArray.CopyTo(bits, 0);
        Array.Reverse(bits);

        /* odd hexValue.Length
        this can help if there is unexpected 1's that bitarray create
        if the hexValue.Length is odd (нечетное)

        if (hexValue.Length % 2 != 0){
            bool[] newBits = new bool[bits.Length - 4];
            Array.Copy(bits, 4, newBits, 0, newBits.Length);
            return newBits;
         } else return bits; */

        return bits;
    }

    public bool[] stringToBitsInBool(string plainText)
    {
        Encoding enc = Encoding.UTF8;

        byte[] bytes = enc.GetBytes(plainText);

        Array.Reverse(bytes);

        BitArray bitArray = new(bytes);

        bool[] bits = new bool[bitArray.Length];

        bitArray.CopyTo(bits, 0);

        Array.Reverse(bits);

        return bits;
    }

    public void printBits(BitArray bitArray = null)
    {
        if (bitArray is null) bitArray = this.internalState;

        int breaker = 0;
        string buildBits = "";

        foreach (bool bite in bitArray)
        {
            if (breaker == 8)
            {
                Console.Write(" ");
                breaker = 0;
            }
            if (bite)
            {
                Console.Write(1);
                buildBits += "1";
            }
            else
            {
                Console.Write(0);
                buildBits += "0";
            }
            breaker++;
        }

    }
}

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

        internalState = stateInit(key, IV);
        return;
    }

    private BitArray stateInit(string key, string IV)
    {
        BitArray state = new BitArray(length: 288);


        bool[] keyBits = hexToBits(key);
        bool[] IVBits = hexToBits(IV);

        List<bool> allBitsState = new();

        allBitsState.AddRange(keyBits.ToList());
        allBitsState.AddRange(Enumerable.Repeat(false, 13));

        allBitsState.AddRange(IVBits.ToList());
        allBitsState.AddRange(Enumerable.Repeat(false, 4));

        allBitsState.AddRange(Enumerable.Repeat(false, 108));
        allBitsState.AddRange(Enumerable.Repeat(true, 3));


        return new BitArray(allBitsState.ToArray());
    }

    static public bool[] hexToBits(string hexValue)
    {
        BigInteger decimalValue = BigInteger.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);

        // Encoding enc = Encoding.ASCII;

        // BitArray bitArray = new(Encoding.ASCII.GetBytes(hexValue));

        BitArray bitArray = new(decimalValue.ToByteArray());

        bool[] bits = new bool[bitArray.Length];

        bitArray.CopyTo(bits, 0);
        Array.Reverse(bits);

        //        bitArray = new BitArray(bits);


        // int breaker = 0;
        // string buildBits = "";

        // foreach (bool bite in bitArray)
        // {
        //     if (breaker == 4)
        //     {
        //         Console.Write(" ");
        //         breaker = 0;
        //     }
        //     if (bite){
        //         Console.Write(1);
        //         buildBits+="1";
        //     }
        //     else {
        //         Console.Write(0);
        //          buildBits+="0";
        //     }
        //     breaker++;
        // }

        return bits;
    }
}

using System;
using System.Collections;
using System.Numerics;
using System.Security.Cryptography;
using Nito.Collections;

namespace Trivium;


/*
use right shift for moving bits in array
*/
class Program
{
    static void Main(string[] args)
    {
        string hexValue = "CB4579AD1CA63FB5F2AA";
        Trivium.hexToBits(hexValue);
        var test = new Deque<int>();
        Trivium teswt = new("CB4579AD1CA63FB5F2AA", "CB4520FC1CA63F830D3A");



       /* BigInteger decimalValue = BigInteger.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);

        List<byte> bytes = (decimalValue.ToByteArray()).ToList();

        BigInteger tes = 1;
        BitArray bydtes = new(tes.ToByteArray());
*/
        /* Convert all (key, IV and others values like 0s and 1s) to bytes array,
        piece them together and convert all to a BitArray */
        test.AddToFront(5);
        test.AddToFront(6);
        test.AddToBack(7);

        foreach (var item in test){
            Console.WriteLine(item);
        }
        Console.ReadLine();
    }
}

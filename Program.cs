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
        string hexValue = "ABC";


        Trivium teswt = new("CB4579AD1CA63FB5F2AA", "CB4520FC1CA63F830D3A");

        //teswt.printBits();
// false  true true false true
        var strBits = teswt.stringToBitsInBool("Hello");

        teswt.printBits(new BitArray (strBits));

       /* BigInteger decimalValue = BigInteger.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);

        List<byte> bytes = (decimalValue.ToByteArray()).ToList();

        BigInteger tes = 1;
        BitArray bydtes = new(tes.ToByteArray());
*/
        /* Convert all (key, IV and others values like 0s and 1s) to bytes array,
        piece them together and convert all to a BitArray */


        Console.ReadLine();
    }
}

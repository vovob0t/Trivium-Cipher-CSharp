using System;
using System.Collections;
using System.Numerics;
using System.Security.Cryptography;

namespace Trivium;

class Program
{
    static void Main(string[] args)
    {
        Program.program();
        Console.ReadLine();
    }

    static void program()
    {
        Console.Write("Welcome to a Trivium cipher program!\n\n" +
        "Please, enter text you want to encrypt: ");
        string plainText = Console.ReadLine();


        Console.Write("\nNow, enter your hexadecimal key (20 hex) or\n" +
                    "use example key - A1B2C3D4E5F6A1B2C3D4 : ");
        string key = Console.ReadLine();


        Console.Write("\nNow, enter your hexadecimal IV (20 hex) or\n" +
                    "use example IV - 4A8F372D1B96C0A3E5D7 : ");
        string IV = Console.ReadLine();


        Trivium trivium = new();

        bool[] encryptedText = trivium.encryption(key, IV, plainText);

        Console.WriteLine("\nYour encrypted message:\n"
        + $"{trivium.bitsToHex(encryptedText)}\n");

        string decryptedText = trivium.bitsToString(
            trivium.decryption(key, IV, encryptedText)
        );

        Console.WriteLine($"\nYour decrypted message:\n" +
                        $"{decryptedText}\n");
    }
}

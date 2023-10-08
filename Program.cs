using System.Numerics;

namespace Trivium;

class Program
{
    static void Main(string[] args)
    {
        Program.ProgramMain();
        Console.ReadLine();
    }

    static void ProgramMain()
    {
        string plainText = "", key = "", IV = "";

        Program.ValuesInitialize(ref plainText, ref key, ref IV);

        Program.PrintValues(plainText, key, IV);

        Trivium trivium = new();

        bool[] encryptedText = trivium.Encryption(key, IV, plainText);

        Console.WriteLine("\nYour encrypted message:\n"
        + $"{trivium.BitsToHex(encryptedText)}");

        string decryptedText = trivium.BitsToString(
            trivium.Decryption(key, IV, encryptedText)
        );

        Console.WriteLine($"\nYour decrypted message:\n" +
                        $"{decryptedText}\n");
    }

    static public void PrintValues(string plainText, string key, string IV){
        Console.WriteLine($"Text: {plainText}\n");

        Console.WriteLine($"Key:  {key}");

        Console.WriteLine($"IV:   {IV}\n");
    }

    static public void ValuesInitialize(ref string plainText, ref string key, ref string IV)
    {
        Console.Write("Welcome to a Trivium cipher program!\n\n" +
        "Please, enter text you want to encrypt: ");
        plainText = Console.ReadLine();


        Console.Write("\nNow, enter your hexadecimal key (20 hex) or\n" +
                    "use example key - A1B2C3D4E5F6A1B2C3D4 : "
        );
        Program.InputHexValues(ref key);


        Console.Write("\nNow, enter your hexadecimal IV (20 hex) or\n" +
                    "use example IV - 4A8F372D1B96C0A3E5D7 : "
        );
        Program.InputHexValues(ref IV);
    }

    static public void InputHexValues(ref string value)
    {
        while (true)
        {
            value = Console.ReadLine();
            BigInteger outParam;

            if (value.Length != 20)
            {
                Console.Write("Invalid input: Length should be 20 characters!\nTry again: ");
                continue; //invalid input, loop restarts
            }
            else if (!BigInteger.TryParse(value, System.Globalization.NumberStyles.HexNumber, default, out outParam))
            {
                Console.Write("Invalid input: Given hex is not in right format!\nTry again: ");
                continue; //invalid input, loop restarts
            }

            //input is valid, break loop
            Console.Clear();
            break;
        }
    }
}

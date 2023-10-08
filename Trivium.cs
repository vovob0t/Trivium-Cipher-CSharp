using System.Collections;
using System.Collections.Specialized;
using System.Numerics;
using System.Text;

namespace Trivium;

public class Trivium
{
    private BitArray internalState = null;

    public bool[] decryption(string key, string IV, bool[] cipherText)
    {
        this.stateInitialization(key, IV);

        List<bool> keyStream = this.keyStreamGeneration(cipherText.Length);

        List<bool> plainText = new();

        for (int i = 0; i < cipherText.Length; i++)
        {
            plainText.Add(
                cipherText[i] ^ keyStream[i]
            );
        }

        this.internalState = null;

        return plainText.ToArray();
    }

    public bool[] encryption(string key, string IV, string plainText)
    {
        bool[] textInBits = this.stringToBits(plainText);

        this.stateInitialization(key, IV);

        List<bool> keyStream = this.keyStreamGeneration(textInBits.Length);
        List<bool> encryptedText = new();

        for (int i = 0; i < textInBits.Length; i++)
        {
            encryptedText.Add(
                textInBits[i] ^ keyStream[i]
            );
        }

        this.internalState = null;

        return encryptedText.ToArray();
    }

    private void stateInitialization(string key, string IV)
    {
        List<bool> keyBits = this.hexToBits(key).ToList();
        List<bool> IVBits = this.hexToBits(IV).ToList();

        List<bool> allBitsState = new();

        allBitsState.AddRange(keyBits);
        allBitsState.AddRange(Enumerable.Repeat(false, 13));

        allBitsState.AddRange(IVBits);
        allBitsState.AddRange(Enumerable.Repeat(false, 4));

        allBitsState.AddRange(Enumerable.Repeat(false, 108));
        allBitsState.AddRange(Enumerable.Repeat(true, 3));

        this.internalState = new BitArray(allBitsState.ToArray());

        this.keyStreamGeneration();
        return;
    }

    private List<bool> keyStreamGeneration(int size = 4 * 288)
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

    public bool[] hexToBits(string hexValue)
    {
        //helps to save 0's from being deleted when getting bytes from BigInteger
        hexValue = "1" + hexValue;

        List<byte> bytes =
        BigInteger.Parse(hexValue, System.Globalization.NumberStyles.HexNumber)
        .ToByteArray()
        .ToList();

        bytes.RemoveAt(bytes.Count - 1);

        BitArray bitArray = new(bytes.ToArray());
        bool[] bits = new bool[bitArray.Length];

        bitArray.CopyTo(bits, 0);

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

    public bool[] stringToBits(string plainText)
    {
        Encoding enc = Encoding.UTF8;
        byte[] bytes = enc.GetBytes(plainText);

        BitArray bitArray = new(bytes);

        bool[] bits = new bool[bitArray.Length];
        bitArray.CopyTo(bits, 0);

        return bits;
    }

    public string bitsToString(bool[] textInBits)
    {
        BitArray bitArray = new(textInBits);
        byte[] bytes = new byte[(bitArray.Length + 7) / 8];

        bitArray.CopyTo(bytes, 0);

        return Encoding.UTF8.GetString(bytes);
    }

    public string bitsToHex(bool[] bits)
    {
        //we need to do all this reverses cause bitarray is messing with little/big endians
        Array.Reverse(bits);
        BitArray bitArray = new BitArray(bits);

        // equation for getting a needed number of bytes
        byte[] bytes = new byte[(bitArray.Length + 7) / 8];

        bitArray.CopyTo(bytes, 0);
        Array.Reverse(bytes);

        //this reverse to make initial bits array come back to normal
        Array.Reverse(bits);
        return BitConverter.ToString(bytes).Replace("-", "");
    }

    //method, if you want to print bits to check the between results
    public string printBits(BitArray bitArray = null)
    {
        if (bitArray is null) bitArray = this.internalState;

        string buildBits = "";
        foreach (bool bite in bitArray)
        {
            if (bite)
            {
                buildBits += "1";
            }
            else
            {
                buildBits += "0";
            }
        }
        return buildBits;
    }
}

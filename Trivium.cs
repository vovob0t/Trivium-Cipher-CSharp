using System.Collections;
using System.Numerics;
using System.Text;

namespace Trivium;

public class Trivium
{
    private BitArray _internalState = null;

    public bool[] Decryption(string key, string IV, bool[] cipherText)
    {
        this._StateInitialization(key, IV);

        List<bool> keyStream = this._KeyStreamGeneration(cipherText.Length);

        List<bool> plainText = new();

        for (int i = 0; i < cipherText.Length; i++)
        {
            plainText.Add(
                cipherText[i] ^ keyStream[i]
            );
        }

        this._internalState = null;

        return plainText.ToArray();
    }

    public bool[] Encryption(string key, string IV, string plainText)
    {
        bool[] textInBits = this.StringToBits(plainText);

        this._StateInitialization(key, IV);

        List<bool> keyStream = this._KeyStreamGeneration(textInBits.Length);
        List<bool> encryptedText = new();

        for (int i = 0; i < textInBits.Length; i++)
        {
            encryptedText.Add(
                textInBits[i] ^ keyStream[i]
            );
        }

        this._internalState = null;

        return encryptedText.ToArray();
    }

    private void _StateInitialization(string key, string IV)
    {
        List<bool> keyBits = this.HexToBits(key).ToList();
        List<bool> IVBits = this.HexToBits(IV).ToList();

        List<bool> allBitsState = new();

        allBitsState.AddRange(keyBits);
        allBitsState.AddRange(Enumerable.Repeat(false, 13));

        allBitsState.AddRange(IVBits);
        allBitsState.AddRange(Enumerable.Repeat(false, 4));

        allBitsState.AddRange(Enumerable.Repeat(false, 108));
        allBitsState.AddRange(Enumerable.Repeat(true, 3));

        this._internalState = new BitArray(allBitsState.ToArray());

        this._KeyStreamGeneration();
        return;
    }

    private List<bool> _KeyStreamGeneration(int size = 4 * 288)
    {
        List<bool> z = new();
        bool t1, t2, t3;

        for (int i = 0; i < size; i++)
        {
            t1 = this._internalState[65] ^ this._internalState[92];

            t2 = this._internalState[161] ^ this._internalState[176];

            t3 = this._internalState[242] ^ this._internalState[287];

            z.Add(t1 ^ t2 ^ t3);

            t1 = t1 ^ (this._internalState[90] & this._internalState[91]) ^ this._internalState[170];

            t2 = t2 ^ (this._internalState[174] & this._internalState[175]) ^ this._internalState[263];

            t3 = t3 ^ (this._internalState[285] & this._internalState[286]) ^ this._internalState[68];

            this._internalState.LeftShift(1);

            this._internalState[0] = t1;
            this._internalState[93] = t2;
            this._internalState[177] = t3;
        }
        return z;
    }

    public bool[] HexToBits(string hexValue)
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

        return bits;
    }

    public bool[] StringToBits(string plainText)
    {
        Encoding enc = Encoding.UTF8;
        byte[] bytes = enc.GetBytes(plainText);

        BitArray bitArray = new(bytes);

        bool[] bits = new bool[bitArray.Length];
        bitArray.CopyTo(bits, 0);

        return bits;
    }

    public string BitsToString(bool[] textInBits)
    {
        BitArray bitArray = new(textInBits);
        byte[] bytes = new byte[(bitArray.Length + 7) / 8];

        bitArray.CopyTo(bytes, 0);

        return Encoding.UTF8.GetString(bytes);
    }

    public string BitsToHex(bool[] bits)
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
    public string PrintBits(BitArray bitArray = null)
    {
        if (bitArray is null) bitArray = this._internalState;

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

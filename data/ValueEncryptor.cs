using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Runtime.InteropServices;

[System.Serializable]
public class ValueEncryptor : Singleton<ValueEncryptor>
{
    // 암호화의 이해
    // 데이터 암호화 >> SHA(키) : SHA(값)+AES(값)
    // 봉인 >> SHA(값)+AES(값)
    private byte[] _keys;
    private byte[] _iv;
    private byte[] _keysLocal;
    private byte[] _ivLocal;
    private int keySize = 256;
    private int blockSize = 128;
    byte[] bytesTemp;
    string hashTemp;
    string valueTemp;
    string a = "-", b = "", c = "_";
    private StringBuilder sb;
    private StringBuilder sb2;
    private StringBuilder sb3;
    RijndaelManaged aes;
    RijndaelManaged aesLocal;
    SHA1CryptoServiceProvider sha;

    private int kiss; //키임
    private int tKiss; //키임
    private string sKiss; //키임
    private sFloat sf;

    [StructLayout(LayoutKind.Explicit)]
    private struct sFloat
    {
        [FieldOffset(0)]
        public float f;

        [FieldOffset(0)]
        public int i;
    }
    /// <summary>
    /// string 봉인 함수
    /// </summary>
    public string Seal(string D5XC8X5A)
    {
        //널체크
        if (string.IsNullOrEmpty(D5XC8X5A)) return "";

        int kl = sKiss.Length;
        int vl = D5XC8X5A.Length;
        char[] result = new char[vl];

        for (int i = 0; i < vl; i++)
        {
            //데이터 변조.
            result[i] = (char)(D5XC8X5A[i] ^ sKiss[i % kl]);
        }

        sb3.Length = 0;
        //변수 마크
        sb3.Append('S');
        //허수 삽입
        sb3.Append(UnityEngine.Random.Range(1000, 9999));
        sb3.Append(result); //더미 값
        return sb3.ToString();
    }

    /// <summary>
    /// bool 봉인 함수, 봉인된 결과는 string
    /// </summary>
    public string Seal(bool D5XC8X5A)
    {

        int encryptedValue = D5XC8X5A ? tKiss : UnityEngine.Random.Range(1000, 9999);
        sb3.Length = 0;
        sb3.Append('B');
        sb3.Append(UnityEngine.Random.Range(1000, 9999)); //더미 값
        sb3.Append(encryptedValue);
        return sb3.ToString();
    }
    /// <summary>
    /// float 봉인 함수, 봉인된 결과는 string
    /// </summary>
    public string Seal(float D5XC8X5A)
    {
        sf.f = D5XC8X5A;
        sb3.Length = 0;
        sb3.Append('F');
        sb3.Append(UnityEngine.Random.Range(1000, 9999)); //더미 값
        sb3.Append(sf.i ^ kiss);
        return sb3.ToString();
    }
    /// <summary>
    /// int 봉인 함수, 봉인된 결과는 string
    /// </summary>
    public string Seal(int D5XC8X5A)
    {
        sb3.Length = 0;
        sb3.Append('I');
        sb3.Append(UnityEngine.Random.Range(1000, 9999)); //더미 값
        sb3.Append(D5XC8X5A ^ kiss);
        return sb3.ToString();
    }

    /// <summary>
    /// 변수 봉인을 위한 지역 변수 초기화 및 복호화 지역 변수 초기화 호출
    /// </summary>
    public void LocalInitialization()
    {
        kiss = UnityEngine.Random.Range(100000000, 999999999);
        sKiss = kiss.ToString();
        tKiss = UnityEngine.Random.Range(1000, 9999);
        ValueDecryptor.Instance.SetLoaclInitialization(kiss, tKiss);
    }

    /// <summary>
    /// 세이브를 위한 rsa 및 sga 함수 초기화 및 복호화 클래스의 세이브 부분 초기화
    /// </summary>
    public void Initialization()
    {
        sha = new SHA1CryptoServiceProvider();
        sb = new StringBuilder();
        sb3 = new StringBuilder();
        aes = new RijndaelManaged();
        byte[] saltBytes = new byte[] { 14, 85, 24, 36, 63, 45, 80, 46 };
        string randomSeedForValue = "2cd53117aa87ca85";

        {
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(randomSeedForValue + "b9532a58c7a84e16", saltBytes, 1000);
            _keys = key.GetBytes(keySize / 8);
            _iv = key.GetBytes(blockSize / 8);
        }

        aes.KeySize = keySize;
        aes.BlockSize = blockSize;

        aes.Key = _keys;
        aes.IV = _iv;

        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        ValueDecryptor.Instance.Initialization(aes);
    }

    public string EncryptKey(string key)
    {
        
        return 'K' + GetHash(key + "HI"); //HI는 계정 이름으로 대체한다.
    }
    public string EncryptValue(string value)
    {

        return 'V' + GetHash(value) + GetEncrypt(value);
        //40글자 + 암호화값.
    }
    /// <summary>
    /// sha 해시 값
    /// </summary>
    public string GetHash(string original)
    {
        //string >> bytes[]
        bytesTemp = sha.ComputeHash(Encoding.UTF8.GetBytes(original));
        //byte[] > string
        return BitConverter.ToString(bytesTemp).Replace(a, b);
    }

    /// <summary>
    /// aes 암호화
    /// </summary>
    public byte[] GetEncrypt(byte[] bytesToBeEncrypted)
    {
        using (ICryptoTransform ct = aes.CreateEncryptor())
        {
            return ct.TransformFinalBlock(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
        }
    }
    public string GetEncrypt(string input)
    {
        bytesTemp = Encoding.UTF8.GetBytes(input);
        bytesTemp = GetEncrypt(bytesTemp);
        return TrasnformToString(bytesTemp);
    }

    string TrasnformToString(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace(a, c);
    }
}

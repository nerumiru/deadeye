using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;
using Boo.Lang;
using System.Runtime.InteropServices;

[System.Serializable]
public class ValueDecryptor : Singleton<ValueDecryptor>
{

    RijndaelManaged aes;
    RijndaelManaged aesLocal;
    byte[] bytesTemp;
    string valueTemp;
    private StringBuilder sb;
    List<byte> arrayTemp;
    //값 = temp.Remove(0, 40);
    //해시 = temp.Remove(40);

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
    /// 복호화를 위한 rsa 정보 초기화
    /// </summary>
    public void Initialization(RijndaelManaged temp)
    {
        aes = temp;
        arrayTemp = new List<byte>();

    }
    /// <summary>
    /// 변수 봉인 해제를 위한 복호화 변수 초기화 함수
    /// </summary>
    public void SetLoaclInitialization(int kiss_, int tKiss_)
    {
        kiss = kiss_; 
        tKiss = tKiss_; 
        sKiss = kiss.ToString();
    }
    /// <summary>
    /// string 봉인 해제
    /// </summary>
    public string Open(string SD52C5Q)
    {
        //널 체크
        if (string.IsNullOrEmpty(SD52C5Q))  return "";
        //변수 종류 체크
        if (SD52C5Q.Remove(1) == "S") return "";
        //허수 제거
        SD52C5Q = SD52C5Q.Remove(0, 5);       

        int kl = sKiss.Length;
        int vl = SD52C5Q.Length;
        char[] result = new char[vl];

        for (int i = 0; i < vl; i++)
        {
            result[i] = (char)(SD52C5Q[i] ^ sKiss[i % kl]);
        }

        return new string(result);
    }
    /// <summary>
    /// 봉인된 bool을 해제한다.
    /// </summary>
    public bool OpenB(string SD52C5Q)
    {
        //널 체크
        if (string.IsNullOrEmpty(SD52C5Q)) return false;
        //변수 종류 체크
        if (SD52C5Q.Remove(1) == "B") return false;

        int value = int.Parse(SD52C5Q.Remove(1));
        SD52C5Q = SD52C5Q.Remove(0, 5);
        
        if (int.Parse(SD52C5Q) == tKiss) return true;
        else return false;
    }
    /// <summary>
    /// 봉인된 float을 해제한다.
    /// </summary>
    public float OpenF(string SD52C5Q)
    {
        //널 체크
        if (string.IsNullOrEmpty(SD52C5Q)) return 0f;
        //변수 종류 체크
        if (SD52C5Q.Remove(1) == "F") return 0f;

        int value = int.Parse(SD52C5Q.Remove(1));
        SD52C5Q = SD52C5Q.Remove(0, 5);
        
        sf.i = int.Parse(SD52C5Q) ^ kiss;
        return sf.f;
    }
    /// <summary>
    /// 봉인된 int을 해제한다.
    /// </summary>
    public int OpenI(string SD52C5Q)
    {
        //널 체크
        if (string.IsNullOrEmpty(SD52C5Q)) return 0;
        //변수 종류 체크
        if (SD52C5Q.Remove(1) == "I") return 0;


        int value = int.Parse(SD52C5Q.Remove(1));
        SD52C5Q = SD52C5Q.Remove(0, 5);
        
        return int.Parse(SD52C5Q) ^ kiss;
    }
    
    /// <summary>
    /// 키값 분석 및 복호화하여 반화하는 함수
    /// </summary>
    public string DecryptValue(string value)
    {
        //앞40글자와 뒤의 해독된 해쉬값이 같아야 한다.
        //1분해
        value = value.Remove(0,1);  //마크    
        valueTemp = value.Remove(40);   //해쉬
        value = GetDecrypt(value.Remove(0, 40)); //내용
        //3변조 확인
        if (valueTemp == ValueEncryptor.Instance.GetHash(value))
            return value;
        else
            return "polluted";
    }
    /// <summary>
    /// 복호화 함수 : stirng to stirng
    /// </summary>
    public string GetDecrypt(string input)
    {
        //바이트로 변환
        bytesTemp = RestoreToBytes(input);
        //복호화
        using (ICryptoTransform ct = aes.CreateDecryptor())
        {
            bytesTemp = ct.TransformFinalBlock(bytesTemp, 0, bytesTemp.Length);
        }
        return Encoding.UTF8.GetString(bytesTemp);
    }
    /// <summary>
    /// 복호화 함수2 : byte to byte
    /// </summary>
    public byte[] GetDecrypt(byte[] input)
    {
        ICryptoTransform decrypt = aes.CreateDecryptor();
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
            {
                cs.Write(input, 0, input.Length);
            }

            input = ms.ToArray();
        }

        //String Output = GetString(xBuff);
        return input;
    }
    /// <summary>
    /// string을 byte로 바꾸는 함수
    /// </summary>
    byte[] RestoreToBytes(string str)
    {
        char[] temp = str.ToCharArray();
        int temp2 = 0, temp3 = 0;
        byte[] result;
        Array.ForEach(temp, x =>
        {
            if ((x == '_'))
            {
                arrayTemp.Add((byte)temp2);
                temp2 = 0;
            }
            else
            {
                temp3 = hexToint(x);
                temp2 = temp2 == 0 ? temp3 : temp2 * 16 + temp3;
            }
        });
        arrayTemp.Add((byte)temp2);

        result = arrayTemp.ToArray();
        arrayTemp.Clear();

        return result;
    }
    int hexToint(char ch)
    {
        if (ch >= '0' && ch <= '9')
            return ch - '0';
        if (ch >= 'A' && ch <= 'F')
            return ch - 'A' + 10;
        if (ch >= 'a' && ch <= 'f')
            return ch - 'a' + 10;
        return -1;
    }
}

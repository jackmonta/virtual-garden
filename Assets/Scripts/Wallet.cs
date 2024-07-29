using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Wallet
{
    public int Money { get; private set; }

    public Wallet()
    {
        Money = 0;
    }

    public void Save(string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(fileStream, this);
        }
    }

    public static Wallet Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                return (Wallet) formatter.Deserialize(fileStream);
            }
        }
        else
        {
            return new Wallet();
        }
    }
}
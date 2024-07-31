using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerManager player) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.kek";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerStuffs data = new PlayerStuffs(player); 

        formatter.Serialize(stream, data);

        Debug.Log("Saved Game!");
        stream.Close();
    }

    public static PlayerStuffs LoadPlayer() {
        string path = Application.persistentDataPath + "/player.kek";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerStuffs data = formatter.Deserialize(stream) as PlayerStuffs;
            stream.Close();

            Debug.Log("Loaded Save File!");
            return data;
        }
        else {
            return null;
        }
    }
}

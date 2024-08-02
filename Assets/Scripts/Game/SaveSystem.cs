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

        Debug.Log("Saved Player!");
        stream.Close();
    }

    public static void SaveCompanion(Character companion, int index) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + $"/companion{index}.kek";
        FileStream stream = new FileStream(path, FileMode.Create);

        CompanionStuffs data = new CompanionStuffs(companion); 

        formatter.Serialize(stream, data);

        Debug.Log("Saved Companions!");
        stream.Close();
    }

    public static void SavePlayerSkills(Skill skill, int index) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + $"/playerskill{index}.kek";
        FileStream stream = new FileStream(path, FileMode.Create);

        SkillStuffs data = new SkillStuffs(skill); 

        formatter.Serialize(stream, data);

        Debug.Log("Saved Player Skills!");
        stream.Close();
    }

    public static void SaveCompanionSkills(Skill skill, int compIndex, int skillIndex) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + $"/companion{compIndex}_skill{skillIndex}.kek";
        FileStream stream = new FileStream(path, FileMode.Create);

        SkillStuffs data = new SkillStuffs(skill); 

        formatter.Serialize(stream, data);

        Debug.Log("Saved Player Skills!");
        stream.Close();
    }

    public static PlayerStuffs LoadPlayer() {
        string path = Application.persistentDataPath + "/player.kek";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerStuffs data = formatter.Deserialize(stream) as PlayerStuffs;
            stream.Close();

            Debug.Log("Loaded Player File!");
            return data;
        }
        else {
            return null;
        }
    }

    public static SkillStuffs LoadPlayerSkills(int index) {
        string path = Application.persistentDataPath + $"/playerskill{index}.kek";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SkillStuffs data = formatter.Deserialize(stream) as SkillStuffs;
            stream.Close();

            Debug.Log("Loaded Player File!");
            return data;
        }
        else return null;
    }

    public static CompanionStuffs LoadCompanions(int index) {
        string path = Application.persistentDataPath + $"/companion{index}.kek";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CompanionStuffs data = formatter.Deserialize(stream) as CompanionStuffs;
            stream.Close();

            Debug.Log($"Loaded Companion{index}!");
            return data;
        }
        else return null;
    }

    public static SkillStuffs LoadCompanionSkills(int compIndex, int index) {
        string path = Application.persistentDataPath + $"/companion{compIndex}_skill{index}.kek";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SkillStuffs data = formatter.Deserialize(stream) as SkillStuffs;
            stream.Close();

            Debug.Log($"Loaded Companion{index}!");
            return data;
        }
        else return null;
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
class ApplicationManager
{
    #region Singleton
    private static ApplicationManager instance;

    public static ref ApplicationManager GetInstance()
    {
        if (instance == null)
            instance = new ApplicationManager();

        return ref instance;
    }
    public static void DestroyInstance()
    {
        if (instance != null)
            instance.Destroy();

        instance = null;
    }

    #endregion

    private const string filename = "Settings.txt";

    public ApplicationManager()
    {
        LoadFromSettings();
    }

    private void LoadFromSettings()
    {
        if (!File.Exists(filename))
            File.Create(filename);

        FileStream fs = File.OpenRead(filename);
        StreamReader sr = new StreamReader(fs);

        string line = sr.ReadLine(); //[score/int],[MasterVol/float],[SEVol/float],[BGVol/float]

        //Default values
        float tempScore = 0;
        float tempMasterVol = 1.0f;
        float tempSEVol = 1.0f;
        float tempBGVol = 1.0f;

        if (line != null)
        {
            string[] data = line.Split(',');

            //Note: These are precausions iff the user edits the 'settings.txt' file.
            //  by prevent any unexpected crashes caused by the settings.txt file being 
            //  'uncompatable'.

            if (data.Length >= 1) //Only if data exists
                float.TryParse(data[0], out tempScore);
            if (data.Length >= 2) //Only if data exists
                float.TryParse(data[1], out tempMasterVol);
            if (data.Length >= 3) //Only if data exists
                float.TryParse(data[2], out tempSEVol);
            if (data.Length >= 4) //Only if data exists
                float.TryParse(data[3], out tempBGVol);
        }

        GameManager.HighScore = tempScore;
        GameManager.MasterVolume = tempMasterVol;
        GameManager.SoundEffectVolume = tempSEVol;
        GameManager.BackGroundVolume = tempBGVol;

        sr.Close();
        fs.Close();
    }

    public void Destroy()
    {
        FileStream fs = File.OpenWrite(filename);
        StreamWriter sw = new StreamWriter(fs);

        float tempScore = GameManager.HighScore;
        float tempMasterVol = GameManager.MasterVolume;
        float tempSEVol = GameManager.SoundEffectVolume;
        float tempBGVol = GameManager.BackGroundVolume;

        sw.WriteLine($"{tempScore},{Mathf.Clamp(tempMasterVol, 0f, 1f)},{Mathf.Clamp(tempSEVol, 0f, 1f)},{Mathf.Clamp(tempBGVol, 0f, 1f)}");
        sw.Close();
        fs.Close();
    }
}

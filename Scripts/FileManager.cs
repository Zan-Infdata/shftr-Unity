using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class FileManager{

    public const string DOWN_PATH = @"./Assets/Shapeshifter/";

    public static bool CheckIfModelExists(string file){
        string fp = DOWN_PATH+file;
        return File.Exists(fp);
    }

    public static void SaveModel(string file, byte[] data){
        File.WriteAllBytes(DOWN_PATH+file, data);
    }




}

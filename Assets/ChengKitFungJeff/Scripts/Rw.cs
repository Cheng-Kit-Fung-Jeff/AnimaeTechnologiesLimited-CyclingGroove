using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Rw
{
    public static string Read(string path, out string error)
    {
        error = null;
        try {
            return File.ReadAllText(Path.Combine(Application.dataPath,path));
        }
        catch (Exception e)
        {
            error = e.Message;
        }
        return null;
    }
}

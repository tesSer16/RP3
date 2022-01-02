using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info
{
    public static int maxCombo { get; set; }
    public static float score { get; set; }
    public static int beats { get; set; }
    public static string selectedMusic { get; set; }
    public static string musicTitle { get; set; }
    public static string musicArtist { get; set; }
    public static int perfect { get; set; }
    public static int good { get; set; }
    public static int bad { get; set; }
    public static int miss { get; set; }
    public static float playTime { get; set; }
    public static int order { get; set; }
    public static List<Queue<int>> Chart { get; set; }
    public static float beatInterval { get; set; }
    public static string musicPath { get; set; }
}

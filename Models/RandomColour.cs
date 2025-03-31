using System;
using SkiaSharp;

namespace AOP_3.Models;
public class RandomColour
{
    public SKColor GetRandomColour()
    {
        var random = new Random();
        return new SKColor((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
    }
}
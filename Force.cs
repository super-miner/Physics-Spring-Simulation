using System;
using Microsoft.Xna.Framework;

public class Force
{
    public string name;
    public Vector2 vector;

    public Force(string name, float x, float y) {
        this.name = name;
        this.vector = new Vector2(x, y);

        if (vector != vector) { // Checks if NaN
            Console.WriteLine("The force " + name + " is NaN."); // TODO: Throw warning
        }
    }

    public Force(string name, Vector2 vector) {
        this.name = name;
        this.vector = vector;

        if (vector != vector) { // Checks if NaN
            Console.WriteLine("The force " + name + " is NaN."); // TODO: Throw warning
        }
    }
}
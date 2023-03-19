using Microsoft.Xna.Framework;

public class Camera
{
    public static Camera main = null;

    public Vector2 position = Vector2.Zero;
    public int pixelsPerMeter = 100;

    public Camera(Vector2 position, int pixelsPerMeter) {
        if (main == null) {
            main = this;
        }

        this.position = position;
        this.pixelsPerMeter = pixelsPerMeter;
    }

    public Vector2 WorldToScreenSpace(Vector2 worldPosition) {
        return worldPosition * pixelsPerMeter - position;
    }

    public float WorldToScreenSpace(float worldScale) {
        return worldScale * pixelsPerMeter;
    }
}
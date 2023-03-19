using Microsoft.Xna.Framework;

public class Camera
{
    public static Camera main = null;

    public Vector2 position = Vector2.Zero;
    public int pixelsPerMeter = 100;

    private GraphicsDeviceManager graphics;

    public Camera(GraphicsDeviceManager graphics, Vector2 position, int pixelsPerMeter) {
        if (main == null) {
            main = this;
        }

        this.graphics = graphics;
        this.position = position;
        this.pixelsPerMeter = pixelsPerMeter;
    }

    public Vector2 WorldToScreenSpace(Vector2 worldPosition) {
        return (worldPosition - position) * pixelsPerMeter + GetScreenSizePixels() / 2.0f;
    }

    public float WorldToScreenSpace(float worldScale) {
        return worldScale * pixelsPerMeter;
    }

    public Vector2 ScreenToWorldSpace(Vector2 screenPosition) {
        return (screenPosition - GetScreenSizePixels() / 2.0f) / pixelsPerMeter + position;
    }

    public Vector2 GetScreenSizePixels() {
        return new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
    }
}
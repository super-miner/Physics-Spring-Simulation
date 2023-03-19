using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class InteractionController
{
    private bool objectSelected = false;
    private bool movingCamera = false;
    private PhysicsHinge selectedHinge = null;
    private Vector2 selectedOffset = Vector2.Zero;
    private Vector2 lastMousePosition = Vector2.Zero;
    private int lastScrollWheelValue = 0;

    public void Update(float deltaTime) {
        MouseState mouse = Mouse.GetState();

        int deltaScroll = mouse.ScrollWheelValue - lastScrollWheelValue;
        lastScrollWheelValue = mouse.ScrollWheelValue;

        Camera.main.pixelsPerMeter = MathHelper.Clamp(Camera.main.pixelsPerMeter + deltaScroll / 25, 10, 1000);

        Vector2 screenMousePosition = mouse.Position.ToVector2();
        Vector2 mousePosition = Camera.main.ScreenToWorldSpace(screenMousePosition);
        Vector2 deltaMousePosition = (screenMousePosition - lastMousePosition) / Camera.main.pixelsPerMeter;
        lastMousePosition = screenMousePosition;

        if (mouse.LeftButton == ButtonState.Pressed) {
            if (objectSelected) {
                selectedHinge.position = mousePosition + selectedOffset;
            }
            else {
                PhysicsHinge nearestHinge = GetNearestHinge(mousePosition);
                float distanceSquared = Vector2.DistanceSquared(mousePosition, nearestHinge.position);

                if (distanceSquared <= PhysicsManager.GetInstance().circleGrabRadius * PhysicsManager.GetInstance().circleGrabRadius) {
                    objectSelected = true;

                    SelectHinge(mousePosition, nearestHinge);
                }
            }
        }
        else if (mouse.LeftButton == ButtonState.Released && objectSelected) {
            objectSelected = false;

            DeselectHinge();
        }

        if (mouse.MiddleButton == ButtonState.Pressed) {
            if (movingCamera) {
                Camera.main.position -= deltaMousePosition;
            }
            else {
                movingCamera = true;
            }
        }
        else if (mouse.MiddleButton == ButtonState.Released && movingCamera) {
            movingCamera = false;
        }

        Console.WriteLine(deltaMousePosition);
        Console.WriteLine(Camera.main.position);
    }

    public void SelectHinge(Vector2 mousePosition, PhysicsHinge physicsHinge) {
        selectedHinge = physicsHinge;
        selectedOffset = physicsHinge.position - mousePosition;

        selectedHinge.SetEnabled(false);
    }

    public void DeselectHinge() {
        selectedHinge.SetEnabled(true);

        selectedHinge = null;
        selectedOffset = Vector2.Zero;
    }

    public PhysicsHinge GetNearestHinge(Vector2 position) {
        PhysicsHinge nearestHinge = null;
        float nearestDistanceSquared = float.PositiveInfinity;
        foreach (PhysicsHinge physicsHinge in PhysicsManager.GetInstance().physicsHinges) {
            float distanceSquared = Vector2.DistanceSquared(position, physicsHinge.position);
            if (distanceSquared < nearestDistanceSquared) {
                nearestHinge = physicsHinge;
                nearestDistanceSquared = distanceSquared;
            }
        }
        return nearestHinge;
    }
}
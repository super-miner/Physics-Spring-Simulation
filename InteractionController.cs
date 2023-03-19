using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class InteractionController
{
    private bool objectSelected = false;
    private PhysicsHinge selectedHinge = null;
    private Vector2 selectedOffset = Vector2.Zero;

    public void Update(float deltaTime) {
        MouseState mouse = Mouse.GetState();

        if (mouse.LeftButton == ButtonState.Pressed) {
            Vector2 mousePosition = mouse.Position.ToVector2() / Camera.main.pixelsPerMeter;

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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class InteractionController
{
    private bool objectSelected = false;
    private PhysicsObject selectedObject = null;
    private Vector2 selectedOffset = Vector2.Zero;

    public void Update(float deltaTime) {
        MouseState mouse = Mouse.GetState();

        if (mouse.LeftButton == ButtonState.Pressed) {
            Vector2 mousePosition = mouse.Position.ToVector2() / PhysicsManager.GetInstance().pixelsPerMeter;

            if (objectSelected) {
                selectedObject.position = mousePosition + selectedOffset;
            }
            else {
                PhysicsObject nearestObject = GetNearestObject(mousePosition);
                float distanceSquared = Vector2.DistanceSquared(mousePosition, nearestObject.position);

                if (distanceSquared <= PhysicsManager.GetInstance().circleGrabRadius * PhysicsManager.GetInstance().circleGrabRadius) {
                    objectSelected = true;

                    SelectObject(mousePosition, nearestObject);
                }
            }
        }
        else if (mouse.LeftButton == ButtonState.Released && objectSelected) {
            objectSelected = false;

            DeselectObject();
        }
    }

    public void SelectObject(Vector2 mousePosition, PhysicsObject physicsObject) {
        selectedObject = physicsObject;
        selectedOffset = physicsObject.position - mousePosition;

        selectedObject.SetEnabled(false);
    }

    public void DeselectObject() {
        selectedObject.SetEnabled(true);

        selectedObject = null;
        selectedOffset = Vector2.Zero;
    }

    public PhysicsObject GetNearestObject(Vector2 position) {
        PhysicsObject nearestObject = null;
        float nearestDistanceSquared = float.PositiveInfinity;
        foreach (PhysicsObject physicsObject in PhysicsManager.GetInstance().physicsObjects) {
            float distanceSquared = Vector2.DistanceSquared(position, physicsObject.position);
            if (distanceSquared < nearestDistanceSquared) {
                nearestObject = physicsObject;
                nearestDistanceSquared = distanceSquared;
            }
        }
        return nearestObject;
    }
}
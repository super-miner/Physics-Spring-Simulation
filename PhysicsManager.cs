using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class PhysicsManager
{
    public static PhysicsManager instance = null;

    public int pixelsPerMeter = 100;
    public int simulationsPerFrame = 10;
    public float simulationSpeed = 1.0f;
    public float forceDisplayMultiplyer = 0.2f;
    public float circleGrabRadius = 0.25f;
    public float circleConnectorRadius = 0.1f;
    public bool displayForces = false;
    public List<PhysicsObject> physicsObjects = new List<PhysicsObject>();
    public List<PhysicsJoint> physicsJoints = new List<PhysicsJoint>();

    public static PhysicsManager GetInstance() {
        if (instance == null) {
            instance = new PhysicsManager();
        }

        return instance;
    }

    public void Simulate(float deltaTime) {
        for (int i = 0; i < simulationsPerFrame; i++) {
            foreach (PhysicsJoint physicsJoint in physicsJoints) {
                physicsJoint.Simulate(deltaTime * simulationSpeed / simulationsPerFrame);
            }

            foreach (PhysicsObject physicsObject in physicsObjects) {
                if (physicsObject.mass > 0.0f) {
                    physicsObject.queuedForces.Add(new Vector2(0.0f, 9.81f) * physicsObject.mass);
                }

                physicsObject.Simulate(deltaTime * simulationSpeed / simulationsPerFrame);
            }

            foreach (PhysicsJoint physicsJoint in physicsJoints) {
                physicsJoint.PostSimulate(deltaTime * simulationSpeed / simulationsPerFrame);
            }

            foreach (PhysicsObject physicsObject in physicsObjects) {
                physicsObject.PostSimulate(deltaTime * simulationSpeed / simulationsPerFrame);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        foreach (PhysicsJoint physicsJoint in physicsJoints) {
            physicsJoint.Draw(spriteBatch);
        }

        foreach (PhysicsObject physicsObject in physicsObjects) {
            physicsObject.Draw(spriteBatch);
        }

        if (displayForces) {
            foreach (PhysicsObject physicsObject in physicsObjects) {
                physicsObject.DrawForces(spriteBatch);
            }
        }
    }
}
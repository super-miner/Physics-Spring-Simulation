using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class PhysicsManager
{
    public static PhysicsManager instance = null;

    public int simulationsPerFrame = 10;
    public float simulationSpeed = 1.0f;
    public float forceDisplayMultiplyer = 0.2f;
    public float circleGrabRadius = 0.25f;
    public float circleConnectorRadius = 0.1f;
    public bool displayForces = false;
    public List<PhysicsHinge> physicsHinges = new List<PhysicsHinge>();
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

            foreach (PhysicsHinge physicsHinge in physicsHinges) {
                if (physicsHinge.mass > 0.0f) {
                    physicsHinge.queuedForces.Add(new Force("Gravity Force", new Vector2(0.0f, 9.81f) * physicsHinge.mass));
                }

                physicsHinge.Simulate(deltaTime * simulationSpeed / simulationsPerFrame);
            }

            foreach (PhysicsJoint physicsJoint in physicsJoints) {
                physicsJoint.PostSimulate(deltaTime * simulationSpeed / simulationsPerFrame);
            }

            foreach (PhysicsHinge physicsHinge in physicsHinges) {
                physicsHinge.PostSimulate(deltaTime * simulationSpeed / simulationsPerFrame);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        foreach (PhysicsJoint physicsJoint in physicsJoints) {
            physicsJoint.Draw(spriteBatch);
        }

        foreach (PhysicsHinge physicsHinge in physicsHinges) {
            physicsHinge.Draw(spriteBatch);
        }

        if (displayForces) {
            foreach (PhysicsHinge physicsHinge in physicsHinges) {
                physicsHinge.DrawForces(spriteBatch);
            }
        }
    }
}
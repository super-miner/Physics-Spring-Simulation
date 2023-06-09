using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

public class PhysicsJoint
{
    public PhysicsHinge connection1;
    public PhysicsHinge connection2;
    public float equilibriumLength;
    public readonly float equilibriumLengthSquared;
    public float frictionCoefficient;

    public PhysicsJoint(PhysicsHinge connection1, PhysicsHinge connection2, float equilibriumLength, float frictionCoefficient) {
        this.connection1 = connection1;
        this.connection2 = connection2;
        this.equilibriumLength = equilibriumLength;
        this.equilibriumLengthSquared = equilibriumLength * equilibriumLength;
        this.frictionCoefficient = frictionCoefficient;
    }

    public virtual void Simulate(float deltaTime) {

    }

    public virtual void PostSimulate(float deltaTime) {
        float connectionsDistance = Vector2.Distance(connection1.position, connection2.position);
        Vector2 deltaConnections = Vector2.Normalize(connection2.position - connection1.position);
        Vector2 perpDeltaConnections = new Vector2(-deltaConnections.Y, deltaConnections.X);

        float normalForce1 = Vector2.Dot(connection2.GetNetForce().vector, deltaConnections);
        float frictionCoefficient1 = frictionCoefficient + connection2.frictionCoefficient;
        float frictionForce1Pivot = frictionCoefficient1 * normalForce1;
        float firctionTorque1 = frictionForce1Pivot * PhysicsManager.GetInstance().circleConnectorRadius;
        float frictionForce1 = firctionTorque1 / connectionsDistance;

        Vector2 swingVector1 = connection1.velocity == Vector2.Zero ? Vector2.Zero : perpDeltaConnections * Vector2.Dot(connection1.velocity, perpDeltaConnections);
        Vector2 swingDirection1 = swingVector1 == Vector2.Zero ? Vector2.Zero : -Vector2.Normalize(swingVector1);

        float normalForce2 = Vector2.Dot(connection1.GetNetForce().vector, deltaConnections);
        float frictionCoefficient2 = frictionCoefficient + connection1.frictionCoefficient;
        float frictionForce2Pivot = frictionCoefficient2 * normalForce2;
        float firctionTorque2 = frictionForce2Pivot * PhysicsManager.GetInstance().circleConnectorRadius;
        float frictionForce2 = firctionTorque2 / connectionsDistance;

        Vector2 swingVector2 = connection2.velocity == Vector2.Zero ? Vector2.Zero : perpDeltaConnections * Vector2.Dot(connection2.velocity, perpDeltaConnections);
        Vector2 swingDirection2 = swingVector2 == Vector2.Zero ? Vector2.Zero : -Vector2.Normalize(swingVector2);

        if (connection1.mass > 0.0f && swingDirection1 != Vector2.Zero) {
            connection1.queuedForces.Add(new Force("Friction Force", frictionForce1 * swingDirection1));
        }
        if (connection2.mass > 0.0f && swingDirection2 != Vector2.Zero) {
            connection2.queuedForces.Add(new Force("Friction Force", frictionForce2 * swingDirection2));
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch) {
        float distance = Vector2.Distance(connection1.position, connection2.position);
        float displacementFromEquilibrium = MathF.Abs(distance - equilibriumLength);

        ShapeExtensions.DrawLine(spriteBatch, Camera.main.WorldToScreenSpace(connection1.position), Camera.main.WorldToScreenSpace(connection2.position), Color.Lerp(Color.Red, Color.Blue, 1 - (displacementFromEquilibrium / equilibriumLength)), Camera.main.WorldToScreenSpace(0.04f), 1);
    }
}
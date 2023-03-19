using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

public class PhysicsObject
{
    public bool enabled = true;
    public float mass = 0.0f;
    public Vector2 position = Vector2.Zero;
    public Vector2 velocity = Vector2.Zero;
    public List<Vector2> queuedForces = new List<Vector2>();

    private List<Vector2> forces = new List<Vector2>();

    public PhysicsObject(float mass, Vector2 position) {
        this.mass = mass;
        this.position = position;
    }

    public PhysicsObject(float mass, Vector2 position, Vector2 velocity) {
        this.mass = mass;
        this.position = position;
        this.velocity = velocity;
    }

    public void Simulate(float deltaTime) {
        RealizeForces();

        if (!enabled) {
            return;
        }

        if (mass > 0.0f) {
            Vector2 netForce = GetNetForce();
            Vector2 acceleration = netForce / mass;

            velocity += acceleration * deltaTime;
            position += velocity * deltaTime;
        }
    }

    public void PostSimulate(float deltaTime) {
        if (!enabled) {
            return;
        }
    }

    public void RealizeForces() {
        forces = new List<Vector2>(queuedForces);

        queuedForces.Clear();
    }

    public void Draw(SpriteBatch spriteBatch) {
        ShapeExtensions.DrawCircle(spriteBatch, position * PhysicsManager.GetInstance().pixelsPerMeter, PhysicsManager.GetInstance().circleGrabRadius * PhysicsManager.GetInstance().pixelsPerMeter, 100, enabled ? Color.Red : Color.LightGray, 1.0f, 0);
        ShapeExtensions.DrawCircle(spriteBatch, position * PhysicsManager.GetInstance().pixelsPerMeter, (PhysicsManager.GetInstance().circleConnectorRadius / 2) * PhysicsManager.GetInstance().pixelsPerMeter, 100, enabled ? Color.Red : Color.LightGray, PhysicsManager.GetInstance().circleConnectorRadius * PhysicsManager.GetInstance().pixelsPerMeter, 0);
    }

    public void DrawForces(SpriteBatch spriteBatch) {
        if (!enabled) {
            return;
        }

        if (mass <= 0) {
            return;
        }

        foreach (Vector2 force in forces) {
            if (force.LengthSquared() > 0.0001f) {
                DrawArrow(spriteBatch, position * PhysicsManager.GetInstance().pixelsPerMeter, (position + force * PhysicsManager.GetInstance().forceDisplayMultiplyer) * PhysicsManager.GetInstance().pixelsPerMeter, Color.Green, 1.0f, new Vector2(10.0f, 4.0f));
            }
        }
    }

    public Vector2 GetNetForce() {
        Vector2 netForce = Vector2.Zero;
        foreach (Vector2 force in forces) {
            netForce += force;
        }
        return netForce;
    }

    public void SetEnabled(bool value) {
        if (enabled != value) {
            velocity = Vector2.Zero;
            forces.Clear();
        }

        enabled = value;
    }

    private void DrawArrow(SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition, Color color, float thickness, Vector2 arrowHeadSize) {
        float distance = Vector2.Distance(startPosition, endPosition);
        Vector2 direction = endPosition - startPosition;
        Vector2 normalizedDirection = Vector2.Normalize(direction);

        float lineDistance = distance - arrowHeadSize.X;
        if (lineDistance > 0) {
            ShapeExtensions.DrawLine(spriteBatch, startPosition, startPosition + normalizedDirection * lineDistance, color, thickness);
        }

        Vector2 lineEndPoint = startPosition + normalizedDirection * lineDistance;

        Vector2 perp = new Vector2(-normalizedDirection.Y, normalizedDirection.X);
        Vector2 perp1 = perp * arrowHeadSize.Y;
        Vector2 perp2 = -perp * arrowHeadSize.Y;

        List<Vector2> points = new List<Vector2>() {
            perp1,
            normalizedDirection * (distance - lineDistance),
            perp2
        };

        ShapeExtensions.DrawPolygon(spriteBatch, lineEndPoint, points, color, thickness);
    }
}
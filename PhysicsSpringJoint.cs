using System;
using Microsoft.Xna.Framework;

public class PhysicsSpringJoint : PhysicsJoint
{
    public float springCoefficient = 0.0f;
    public float dampingCoefficient = 0.0f;

    public PhysicsSpringJoint(PhysicsHinge connection1, PhysicsHinge connection2, float equilibriumLength, float frictionCoefficient, float springCoefficient, float dampingCoefficient) : base(connection1, connection2, equilibriumLength, frictionCoefficient) {
        this.springCoefficient = springCoefficient;
        this.dampingCoefficient = dampingCoefficient;
    }

    public override void Simulate(float deltaTime) {
        float distance = Vector2.Distance(connection1.position, connection2.position);
        if (distance != equilibriumLength) {
            float displacementFromEquilibrium = distance - equilibriumLength;
            float forceMagnitude = springCoefficient * (displacementFromEquilibrium);

            Vector2 deltaConnections = Vector2.Normalize(connection2.position - connection1.position);
            Vector2 forceSpring = deltaConnections * -forceMagnitude;

            connection1.queuedForces.Add(new Force("Spring Force", -forceSpring));
            connection2.queuedForces.Add(new Force("Spring Force", forceSpring));

            if (connection1.mass > 0.0f) {
                float criticalDampingCoefficient1 = 2 * MathF.Sqrt(springCoefficient * connection1.mass);
                Vector2 forceDamping1 = -(dampingCoefficient / criticalDampingCoefficient1) * (deltaConnections * Vector2.Dot(connection1.velocity, deltaConnections));

                connection1.queuedForces.Add(new Force("Damping Force", forceDamping1));
            }

            if (connection2.mass > 0.0f) {
                float criticalDampingCoefficient2 = 2 * MathF.Sqrt(springCoefficient * connection2.mass);
                Vector2 forceDamping2 = -(dampingCoefficient / criticalDampingCoefficient2) * (deltaConnections * Vector2.Dot(connection2.velocity, deltaConnections));

                connection2.queuedForces.Add(new Force("Damping Force", forceDamping2));
            }

            base.Simulate(deltaTime);
        }
    }

    public override void PostSimulate(float deltaTime) {
        base.PostSimulate(deltaTime);
    }
}
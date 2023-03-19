using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;

namespace PhysicsSimulation;

public class GameManager : Game
{
    public static GameManager instance = null;
    
    public InteractionController interactionController;
    public Camera mainCamera;
    public bool simulating = true;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Desktop _desktop;

    public GameManager() {
        if (instance == null) {
            instance = this;
        }

        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        mainCamera = new Camera(_graphics, Vector2.Zero, 100);
        interactionController = new InteractionController();

        MyraEnvironment.Game = this;

        Grid grid = new Grid {
            RowSpacing = 8,
            ColumnSpacing = 8
        };

        for (int i = 0; i < 2; i++) {
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        }

        for (int i = 0; i < 4; i++) {
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
        }

        Label exampleSimulationLabel = new Label {
            GridColumn = 0,
            GridRow = 0,
            Text = "Example Simulation: "
        };
        grid.Widgets.Add(exampleSimulationLabel);

        ComboBox exampleSimulationValue = new ComboBox {
            GridColumn = 1,
            GridRow = 0,
            SelectedIndex = 1
        };
        exampleSimulationValue.Items.Add(new ListItem("Single Spring"));
        exampleSimulationValue.Items.Add(new ListItem("Chain of Springs"));
        exampleSimulationValue.Items.Add(new ListItem("No Fixed Point"));
        exampleSimulationValue.Items.Add(new ListItem("Suspended Ball"));
        exampleSimulationValue.SelectedIndexChanged += (arg1, arg2) => {
            PhysicsManager.GetInstance().physicsHinges.Clear();
            PhysicsManager.GetInstance().physicsJoints.Clear();

            switch (exampleSimulationValue.SelectedIndex) {
                case 0:
                    LoadSimulation1();
                    break;
                case 1:
                    LoadSimulation2();
                    break;
                case 2:
                    LoadSimulation3();
                    break;
                case 3:
                    LoadSimulation4();
                    break;
            }
        };
        grid.Widgets.Add(exampleSimulationValue);

        Label simulationEnabledLabel = new Label {
            GridColumn = 0,
            GridRow = 1,
            Text = "Simulation Enabled: "
        };
        grid.Widgets.Add(simulationEnabledLabel);

        CheckBox simulationEnabledValue = new CheckBox {
            GridColumn = 1,
            GridRow = 1,
            IsChecked = true
        };
        simulationEnabledValue.PressedChanged += (arg1, arg2) => {
            simulating = simulationEnabledValue.IsChecked;
        };
        grid.Widgets.Add(simulationEnabledValue);

        Label simulationSpeedLabel = new Label {
            GridColumn = 0,
            GridRow = 2,
            Text = "Simulation Speed (%): "
        };
        grid.Widgets.Add(simulationSpeedLabel);

        SpinButton simulationSpeedValue = new SpinButton {
            GridColumn = 1,
            GridRow = 2,
            Width = 100,
            Increment = 10.0f,
            Value = 100.0f,
            Maximum = 500.0f,
            Minimum = 0.0f
        };
        simulationSpeedValue.ValueChangedByUser += (arg1, arg2) => {
            PhysicsManager.GetInstance().simulationSpeed = (float) simulationSpeedValue.Value / 100.0f;
        };
        grid.Widgets.Add(simulationSpeedValue);

        Label showForcesLabel = new Label {
            GridColumn = 0,
            GridRow = 3,
            Text = "Show Forces: "
        };
        grid.Widgets.Add(showForcesLabel);

        CheckBox showForcesValue = new CheckBox {
            GridColumn = 1,
            GridRow = 3,
            IsChecked = false
        };
        showForcesValue.PressedChanged += (arg1, arg2) => {
            PhysicsManager.GetInstance().displayForces = showForcesValue.IsChecked;
        };
        grid.Widgets.Add(showForcesValue);

        _desktop = new Desktop();
        _desktop.Root = grid;

        LoadSimulation1();

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
            Exit();
        }

        float deltaTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

        interactionController.Update(deltaTime);

        if (simulating) {
            PhysicsManager.GetInstance().Simulate(deltaTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();
        PhysicsManager.GetInstance().Draw(_spriteBatch);
        _spriteBatch.End();

        _desktop.Render();

        base.Draw(gameTime);
    }

    private void LoadSimulation1() {
        PhysicsHinge ball1 = new PhysicsHinge(0.0f, new Vector2(0.0f, 0.0f));
        PhysicsHinge ball2 = new PhysicsHinge(0.5f, new Vector2(0.5f, 0.0f));

        PhysicsManager.GetInstance().physicsHinges.Add(ball1);
        PhysicsManager.GetInstance().physicsHinges.Add(ball2);

        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball1, ball2, 1.0f, 0.3f, 15.0f, 5.0f));
    }

    private void LoadSimulation2() {
        PhysicsHinge ball1 = new PhysicsHinge(0.0f, new Vector2(4.0f, 0.0f));
        PhysicsHinge ball2 = new PhysicsHinge(0.1f, new Vector2(4.5f, 0.0f));
        PhysicsHinge ball3 = new PhysicsHinge(0.1f, new Vector2(5.0f, 0.0f));
        PhysicsHinge ball4 = new PhysicsHinge(0.1f, new Vector2(5.5f, 0.0f));
        PhysicsHinge ball5 = new PhysicsHinge(0.1f, new Vector2(6.0f, 0.0f));

        PhysicsManager.GetInstance().physicsHinges.Add(ball1);
        PhysicsManager.GetInstance().physicsHinges.Add(ball2);
        PhysicsManager.GetInstance().physicsHinges.Add(ball3);
        PhysicsManager.GetInstance().physicsHinges.Add(ball4);
        PhysicsManager.GetInstance().physicsHinges.Add(ball5);

        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball1, ball2, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball2, ball3, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball3, ball4, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball4, ball5, 0.3f, 0.1f, 15.0f, 0.3f));
    }

    private void LoadSimulation3() {
        PhysicsHinge ball1 = new PhysicsHinge(0.5f, new Vector2(3.75f, 0.0f));
        PhysicsHinge ball2 = new PhysicsHinge(0.5f, new Vector2(4.25f, 0.0f));

        PhysicsManager.GetInstance().physicsHinges.Add(ball1);
        PhysicsManager.GetInstance().physicsHinges.Add(ball2);

        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball1, ball2, 1.0f, 0.1f, 15.0f, 5.0f));
    }

    private void LoadSimulation4() {
        PhysicsHinge ball1 = new PhysicsHinge(0.0f, new Vector2(4.0f, 2.0f));
        PhysicsHinge ball2 = new PhysicsHinge(0.1f, new Vector2(4.5f, 2.5f));
        PhysicsHinge ball3 = new PhysicsHinge(0.1f, new Vector2(3.5f, 2.5f));
        PhysicsHinge ball4 = new PhysicsHinge(0.1f, new Vector2(4.5f, 3.0f));
        PhysicsHinge ball5 = new PhysicsHinge(0.1f, new Vector2(3.5f, 3.0f));
        PhysicsHinge ball6 = new PhysicsHinge(0.1f, new Vector2(4.0f, 3.5f));
        PhysicsHinge ball7 = new PhysicsHinge(0.1f, new Vector2(4.25f, 2.25f));
        PhysicsHinge ball8 = new PhysicsHinge(0.1f, new Vector2(3.75f, 2.25f));
        PhysicsHinge ball9 = new PhysicsHinge(0.1f, new Vector2(4.25f, 2.75f));
        PhysicsHinge ball10 = new PhysicsHinge(0.1f, new Vector2(3.75f, 2.75f));
        PhysicsHinge ball11 = new PhysicsHinge(0.1f, new Vector2(4.25f, 3.25f));
        PhysicsHinge ball12 = new PhysicsHinge(0.1f, new Vector2(3.75f, 3.25f));
        PhysicsHinge ball13 = new PhysicsHinge(0.1f, new Vector2(4.0f, 2.75f));

        PhysicsManager.GetInstance().physicsHinges.Add(ball1);
        PhysicsManager.GetInstance().physicsHinges.Add(ball2);
        PhysicsManager.GetInstance().physicsHinges.Add(ball3);
        PhysicsManager.GetInstance().physicsHinges.Add(ball4);
        PhysicsManager.GetInstance().physicsHinges.Add(ball5);
        PhysicsManager.GetInstance().physicsHinges.Add(ball6);
        PhysicsManager.GetInstance().physicsHinges.Add(ball7);
        PhysicsManager.GetInstance().physicsHinges.Add(ball8);
        PhysicsManager.GetInstance().physicsHinges.Add(ball9);
        PhysicsManager.GetInstance().physicsHinges.Add(ball10);
        PhysicsManager.GetInstance().physicsHinges.Add(ball11);
        PhysicsManager.GetInstance().physicsHinges.Add(ball12);
        PhysicsManager.GetInstance().physicsHinges.Add(ball13);

        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball1, ball7, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball1, ball8, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball7, ball2, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball8, ball3, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball2, ball9, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball3, ball10, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball9, ball4, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball10, ball5, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball4, ball11, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball5, ball12, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball11, ball6, 0.3f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball12, ball6, 0.3f, 0.1f, 15.0f, 0.3f));

        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball1, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball2, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball3, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball4, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball5, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball6, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball7, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball8, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball9, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball10, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball11, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
        PhysicsManager.GetInstance().physicsJoints.Add(new PhysicsSpringJoint(ball12, ball13, 0.75f, 0.1f, 15.0f, 0.3f));
    }
}

using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 8.0f;
	public const float JumpVelocity = 12.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 24.0f; //ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	public float sensitivity = 0.002f;
	Camera3D camera3D;
	RayCast3D rayCast3D;

	public override void _Ready(){
		camera3D = GetNode<Camera3D>("Camera3D");
		rayCast3D = GetNode<RayCast3D>("Camera3D/RayCast3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;		 
	}
public override void _UnhandledInput(InputEvent @event)
{
	InputEventMouseMotion e = (InputEventMouseMotion)@event;
    if(e != null){
			float tempCal = Rotation.Y - e.Relative.X * sensitivity;
			Rotation = new Vector3(Rotation.X, tempCal, Rotation.Z);
			tempCal = camera3D.Rotation.X - e.Relative.Y  * sensitivity;
			//camera3D.Rotation = new Vector3(tempCal, camera3D.Rotation.Y, camera3D.Rotation.Z);
			tempCal = Mathf.Clamp(tempCal, Mathf.DegToRad(-70), Mathf.DegToRad(80));
			camera3D.Rotation = new Vector3(tempCal, camera3D.Rotation.Y, camera3D.Rotation.Z);
		}
}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		if (Input.IsActionJustPressed("left_click"))
		{
    		if (rayCast3D.IsColliding())
    		{
        		var blocks = rayCast3D.GetCollider() as BlockBehaviour;
        		if (blocks != null && blocks.HasMethod("DestroyBlock"))
        		{
            		blocks.DestroyBlock(rayCast3D.GetCollisionPoint() - rayCast3D.GetCollisionNormal());
        		}
			}
    	}
		if (Input.IsActionJustPressed("right_click"))
		{
    		if (rayCast3D.IsColliding())
    		{
        		var blocks = rayCast3D.GetCollider() as BlockBehaviour;
        		if (blocks != null && blocks.HasMethod("PlaceBlock"))
        		{
            		blocks.PlaceBlock(rayCast3D.GetCollisionPoint() + rayCast3D.GetCollisionNormal(), 17);
        		}
			}
    	}

		Velocity = velocity;
		MoveAndSlide();
	}
}

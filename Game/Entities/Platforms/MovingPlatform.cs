using System.Collections.Generic;

class MovingPlatform : PlatformBlock
{
    public float speed { get; private set; } // Speed of the platform movement
    private float distance; // Total distance the platform moves to the right
    private float initialX; // Starting X position of the platform
    public int direction { get; private set; } = 1; // 1 for moving right, -1 for moving left
    private bool conveyor = false;

    public MovingPlatform(float x, float y, char sprite, float speed, float distance, bool conveyor=false) : base(x, y, sprite)
    {
        this.speed = speed;
        this.distance = distance;
        this.initialX = x;
        this.conveyor = conveyor;
    }

    public void UpdatePosition(List<Entity> allEntities)
    {
        // Update the platform's position
        if (!conveyor) currentXVelocity = speed * direction;
        UpdatePosition(false, allEntities);

        // Check if the platform has reached the right or left limit
        if (x > initialX + distance)
        {
            x = initialX + distance; // Clamp to the right limit
            direction = -1; // Change direction to left
        }
        else if (x < initialX)
        {
            x = initialX; // Clamp to the left limit
            direction = 1; // Change direction to right
        }
    }
}
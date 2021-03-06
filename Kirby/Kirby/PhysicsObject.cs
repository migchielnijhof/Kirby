﻿using Microsoft.Xna.Framework;
using System;

abstract class PhysicsObject : GameObject
{

    public float Gravity = 0.05f * Game.SpriteScale;

    public Vector2 Position;

    public Vector2 Velocity;

    /// <summary>
    /// The bounding box of the object.
    /// </summary>
    public Rectangle BoundingBox
    {
        get
        {
            boundingBox.Location = new Point((int)Position.X, (int)Position.Y);
            return boundingBox;
        }
    }
    protected Rectangle boundingBox;

    protected bool onGround;

    public PhysicsObject(GameObject parent, ObjectType type) : base(parent, type)
    {
    }

    public void DoPhysics()
    {
        if (!onGround)
            Velocity.Y += Gravity;

        TileGrid grid = (parent as Level).Find(ObjectType.TileGrid) as TileGrid;

        Vector2 distance = Velocity / Game.SpriteScale;

        do
        {
            if (distance.X != 0 && Math.Abs(distance.X / distance.Y) >= Math.Abs(Velocity.X / Velocity.Y))
            {
                if (distance.X > 0)
                {
                    if (distance.X < 1)
                    {
                        float d = distance.X;
                        distance.X = 0;
                        Position.X += d * Game.SpriteScale;
                        if (MapCollisions(grid, new Vector2(d * Game.SpriteScale, 0)))
                        {
                            distance.X = 0;
                            Velocity.X = 0;
                        }
                    }
                    else
                    {
                        distance.X--;
                        Position.X += Game.SpriteScale;
                        if (MapCollisions(grid, new Vector2(1 * Game.SpriteScale, 0)))
                        {
                            distance.X = 0;
                            Velocity.X = 0;
                        }
                    }
                }
                else
                {
                    if (distance.X > -1)
                    {
                        float d = distance.X;
                        distance.X = 0;
                        Position.X += d * Game.SpriteScale;
                        if (MapCollisions(grid, new Vector2(d * Game.SpriteScale, 0)))
                        {
                            distance.X = 0;
                            Velocity.X = 0;
                        }
                    }
                    else
                    {
                        distance.X++;
                        Position.X -= Game.SpriteScale;
                        if (MapCollisions(grid, new Vector2(-1 * Game.SpriteScale, 0)))
                        {
                            distance.X = 0;
                            Velocity.X = 0;
                        }
                    }
                }
            }
            else
            {
                if (distance.Y > 0)
                {
                    if (distance.Y < 1)
                    {
                        float d = distance.Y;
                        distance.Y = 0;
                        Position.Y += d * Game.SpriteScale;
                        if (MapCollisions(grid, new Vector2(0, d * Game.SpriteScale)))
                        {
                            distance.Y = 0;
                            Velocity.Y = 0;
                        }
                    }
                    else
                    {
                        distance.Y--;
                        Position.Y += Game.SpriteScale;
                        if (MapCollisions(grid, new Vector2(0, 1 * Game.SpriteScale)))
                        {
                            distance.Y = 0;
                            Velocity.Y = 0;
                        }
                    }
                }
                else
                {
                    if (distance.Y > -1)
                    {
                        float d = distance.Y;
                        distance.Y = 0;
                        Position.Y += d * Game.SpriteScale;
                        if (MapCollisions(grid, new Vector2(0, d * Game.SpriteScale)))
                        {
                            distance.Y = 0;
                            Velocity.Y = 0;
                        }
                    }
                    else
                    {
                        distance.Y++;
                        Position.Y -= Game.SpriteScale;
                        if (MapCollisions(grid, new Vector2(0, -1 * Game.SpriteScale)))
                        {
                            distance.Y = 0;
                            Velocity.Y = 0;
                        }
                    }
                }
            }
        } while (distance.X != 0 || distance.Y != 0);
        onGround = TestGround(grid);
    }

    protected bool TestGround(TileGrid grid)
    {
        try
        {
            return (grid.tiles[TileGrid.GetIndexX(BoundingBox.Left), TileGrid.GetIndexY(BoundingBox.Bottom + 1)].Solid || grid.tiles[TileGrid.GetIndexX(BoundingBox.Right), TileGrid.GetIndexY(BoundingBox.Bottom + 1)].Solid || (TileGrid.GetIndexY(BoundingBox.Bottom) != TileGrid.GetIndexY(BoundingBox.Bottom + 1) && (grid.tiles[TileGrid.GetIndexX(BoundingBox.Left), TileGrid.GetIndexY(BoundingBox.Bottom + 1)].JumpThrough || grid.tiles[TileGrid.GetIndexX(BoundingBox.Right), TileGrid.GetIndexY(BoundingBox.Bottom + 1)].JumpThrough)));
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
    }

    protected virtual bool MapCollisions(TileGrid grid, Vector2 objectMovement)
    {
        if (Type != ObjectType.Particle)
        {
            if (Position.X < 0)
            {
                Position -= objectMovement;
                return true;
            }
            try
            {
                if (grid.tiles[TileGrid.GetIndexX(BoundingBox.Left), TileGrid.GetIndexY(BoundingBox.Top)].Solid | grid.tiles[TileGrid.GetIndexX(BoundingBox.Right), TileGrid.GetIndexY(BoundingBox.Top)].Solid | grid.tiles[TileGrid.GetIndexX(BoundingBox.Left), TileGrid.GetIndexY(BoundingBox.Bottom)].Solid | grid.tiles[TileGrid.GetIndexX(BoundingBox.Right), TileGrid.GetIndexY(BoundingBox.Bottom)].Solid)
                {
                    Position -= objectMovement;
                    return true;
                }
                else if (TileGrid.GetIndexY(BoundingBox.Bottom) != TileGrid.GetIndexY(BoundingBox.Bottom - 1) && (grid.tiles[TileGrid.GetIndexX(BoundingBox.Left), TileGrid.GetIndexY(BoundingBox.Bottom)].JumpThrough || grid.tiles[TileGrid.GetIndexX(BoundingBox.Right), TileGrid.GetIndexY(BoundingBox.Bottom)].JumpThrough))
                {
                    Position -= objectMovement;
                    return true;
                }
            }
            catch (IndexOutOfRangeException)
            {
                if (objectMovement.Y <= 0)
                {
                    Position -= objectMovement;
                    return true;
                }
            }
        }
        return false;
    }

}
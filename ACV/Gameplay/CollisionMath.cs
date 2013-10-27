using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACV
{
    /// <summary>
    /// A code container for collision-related mathematical functions.
    /// </summary>
    static public class CollisionMath
    {
        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                           Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }


        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels between two
        /// sprites.
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(
                            Matrix transformA, int widthA, int heightA, Color[] dataA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }


        /// <summary>
        /// Calculates an axis aligned rectangle which fully contains an arbitrarily
        /// transformed axis aligned rectangle.
        /// </summary>
        /// <param name="rectangle">Original bounding rectangle.</param>
        /// <param name="transform">World transform of the rectangle.</param>
        /// <returns>A new rectangle which contains the trasnformed rectangle.</returns>
        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                                           Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        //#region Helper Types

        ///// <summary>
        ///// Data defining a circle/line collision result.
        ///// </summary>
        ///// <remarks>Also used for circle/rectangles.</remarks>
        //public struct CircleLineCollisionResult
        //{
        //    public bool Collision;
        //    public Vector2 Point;
        //    public Vector2 Normal;
        //    public float Distance;
        //}
        
        //#endregion


        //#region Collision Methods

        //public static void CalcHull(GameObject gameObject)
        //{
        //    uint[] bits = new uint[gameObject.Sprite.Width * gameObject.Sprite.Height];
        //    gameObject.Sprite.GetData<uint>(bits);
            
        //    gameObject.Hull = new List<Point>();

        //    for (int x = 0; x < gameObject.Sprite.Width; x++)
        //    {
        //        for (int y = 0; y < gameObject.Sprite.Height; y++)
        //        {
        //            if ((bits[x + y * gameObject.Sprite.Width] & 0xFF000000) >> 24 <= 20)
        //                continue;

                    
        //            bool bSilouette = false;

        //            //check for an alpha next to a color to make sure this is an edge
        //            //right
        //            if (x < gameObject.Sprite.Width - 1)
        //            {
        //                if ((bits[x + 1 + y * gameObject.Sprite.Width] & 0xFF000000) >> 24 <= 20)
        //                    bSilouette = true;
        //            }
        //            //left
        //            if (!bSilouette && x > 0)
        //            {
        //                if ((bits[x - 1 + y * gameObject.Sprite.Width] & 0xFF000000) >> 24 <= 20)
        //                    bSilouette = true;
        //            }
        //            //top
        //            if (!bSilouette && y > 0)
        //            {
        //                if ((bits[x + (y - 1) * gameObject.Sprite.Width] & 0xFF000000) >> 24 <= 20)
        //                    bSilouette = true;
        //            }
        //            //bottom
        //            if (!bSilouette && y < gameObject.Sprite.Height-1)
        //            {
        //                if ((bits[x + (y + 1) * gameObject.Sprite.Width] & 0xFF000000) >> 24 <= 20)
        //                    bSilouette = true;
        //            }
        //            //bottom left
        //            if (!bSilouette && x > 0 && y < gameObject.Sprite.Height - 1)
        //            {
        //                if ((bits[x - 1 + (y + 1) * gameObject.Sprite.Width] & 0xFF000000) >> 24 <= 20)
        //                    bSilouette = true;
        //            }
        //            //bottom right
        //            if (!bSilouette && x < gameObject.Sprite.Width - 1 && y < gameObject.Sprite.Height-1)
        //            {
        //                if ((bits[x + 1 + (y + 1) * gameObject.Sprite.Width] & 0xFF000000) >> 24 <= 20)
        //                    bSilouette = true;
        //            }
        //            //top left
        //            if (!bSilouette && x > 0 && y > 0)
        //            {
        //                if ((bits[x - 1 + (y - 1) * gameObject.Sprite.Width] & 0xFF000000) >> 24 <= 20)
        //                    bSilouette = true;
        //            }
        //            //top right
        //            if (!bSilouette && x < gameObject.Sprite.Width && y > 0)
        //            {
        //                if ((bits[x + 1 + (y - 1) * gameObject.Sprite.Width] & 0xFF000000) >> 24 <= 20)
        //                    bSilouette = true;
        //            }

        //            Point p = new Point(x, y);
        //            if(bSilouette && !gameObject.Hull.Contains(p))    
        //                gameObject.Hull.Add(p);
        //        }
        //    }
        //}

        //public static void UpdateHull(GameObject gameObject)
        //{
        //    gameObject.HullTransformed = new Dictionary<Point, bool>(gameObject.HullTransformed == null ? 0 : gameObject.HullTransformed.Count);

        //    float cos = (float)Math.Cos(gameObject.Rotation);
        //    float sin = (float)Math.Sin(gameObject.Rotation);

        //    foreach (Point p in gameObject.Hull)
        //    {
        //        int newX = (int)Math.Round((p.X - gameObject.Origin.X) * cos - (p.Y - gameObject.Origin.Y) * sin);
        //        int newY = (int)Math.Round((p.Y - gameObject.Origin.Y) * cos + (p.X - gameObject.Origin.X) * sin);

        //        if (gameObject.MinX > newX) gameObject.MinX = newX;
        //        if (gameObject.MinY > newY) gameObject.MinY = newY;
        //        if (gameObject.MaxX < newX) gameObject.MaxX = newX;
        //        if (gameObject.MaxY < newY) gameObject.MaxY = newY;

        //        Point newP = new Point(newX, newY);
        //        if(!gameObject.HullTransformed.ContainsKey(newP))
        //        {
        //            gameObject.HullTransformed.Add(newP, true);
        //        }                    
                
        //    }

        //    gameObject.UpdateHull = false;
        //}

        //public static bool IntersectsGameObject(GameObject a, GameObject b)
        //{
        //    BoundingSphere aboundingSphere;
        //    BoundingSphere bboundingSphere;

        //    aboundingSphere.Center = new Vector3(a.Position, 0);
        //    aboundingSphere.Radius = (float)Math.Sqrt(a.LastKnownSize.X * a.LastKnownSize.X + a.LastKnownSize.Y * a.LastKnownSize.Y) / 2.0f;

        //    bboundingSphere.Center = new Vector3(a.Position, 0);
        //    bboundingSphere.Radius = (float)Math.Sqrt(b.LastKnownSize.X * b.LastKnownSize.X + b.LastKnownSize.Y * b.LastKnownSize.Y) / 2.0f;

        //    if (!aboundingSphere.Intersects(bboundingSphere))
        //        return false;

        //    if (a.UpdateHull)
        //        CollisionMath.UpdateHull(a);

        //    if (b.UpdateHull)
        //        CollisionMath.UpdateHull(b);

        //    int minX1 = a.MinX + (int)a.Position.X;
        //    int maxX1 = a.MaxX + (int)a.Position.X;
        //    int minY1 = a.MinY + (int)a.Position.Y;
        //    int maxY1 = a.MaxY + (int)a.Position.Y;

        //    int minX2 = b.MinX + (int)b.Position.X;
        //    int maxX2 = b.MaxX + (int)b.Position.X;
        //    int minY2 = b.MinY + (int)b.Position.Y;
        //    int maxY2 = b.MaxY + (int)b.Position.Y;

        //    if (maxX1 < minX2 || minX1 > maxX2 ||
        //        maxY1 < minY2 || minY1 > maxY2)
        //        return false;

        //    Dictionary<Point,bool>.Enumerator enumer = a.HullTransformed.GetEnumerator();
        //    while(enumer.MoveNext())
        //    {
        //        Point point1 = enumer.Current.Key;
        //        int x1 = point1.X + (int)a.Position.X;
        //        int y1 = point1.Y + (int)a.Position.Y;

        //        if (x1 < minX2 || x1 > maxX2 ||
        //            y1 < minY2 || y1 > maxY2)
        //            continue;

        //        if (b.HullTransformed.ContainsKey(new Point(x1 - (int)b.Position.X, y1 - (int)b.Position.Y)))
        //            return true;
        //    }
        //    return false;
        //}

        //public static bool IntersectsPoint(Point a, GameObject b)
        //{

        //    if (b.UpdateHull)
        //        CollisionMath.UpdateHull(b);
 
        //    if (b.HullTransformed.ContainsKey(a))
        //            return true;

        //    return false;
        //}


        ///// <summary>
        ///// Determines the point of intersection between two line segments, 
        ///// as defined by four points.
        ///// </summary>
        ///// <param name="a">The first point on the first line segment.</param>
        ///// <param name="b">The second point on the first line segment.</param>
        ///// <param name="c">The first point on the second line segment.</param>
        ///// <param name="d">The second point on the second line segment.</param>
        ///// <param name="point">The output value with the interesection, if any.</param>
        ///// <remarks>The output parameter "point" is only valid
        ///// when the return value is true.</remarks>
        ///// <returns>True if intersecting, false otherwise.</returns>
        //public static bool LineLineIntersect(Vector2 a, Vector2 b, Vector2 c,
        //    Vector2 d, out Vector2 point)
        //{
        //    point = Vector2.Zero;

        //    double r, s;
        //    double denominator = (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

        //    // If the denominator in above is zero, AB & CD are colinear
        //    if (denominator == 0)
        //    {
        //        return false;
        //    }

        //    double numeratorR = (a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y);
        //    r = numeratorR / denominator;

        //    double numeratorS = (a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y);
        //    s = numeratorS / denominator;

        //    // non-intersecting
        //    if (r < 0 || r > 1 || s < 0 || s > 1)
        //    {
        //        return false;
        //    }

        //    // find intersection point
        //    point.X = (float)(a.X + (r * (b.X - a.X)));
        //    point.Y = (float)(a.Y + (r * (b.Y - a.Y)));

        //    return true;
        //}


        ///// <summary>
        ///// Determine if two circles intersect or contain each other.
        ///// </summary>
        ///// <param name="center1">The center of the first circle.</param>
        ///// <param name="radius1">The radius of the first circle.</param>
        ///// <param name="center2">The center of the second circle.</param>
        ///// <param name="radius2">The radius of the second circle.</param>
        ///// <returns>True if the circles intersect or contain one another.</returns>
        //public static bool CircleCircleIntersect(Vector2 center1, float radius1,
        //    Vector2 center2, float radius2)
        //{
        //    Vector2 line = center2 - center1;
        //    // we use LengthSquared to avoid a costly square-root call
        //    return (line.LengthSquared() <= (radius1 + radius2) * (radius1 + radius2));
        //}


        ///// <summary>
        ///// Determines if a circle and line segment intersect, and if so, how they do.
        ///// </summary>
        ///// <param name="center">The center of the circle.</param>
        ///// <param name="radius">The radius of the circle.</param>
        ///// <param name="rectangle">The rectangle.</param>
        ///// <param name="result">The result data for the collision.</param>
        ///// <returns>True if a collision occurs, provided for convenience.</returns>
        //public static bool CircleRectangleCollide(Vector2 center, float radius,
        //    Rectangle rectangle, ref CircleLineCollisionResult result)
        //{
        //    float xVal = center.X;
        //    if (xVal < rectangle.Left) xVal = rectangle.Left;
        //    if (xVal > rectangle.Right) xVal = rectangle.Right;

        //    float yVal = center.Y;
        //    if (yVal < rectangle.Top) yVal = rectangle.Top;
        //    if (yVal > rectangle.Bottom) yVal = rectangle.Bottom;

        //    Vector2 direction = new Vector2(center.X - xVal, center.Y - yVal);
        //    float distance = direction.Length();

        //    if ((distance > 0) && (distance < radius))
        //    {
        //        result.Collision = true;
        //        result.Distance = radius - distance;
        //        result.Normal = Vector2.Normalize(direction);
        //        result.Point = new Vector2(xVal, yVal);
        //    }
        //    else
        //    {
        //        result.Collision = false;
        //    }

        //    return result.Collision;
        //}


        ///// <summary>
        ///// Determines if a circle and line segment intersect, and if so, how they do.
        ///// </summary>
        ///// <param name="center">The center of the circle.</param>
        ///// <param name="radius">The radius of the circle.</param>
        ///// <param name="lineStart">The first point on the line segment.</param>
        ///// <param name="lineEnd">The second point on the line segment.</param>
        ///// <param name="result">The result data for the collision.</param>
        ///// <returns>True if a collision occurs, provided for convenience.</returns>
        //public static bool CircleLineCollide(Vector2 center, float radius,
        //    Vector2 lineStart, Vector2 lineEnd, ref CircleLineCollisionResult result)
        //{
        //    Vector2 AC = center - lineStart;
        //    Vector2 AB = lineEnd - lineStart;
        //    float ab2 = AB.LengthSquared();
        //    if (ab2 <= 0f)
        //    {
        //        return false;
        //    }
        //    float acab = Vector2.Dot(AC, AB);
        //    float t = acab / ab2;

        //    if (t < 0.0f)
        //        t = 0.0f;
        //    else if (t > 1.0f)
        //        t = 1.0f;

        //    result.Point = lineStart + t * AB;
        //    result.Normal = center - result.Point;

        //    float h2 = result.Normal.LengthSquared();
        //    float r2 = radius * radius;

        //    if ((h2 > 0) && (h2 <= r2))
        //    {
        //        result.Normal.Normalize();
        //        result.Distance = (radius - (center - result.Point).Length());
        //        result.Collision = true;
        //    }
        //    else
        //    {
        //        result.Collision = false;
        //    }

        //    return result.Collision;
        //}


        //#endregion
    }
}

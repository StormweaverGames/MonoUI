using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoUI
{
    /// <summary>
    /// Some basic GUI utilities
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Fixes the line spacing on a font to make it tighter
        /// </summary>
        /// <param name="font">The font to repair</param>
        public static void FixFont(this SpriteFont font)
        {
            font.LineSpacing = (int)(font.MeasureString(" ").Y / 1.5f); // Re-adjusts the font's line spacing
        }

        /// <summary>
        /// Wraps a peice of text to fit within a given width
        /// </summary>
        /// <param name="s">The string to wrap</param>
        /// <param name="font">The font used for measuring</param>
        /// <param name="width">The width to wrap to</param>
        /// <returns>A string formatted to fit within the width</returns>
        public static string Wrap(this string s, SpriteFont font, float width)
        {
            StringBuilder sb = new StringBuilder(); // Declare and define a StringBuilder to build the output with

            float lineWidth = 0.0f; // Declare and define the current width of the line of text
            float spaceWidth = font.MeasureString(" ").X; // Declare and define a variable to hold the width of the space character
            Vector2 wordSize; // Declare a variable to 

            s = s.Replace("\r", "\n"); // Replace the return character with the newline character
            s = s.Replace("\n", " \n "); // Spaces out the newline characters so they will be considered their own word
            string[] words = s.Split(new [] { ' '}); // Splits the text into words
            
            foreach (string word in words) // Iterate through each word
            {
                if (word != "\n") // If the word is not a newline
                {
                    wordSize = font.MeasureString(word); // measure the size of the word

                    if (lineWidth + wordSize.X < width) // If the width of the current line + the new text is smaller than the width
                    {
                        sb.Append(word + " "); // Append the word and a space character
                        lineWidth += wordSize.X + spaceWidth; // Add the width of the word to the cached line width

                        if (word.EndsWith("\n")) // if this word ends with a newline
                            lineWidth = 0; // Reset the line width counter
                    }
                    else // If the width of the current line + the new text is greater than the width
                    {
                        sb.Append("\n" + word + " "); // Append a newline, then the word
                        lineWidth = wordSize.X + spaceWidth; // Reset the line width to the width of the word
                    }
                }
                else // if the word is a newline
                {
                    sb.Append("\n"); // Append a newline
                    lineWidth = 0; // Reset the line width counter
                }
            }

            return sb.ToString(); // Return the built string
        }

        /// <summary>
        /// Counts the number of characters in a string
        /// </summary>
        /// <param name="source">The source text</param>
        /// <param name="search">The character to search for</param>
        /// <returns>The number of the given characters in the string</returns>
        public static int CountOf(this string source, char search)
        {
            int count = 0;
            foreach (char c in source)
                if (c == search) count++;
            return count;
        }

        /// <summary>
        /// Sets the first character of a string to uppercase
        /// </summary>
        /// <param name="s">The string to perform formatting on</param>
        /// <returns>A capitalized version of <i>s</i></returns>
        public static string Capitalize(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// Converts this point to a Vector2
        /// </summary>
        /// <param name="point">The point to convert</param>
        /// <returns>A Vector2 containing the point's data</returns>
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }

    /// <summary>
    /// Extends the graphics device with some basic, alebiet slow utilities
    /// </summary>
    public static class GraphicsExtensions
    {
        /// <summary>
        /// Fills a rectangle with this graphics device
        /// </summary>
        /// <param name="g">The graphics device to draw with</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color of the rectangle</param>
        public static void DrawRect(this GraphicsDevice g, Rectangle rect, Color color)
        {
            VertexPositionColor[] verts = new VertexPositionColor[]{
                new VertexPositionColor(new Vector3(rect.X, rect.Y, 0.5F), color),
                new VertexPositionColor(new Vector3(rect.Right - 1, rect.Y, 0.5F), color),
                new VertexPositionColor(new Vector3(rect.Right - 1, rect.Bottom - 1, 0.5F), color),
                new VertexPositionColor(new Vector3(rect.X, rect.Bottom - 1, 0.5F), color),
                new VertexPositionColor(new Vector3(rect.X, rect.Y, 0.5F), color)
            };

            g.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, verts, 0, 4);
        }

        /// <summary>
        /// Fills a rectangle with this graphics device
        /// </summary>
        /// <param name="g">The graphics device to draw with</param>
        /// <param name="minX">The minimum x coord to fill from</param>
        /// <param name="minY">The minimum y coord to fill from</param>
        /// <param name="maxX">The maximum x coord to fill to</param>
        /// <param name="maxY">The maximum y coord to fill to</param>
        /// <param name="color">The color of the rectangle</param>
        public static void DrawRect(this GraphicsDevice g, float minX, float minY, float maxX, float maxY, Color color)
        {
            VertexPositionColor[] verts = new VertexPositionColor[]{
                new VertexPositionColor(new Vector3(minX, minY, 0.5F), color),
                new VertexPositionColor(new Vector3(maxX - 1, minY, 0.5F), color),
                new VertexPositionColor(new Vector3(maxX - 1, maxY - 1, 0.5F), color),
                new VertexPositionColor(new Vector3(minX, maxY - 1, 0.5F), color),
                new VertexPositionColor(new Vector3(minX, minY, 0.5F), color)
            };

            g.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, verts, 0, 4);
        }

        /// <summary>
        /// Draws a line with this graphics device
        /// </summary>
        /// <param name="g">The graphics device to draw with</param>
        /// <param name="point1">The first point to draw from</param>
        /// <param name="point2">the point to draw to</param>
        /// <param name="color">The color of the rectangle</param>
        public static void DrawLine(this GraphicsDevice g, Vector2 point1, Vector2 point2, Color color)
        {
            VertexPositionColor[] verts = new VertexPositionColor[]{
                new VertexPositionColor(new Vector3(point1, 0.5F), color),
                new VertexPositionColor(new Vector3(point2, 0.5F), color)
            };

            g.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, verts, 0, 1);
        }

        /// <summary>
        /// Draws a line with this graphics device
        /// </summary>
        /// <param name="g">The graphics device to draw with</param>
        /// <param name="x">The x-coord of the point to draw from</param>
        /// <param name="y">The y-coord of the point to draw from</param>
        /// <param name="x2">The x-coord of the point to draw to</param>
        /// <param name="y2">The y-coord of the point to draw to</param>
        /// <param name="color">The color of the rectangle</param>
        public static void DrawLine(this GraphicsDevice g, float x, float y, float x2, float y2, Color color)
        {
            g.DrawLine(new Vector2(x, y), new Vector2(x2, y2), color);
        }
    }

    /// <summary>
    /// Represents the alignment of text in a GUI component that displays text
    /// </summary>
    public enum TextAlignment
    {
        /// <summary>
        /// The text is centred in the component
        /// </summary>
        Centred,
        /// <summary>
        /// The text sits at the top left corner of the component
        /// </summary>
        TopLeft,
        /// <summary>
        /// The text sits centred along the left edge of the component
        /// </summary>
        CentreLeft,
        /// <summary>
        /// The text sits at the bottom left corner of the component
        /// </summary>
        BottomLeft,
        /// <summary>
        /// The text sits centred along the top edge of the component
        /// </summary>
        CentreTop,
        /// <summary>
        /// The text sits centred along the bottom edge of the component
        /// </summary>
        CentreBottom,
        /// <summary>
        /// The text sits at the top right corner of the component
        /// </summary>
        TopRight,
        /// <summary>
        /// The text sits centred along the right edge of the component
        /// </summary>
        CentreRight,
        /// <summary>
        /// The text sits at the bottom right corner of the component
        /// </summary>
        BottomRight
    }
}


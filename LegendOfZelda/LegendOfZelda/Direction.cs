namespace LegendOfZelda
{
   public enum Direction
    {
        NONE,
        RIGHT,
        UP,
        LEFT,
        DOWN
    }
    public class DirectionUtil
    {
        public static string DirectionToTitleCase(Direction p_dir)
        {
            if (p_dir == Direction.RIGHT)
                return "Right";
            else if (p_dir == Direction.UP)
                return "Up";
            else if (p_dir == Direction.LEFT)
                return "Left";
            else if (p_dir == Direction.DOWN)
                return "Down";
            else
                return "None";
        }
    }
}

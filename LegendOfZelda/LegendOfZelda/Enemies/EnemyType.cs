namespace LegendOfZelda.Enemies
{
    public enum EnemyType { BLUE, RED }

    public class EnemyTypeResolver
    {
        public static string TypeToString(EnemyType p_type, string p_name)
        {
            return p_type == EnemyType.RED ? p_name + "Red" : p_name + "Blue";
        }
    }
}

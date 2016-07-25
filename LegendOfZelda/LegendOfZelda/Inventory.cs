namespace LegendOfZelda
{
    public class Inventory
    {
        private static Inventory instance;
        private Inventory() { }
        public static Inventory Instance
        {
            get
            {
                if (instance == null)
                    instance = new Inventory();
                return instance;
            }
        }

        public int keyCount = 0;
        public int rupeeCount = 0;
        public int bombCount = 0;
        public bool hasMap = false;
        public bool hasCompass = false;
        public bool hasSword = false;
        public bool hasBoomerang = false;
        public bool hasBow = false;
    }
}

namespace Core
{
    public static class enRandom
    {
        public static int Get()
        {
            return s_oRandom.Next();
        }

        public static int Get(int iMaxValue)
        {
            return s_oRandom.Next(iMaxValue);
        }

        public static int Get(int iMinValue, int iMaxValue)
        {
            return s_oRandom.Next(iMinValue, iMaxValue);
        }

        private static System.Random s_oRandom = new System.Random();
    }
}
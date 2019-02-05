namespace ResourcesComparer.Calculation
{
    using System.Linq;

    class TanimotoStringComparer
    {
        /// <summary>
        /// without splitting by word - lets look how it works for us
        /// </summary>
        /// <param name="s1">compared string 1</param>
        /// <param name="s2">compared string 2</param>
        /// <returns>coeficient</returns>
        public static double Tanimoto(string s1, string s2, double lenghtDifferenceCoef)
        {
            return (TanimotoLeft(s2, s1, lenghtDifferenceCoef) + TanimotoLeft(s1, s2, lenghtDifferenceCoef)) / 2.0;
        }

        /// <summary>
        /// without splitting by word - lets look how it works for us
        /// </summary>
        /// <param name="firstString">compared string 1</param>
        /// <param name="secondString">compared string 2</param>
        /// <returns>coeficient</returns>
        private static double TanimotoLeft(string firstString, string secondString, double lenghtDifferenceCoef)
        {
            firstString = firstString.ToLower();
            secondString = secondString.ToLower();
            var firstLenght = firstString.Length;
            var secondLenght = secondString.Length;

            if (firstLenght > secondLenght * lenghtDifferenceCoef || secondLenght > firstLenght * lenghtDifferenceCoef)
            {
                return 0.0;
            }

            var commonSymbols = 0.0;

            foreach (char c1 in firstString)
            {
                if (secondString.Contains(c1))
                {
                    commonSymbols++;
                }
            }

            return commonSymbols / (firstLenght + secondLenght - commonSymbols);
        }

        private static bool IsTanimotoNear(string s1, string s2, double targetCoeficient, double lenghtDifferenceCoef)
        {
            return Tanimoto(s1, s2, lenghtDifferenceCoef) > targetCoeficient;
        }
    }
}

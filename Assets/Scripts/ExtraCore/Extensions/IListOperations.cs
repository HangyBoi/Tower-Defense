using System.Collections.Generic;

namespace Core.Extensions
{
    /// <summary>
    /// Operations methods for ILists
    /// </summary>
    public static class IListOperations
    {
        /// <summary>
        /// Goes to the next element of the list
        /// </summary>
        /// <param name="elements">An <see cref="System.Collections.Generic.IList{T}" /> of elements to choose from</param>
        /// <param name="currentIndex">The current index to be changed via reference</param>
        /// <param name="wrap">If the list should wrap</param>
        /// <typeparam name="T">The generic parameter for the list</typeparam>
        /// <returns>True if there is a next item in the list</returns>
        public static bool Next<T>(this IList<T> elements, ref int currentIndex, bool wrap = false)
        {
            int count = elements.Count;
            if (count == 0)
            {
                return false;
            }

            currentIndex++;

            if (currentIndex >= count)
            {
                if (wrap)
                {
                    currentIndex = 0;
                    return true;
                }
                currentIndex = count - 1;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Goes to the previous element of the list
        /// </summary>
        /// <param name="elements">An <see cref="System.Collections.Generic.IList{T}" /> of elements to choose from</param>
        /// <param name="currentIndex">The current index to be changed via reference</param>
        /// <param name="wrap">If the list should wrap</param>
        /// <typeparam name="T">The generic parameter for the list</typeparam>
        /// <returns>True if there is a previous item in the list</returns>
        public static bool Prev<T>(this IList<T> elements, ref int currentIndex, bool wrap = false)
        {
            int count = elements.Count;
            if (count == 0)
            {
                return false;
            }

            currentIndex--;

            if (currentIndex < 0)
            {
                if (wrap)
                {
                    currentIndex = count - 1;
                    return true;
                }
                currentIndex = 0;
                return false;
            }

            return true;
        }
    }
}
namespace Blog.Helpers;

public static class Pagination
{
    public const int PAGE_SIZE = 2;

    public static IEnumerable<int> GetPageNumbers(int pageNumber, int pageCount)
    {
        if (pageCount < 5)
        {
            for (int i = 1; i <= pageCount; i++)
                yield return i;
        }
        else
        {
            int midPoint = pageNumber < 3
                ? 3
                : pageNumber > pageCount - 2 ? pageCount - 2 : pageNumber;

            int lowerBound = midPoint - 2;
            int upperBound = midPoint + 2;

            if (lowerBound != 1)
            {
                yield return 1;

                if (lowerBound - 1 > 1)
                {
                    yield return -1;
                }
            }

            for (int i = lowerBound; i <= upperBound; i++)
            {
                yield return i;
            }

            if (upperBound != pageCount)
            {
                if (pageCount - upperBound > 1)
                {
                    yield return -1;
                }

                yield return pageCount;
            }
        }
    }
}

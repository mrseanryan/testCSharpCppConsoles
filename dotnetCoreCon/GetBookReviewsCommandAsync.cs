using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class GetBookReviewsCommandAsync
{
    public IEnumerable<string> Execute()
    {
        var userId = GetUserId();

        var userBooks = GetBooksForUser(userId);

        var reviews = userBooks.Select(b => GetBookReviewFor(b, userId));

        return reviews;
    }

    private string GetBookReviewFor(string book, string userId)
    {
        return $"book {book} reviewed by {userId}";
    }

    private IEnumerable<string> GetBooksForUser(string userId)
    {
        return new[] { "b1", "b2", "b3" };
    }

    private string GetUserId()
    {
        return "uid-1";
    }

    public async Task<IEnumerable<string>> ExecuteAsync()
    {
        var userId = await GetUserIdAsync();

        var userBooks = await GetBooksForUserAsync(userId);

        // warning: lazy execution - complicates behavior with async
        var reviewTasks = userBooks.Select(b => GetBookReviewForAsync(b, userId));

        return await Task.WhenAll(reviewTasks);
    }

    private async Task<string> GetUserIdAsync()
    {
        ThreadUtil.DumpThreadId("GetUserIdAsync - 1");
        var userId = await Task.Run(() => GetUserId())
            .ConfigureAwait(true) // IS same thread - xxx does not work?! - because project is not desktop UI??
                ;
        ThreadUtil.DumpThreadId("GetUserIdAsync - 2");

        return userId;
    }

    private async Task<IEnumerable<string>> GetBooksForUserAsync(string userId)
    {
        ThreadUtil.DumpThreadId("GetBooksForUserAsync - 1");
        await Task.Delay(250) // NOT Thread.Sleep()
            .ConfigureAwait(false) // not same thread -> performance
            ;
        ThreadUtil.DumpThreadId("GetBooksForUserAsync - 2");

        return GetBooksForUser(userId);
    }

    private async Task<string> GetBookReviewForAsync(string book, string userId)
    {
        ThreadUtil.DumpThreadId("GetBookReviewForAsync - 1");
        await Task.Delay(250) // NOT Thread.Sleep()
            .ConfigureAwait(false) // not same thread -> performance
            ;
        ThreadUtil.DumpThreadId("GetBookReviewForAsync - 2");

        return GetBookReviewFor(book, userId);
    }
}

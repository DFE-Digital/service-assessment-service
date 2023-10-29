using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Services.Book;

public class DummyInMemoryBookingRequestReadService : IBookingRequestReadService
{

    private readonly IDummyDataStore _dummyDataStore;

    public DummyInMemoryBookingRequestReadService(IDummyDataStore dummyDataStore)
    {
        _dummyDataStore = dummyDataStore;
    }

    public async Task<IEnumerable<IncompleteBookingRequest>> GetAllAssessments()
    {
        // return from dummy data store
        return await _dummyDataStore.GetAllAssessments();
    }

    public async Task<IncompleteBookingRequest?> GetByIdAsync(BookingRequestId id)
    {
        // return from dummy data store, if exists
        return await _dummyDataStore.GetByIdAsync(id);

    }

    public async Task<List<DateOnly>> AvailableReviewDates(BookingRequestId id)
    {
        var request = await GetByIdAsync(id);
        if (request is null)
        {
            throw new ArgumentException($"Booking request with ID {id} not found");
        }

        var availableDates = new List<DateOnly>();
        var startOfThisWeek = DateOnly.FromDateTime(DateTime.Today.AddDays((-(int) DateTime.Today.DayOfWeek) + 1));
        
        var endDate = request.EndDate;
        if (endDate is null)
        {
            // If unknown end date, return dates within the next 5-10weeks
            for (var i = 5; i <= 10; i++)
            {
                var date = startOfThisWeek.AddDays(i * 7);
                availableDates.Add(date);
            }
        }
        else
        {
            // If known end date, return dates within the next 5-10weeks, starting from the monday of the week of the end date
            var endDateStartOfWeek = endDate.Value.AddDays((-(int) endDate.Value.DayOfWeek) + 1);
            // "week beginning" dates that the mondays of each week, for the range 5-10 weeks from the end date 
            for (var i = 0; i <= 5; i++)
            {
                var date = endDateStartOfWeek.AddDays(i * 7);
                availableDates.Add(date);
            }
        }

        // TODO: In the actual API implementation, the DateTime.Today should be injected or similar as a "reference date" 
        var earliestPermittedReviewDate = DateOnly.FromDateTime(startOfThisWeek.ToDateTime(TimeOnly.MinValue).AddDays(5 * 7));
        
        // filter out any dates that are before the earliest permitted review date
        availableDates = availableDates.Where(d => d >= earliestPermittedReviewDate).ToList();
        
        return availableDates;
    }
}

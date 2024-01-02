using Bogus;
using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;
using ServiceAssessmentService.WebApp.Services.Lookups;
using Person = ServiceAssessmentService.WebApp.Models.Person;

namespace ServiceAssessmentService.WebApp.Services.Book;

public class DummyDataStore : IDummyDataStore
{
    // Static, to allow this to be DI'd with request scope
    private static readonly Dictionary<BookingRequestId, IncompleteBookingRequest> _incompleteBookingRequests = new();

    private readonly ILookupsReadService _lookupsReadService;
    public DummyDataStore(ILookupsReadService lookupsReadService)
    {
        _lookupsReadService = lookupsReadService;
    }

    private async Task InitialiseIfEmptyAsync()
    {
        if (!_incompleteBookingRequests.Any())
        {
            await Seed();
        }
    }

    public async Task Seed()
    {
        var availableAssessmentTypes = (await _lookupsReadService.GetAssessmentTypes())
            // .Where(x => x.IsEnabled)
            ;
        var availablePhases = (await _lookupsReadService.GetPhases())
            // .Where(x => x.IsEnabled)
            ;
        var availablePortfolios = (await _lookupsReadService.GetPortfolioOptions())
            // .Where(x => x.IsEnabled)
            ;

        var x = new Faker<IncompleteBookingRequest>()
                .StrictMode(true)
                .UseSeed(100)
                .CustomInstantiator(f => new IncompleteBookingRequest(new BookingRequestId(f.Random.Guid())))
                .RuleFor(o => o.RequestId, f => new BookingRequestId(f.Random.Guid()))
                // .RuleFor(o => o.AssessmentType, f => f.PickRandom(availableAssessmentTypes).OrNull(f, 0.1f))
                // .RuleFor(o => o.PhaseConcluding, f => f.PickRandom(availablePhases).OrNull(f, 0.1f))
                .RuleFor(o => o.AssessmentType, f => f.PickRandom(availableAssessmentTypes))
                .RuleFor(o => o.PhaseConcluding, f => f.PickRandom(availablePhases))
                .RuleFor(o => o.Name, f => string.Join(" ", f.Commerce.ProductName().OrNull(f, 0.2f)))
                .RuleFor(o => o.Description, f => f.Commerce.ProductDescription().OrNull(f, 0.5f))
                .RuleFor(o => o.IsProjectCodeKnown, f => f.Random.Bool(0.5f))
                .RuleFor(o => o.ProjectCode, (f, current) => current.IsProjectCodeKnown == true ? f.Random.AlphaNumeric(6).ToUpperInvariant() : null)
                .RuleFor(o => o.StartDate, f => f.Date.PastDateOnly().OrNull(f, 0.5f))
                .RuleFor(o => o.StartDateDay, (f, current) => current.StartDate?.Day)
                .RuleFor(o => o.StartDateMonth, (f, current) => current.StartDate?.Month)
                .RuleFor(o => o.StartDateYear, (f, current) => current.StartDate?.Year)
                .RuleFor(o => o.IsEndDateKnown, f => f.Random.Bool(0.5f))
                .RuleFor(o => o.EndDate, (f, current) => current.IsEndDateKnown == true ? f.Date.FutureDateOnly(refDate: current.StartDate).OrNull(f, 0.5f) : null)
                .RuleFor(o => o.EndDateDay, (f, current) => current.EndDate?.Day)
                .RuleFor(o => o.EndDateMonth, (f, current) => current.EndDate?.Month)
                .RuleFor(o => o.EndDateYear, (f, current) => current.EndDate?.Year)
                // TODO: Review dates should be based on the end date (or 5-10 weeks into the future) // TODO: Should be the "Monday" for a "week beginning" date
                .RuleFor(o => o.ReviewDates, f =>
                {
                    var dates = Enumerable.Range(1, f.Random.Int(0, 5)).Select(x => f.Date.FutureDateOnly()).ToList();
                    dates.Add(new DateOnly(2024, 1, 1));
                    return dates;
                })
                .RuleFor(o => o.Portfolio, f => f.PickRandom(availablePortfolios).OrNull(f, 0.4f))
                .RuleFor(o => o.DeputyDirector, f =>
                {
                    if (f.Random.Bool(0.2f))
                    {
                        return null;
                    }

                    var firstName = f.Name.FirstName();
                    var lastName = f.Name.LastName();

                    var person = new Person();
                    person.Name = $"{firstName} {lastName}";
                    person.Email = f.Internet.Email(firstName, lastName);

                    return person;
                })
            ;

        var incompleteBookingRequests = x.Generate(10);
        foreach (var incompleteBookingRequest in incompleteBookingRequests)
        {
            _incompleteBookingRequests.Add(incompleteBookingRequest.RequestId, incompleteBookingRequest);
        }
    }

    public async Task<IncompleteBookingRequest> CreateNewBookingRequest(Phase? concludingPhase = null, AssessmentType? assessmentType = null)
    {
        await InitialiseIfEmptyAsync();

        var incompleteBookingRequest = new IncompleteBookingRequest(BookingRequestId.New());
        incompleteBookingRequest.AssessmentType ??= assessmentType;
        incompleteBookingRequest.PhaseConcluding ??= concludingPhase;

        _incompleteBookingRequests.Add(incompleteBookingRequest.RequestId, incompleteBookingRequest);

        return incompleteBookingRequest;
    }

    public async Task<IEnumerable<IncompleteBookingRequest>> GetAllAssessments()
    {
        await InitialiseIfEmptyAsync();

        return _incompleteBookingRequests.Values;
    }

    public async Task<IncompleteBookingRequest?> GetByIdAsync(BookingRequestId id)
    {
        await InitialiseIfEmptyAsync();

        return _incompleteBookingRequests.TryGetValue(id, out var incompleteBookingRequest)
            ? incompleteBookingRequest
            : null;
    }

    public async Task<IncompleteBookingRequest> Put(BookingRequestId id, IncompleteBookingRequest incompleteBookingRequest)
    {
        await InitialiseIfEmptyAsync();

        _incompleteBookingRequests[id] = incompleteBookingRequest;
        return incompleteBookingRequest;
    }

}

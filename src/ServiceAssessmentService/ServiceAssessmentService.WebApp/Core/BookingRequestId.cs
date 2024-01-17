using StronglyTypedIds;

namespace ServiceAssessmentService.WebApp.Core;

[StronglyTypedId(converters: StronglyTypedIdConverter.TypeConverter | StronglyTypedIdConverter.SystemTextJson)]
public partial struct BookingRequestId
{
}

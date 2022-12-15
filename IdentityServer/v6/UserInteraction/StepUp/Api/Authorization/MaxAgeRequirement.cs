using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization;

public class MaxAgeRequirement : IAuthorizationRequirement
{
    public MaxAgeRequirement(TimeSpan maxAge)
    {
        MaxAge = maxAge;
    }

    public TimeSpan MaxAge { get; }
}
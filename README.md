# SEEK's Job Ad Posting API Client

## What It Is
1. A .NET version of SEEK's Job Ad Posting API Client which can be installed via [NuGet](https://www.nuget.org/packages/SEEK.AdPostingApi.Client).
2. It comprises of the following three projects:
    1. *SEEK.AdPostingApi.Client* - Source code of the API client
    2. *SEEK.AdPostingApi.SampleConsumer* - Example code using the API client to make requests to the Job Ad Posting API
    3. *SEEK.AdPostingApi.Client.Tests* - Contract (PACT) tests between the API client and the Job Ad Posting API.

## What It Does
1. Exchanges the OAuth 2.0 credentials (client key and client secret) for an OAuth 2.0 access token.
2. Using the OAuth 2.0 access token:
    1. Retrieves the API links from the Job Ad Posting API.
    2. Depending on the operation (create, update, retrieve or expire):
        1. Builds the appropriate API link for the operation.
        2. Makes the appropriate request to the API.

## Resources

1. [Job Ad Posting API Documentation](https://devportal.seek.com.au)
2. [NuGet Package](https://www.nuget.org/packages/SEEK.AdPostingApi.Client)
3. [Release Notes](https://github.com/SEEK-Jobs/ad-posting-api-client/releases)
4. [Contract (PACT) Between the API Client and the Job Ad Posting API](https://github.com/SEEK-Jobs/ad-posting-api-client/blob/master/pact/README.md)

## Usage

### Install the API Client
Via NuGet with `Install-Package SEEK.AdPostingApi.Client`

### Initializing the API Client
To initialize a client, the following values are needed:
* **Client Key** [Required]: the client key for getting an OAuth 2 access token
* **Client Secret** [Required]: the client secret for getting an OAuth 2 access token
* **Environment** [Optional]: the environment to which your consumer will integrate with, either "Integration" or "Production" can be supplied. Without supplying anything, "Production" will be used by default.
* **AdPostingApiBaseUrl** [Optional]: the URL of the Job Ad Posting API. Without supplying anything, the Production URL will be used by default.

### Example Code: Construct an API Client to Create a Job Advertisement

```c#
IAdPostingApiClient postingClient = new AdPostingApiClient("<client id>", "<client secret>", Environment.Integration);

var ad = new Advertisement
{
    CreationId = "Sample Consumer 20151001 114732 1234567",
    ThirdParties = new ThirdParties { AdvertiserId = "Advertiser Id" },
    JobTitle = "A Job Title",
    JobSummary = "Job summary of the job ad",
    AdvertisementDetails = "Experience Required",
    AdvertisementType = AdvertisementType.Classic,
    WorkType = WorkType.Casual,
    Salary = new Salary
    {
        Type = SalaryType.HourlyRate,
        Minimum = 20,
        Maximum = 24
    },
    Location = new Location
    {
        Id = "Melbourne",
        AreaId = "MelbourneNorthernSuburbs"
    },
    SubclassificationId = "AerospaceEngineering"
};

AdvertisementResource advertisement = await postingClient.CreateAdvertisementAsync(ad);
```

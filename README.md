# SEEK's Job Ad Posting API Client
This is a .Net version of SEEK's Job Ad Posting API Client. It composes of the following three projects:
* SEEK.AdPostingApi.Client: the source code of the client
* SEEK.AdPostingApi.SampleConsumer: the sample code using the client to make requests against Ad Posting API
* SEEK.AdPostingApi.SampleConsumer.Tests: the pact tests for Ad Posting API using the client


## Build your consumer

### Install the client
Via Nuget with [Install-Package SEEK.AdPostingApi -IncludePrerelease](http://www.nuget.org/packages/TODO)

### Configure values
The client needs the following configuration values:
* **Client Key** [Required]: the client key for getting an OAuth 2 access token
* **Client Secret** [Required]: the client secret for getting an OAuth 2 access token
* **Environment** [Optional]: the environment to which your consumer will integrate with, either "Integration" or "Production" can be supplied. Without supplying anything, "Production" will be used by default.
* **AdPostingApiBaseUrl** [Optional]: the URL of the Job Ad Posting API. Without supplying anything, "Production" URL will be used by default.

The configuration can be done by JSON file as the "SEEK.AdPostingApi.SampleConsumer" project shows. You can also use your own mechanism like setting them as environmental variable.

### Construct a client to make API calls
A job ad posting request may look something like this.

```c#
            IAdPostingApiClient postingClient = new AdPostingApiClient(configuration.ClientKey, configuration.ClientSecret, configuration.Environment);

            var ad = new Advertisement
            {
                AdvertiserId = "Advertiser Id",
                JobTitle = "A Job Title",
                JobSummary = "Job summary of the job ad",
                AdvertisementDetails = "Experience Required",
                AdvertisementType = AdvertisementType.Classic,
                WorkType = WorkType.Casual,
                SalaryType = SalaryType.HourlyRate,
                LocationId = "1002",
                SubclassificationId = "6227",
                SalaryMinimum = 20,
                SalaryMaximum = 24
            };

            Uri jobAdLink = await postingClient.CreateAdvertisementAsync(ad);
```

## What does the client do
Take the posting request as an example, when the "CreateAdvertisementAsync" method is called, the following actions will be taken:
 1. It takes the OAuth 2.0 credentials (client key and client secret) to obtain an OAuth2 access token
 2. It takes the OAuth2 access token to get all the available links and identify which one to use to create a job ad
 3. It then takes the OAuth2 access token, the link (from step 2) and the job ad object to make a request to the create the job ad 

## API document
* [SEEK OAuth 2](http://docs.oauth2seek.apiary.io/#)
* [Job Ad Posting API](http://docs.jobadposting.apiary.io/#)

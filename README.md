# SEEK's Job Ad Posting API Client
This is a .Net version of SEEK's Job Ad Posting API Client. It composes of the following three projects:
* SEEK.AdPostingApi.Client: the source code of the client
* SEEK.AdPostingApi.SampleConsumer: the sample code using the client to make requests against Ad Posting API
* SEEK.AdPostingApi.SampleConsumer.Tests: the pact tests for Ad Posting API using the client


## Build your consumer

### Install the client
Via Nuget with [Install-Package SEEK.AdPostingApi.Client -Pre](https://www.nuget.org/packages/SEEK.AdPostingApi.Client/)

### Input values
To initialize a client, the following values are needed:
* **Client Key** [Required]: the client key for getting an OAuth 2 access token
* **Client Secret** [Required]: the client secret for getting an OAuth 2 access token
* **Environment** [Optional]: the environment to which your consumer will integrate with, either "Integration" or "Production" can be supplied. Without supplying anything, "Production" will be used by default.
* **AdPostingApiBaseUrl** [Optional]: the URL of the Job Ad Posting API. Without supplying anything, "Production" URL will be used by default.

You can choose your own mechanism to supply the above values, like JSON file or environmental variables.

### Construct a client to make API calls
A job ad posting request may look something like this.

```c#
            IAdPostingApiClient postingClient = new AdPostingApiClient("<client id>", "<client secret>", Environment.Integration);

            var ad = new Advertisement
            {
                CreationId = "Sample Consumer 20151001 114732 1234567",
                AdvertiserId = "Advertiser Id",
                JobTitle = "A Job Title",
                JobSummary = "Job summary of the job ad",
                AdvertisementDetails = "Experience Required",
                AdvertisementType = AdvertisementType.Classic,
                WorkType = WorkType.Casual,
                Salary = new Salary() { Type = SalaryType.HourlyRate,
                                        Minimum = 20,
                                        Maximum = 24},
                LocationId = "1002",
                SubclassificationId = "6227",
               
            };

            AdvertisementResource advertisement = await postingClient.CreateAdvertisementAsync(ad);
```

## What does the client do
Take the posting request as an example, when the "CreateAdvertisementAsync" method is called, the following actions will be taken:
 1. It takes the OAuth 2.0 credentials (client key and client secret) to obtain an OAuth2 access token
 2. It takes the OAuth2 access token to get all the available links and identify which one to use to create a job ad
 3. It then takes the OAuth2 access token, the link (from step 2) and the job ad object to make a request to the create the job ad 
 4. Other operations like update, retrieve, and expire job ad are also available.

## API document
* [SEEK OAuth 2](http://docs.oauth2seek.apiary.io/#)
* [Job Ad Posting API](http://docs.adposting.apiary.io/#)

# A pact between Ad Posting API Client and Ad Posting Logo API

## Requests from Ad Posting API Client to Ad Posting Logo API

* [A GET logos request to retrieve all logos](#a_GET_logos_request_to_retrieve_all_logos_given_There_are_no_logos_for_any_advertiser_related_to_the_requestor) given There are no logos for any advertiser related to the requestor

* [A GET logos request to retrieve all logos](#a_GET_logos_request_to_retrieve_all_logos_given_There_are_multiple_logos_for_multiple_advertisers_related_to_the_requestor) given There are multiple logos for multiple advertisers related to the requestor

* [A GET logos request to retrieve all logos for an advertiser](#a_GET_logos_request_to_retrieve_all_logos_for_an_advertiser_given_There_are_multiple_logos_for_multiple_advertisers_related_to_the_requestor) given There are multiple logos for multiple advertisers related to the requestor

* [A GET logos request to retrieve all logos for an advertiser not related to requestor](#a_GET_logos_request_to_retrieve_all_logos_for_an_advertiser_not_related_to_requestor)

* [A GET logos request to retrieve all logos for an advertiser that doesn't exist](#a_GET_logos_request_to_retrieve_all_logos_for_an_advertiser_that_doesn_t_exist)

* [A GET logos request to retrieve all logos with invalid request field values](#a_GET_logos_request_to_retrieve_all_logos_with_invalid_request_field_values_given_There_are_multiple_logos_for_multiple_advertisers_related_to_the_requestor) given There are multiple logos for multiple advertisers related to the requestor

## Interactions from Ad Posting API Client to Ad Posting Logo API

<a name="a_GET_logos_request_to_retrieve_all_logos_given_There_are_no_logos_for_any_advertiser_related_to_the_requestor"></a>
Given **there are no logos for any advertiser related to the requestor**, upon receiving **a GET logos request to retrieve all logos** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/logo",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Accept": "application/vnd.seek.logo-list+json; version=1; charset=utf-8, application/vnd.seek.logo-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting Logo API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.logo-list+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "_embedded": {
      "logos": []
    },
    "_links": {
      "self": {
        "href": "/logo"
      }
    }
  }
}
```

<a name="a_GET_logos_request_to_retrieve_all_logos_given_There_are_multiple_logos_for_multiple_advertisers_related_to_the_requestor"></a>
Given **there are multiple logos for multiple advertisers related to the requestor**, upon receiving **a GET logos request to retrieve all logos** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/logo",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.logo-list+json; version=1; charset=utf-8, application/vnd.seek.logo-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting Logo API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.logo-list+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "_embedded": {
      "logos": [
        {
          "logoId": "18475",
          "advertiserId": "456",
          "name": "The red logo",
          "updatedDateTime": "2018-01-03T22:55:55+11:00",
          "state": "Active",
          "_links": {
            "view": {
              "href": "https://www.seek.com.au/logos/jobseeker/thumbnail/18475.png"
            }
          }
        },
        {
          "logoId": "781312",
          "advertiserId": "456",
          "name": "The blue logo",
          "updatedDateTime": "2016-08-23T21:55:55+10:00",
          "state": "Active",
          "_links": {
            "view": {
              "href": "https://www.seek.com.au/logos/jobseeker/thumbnail/781312.png"
            }
          }
        },
        {
          "logoId": "129301",
          "advertiserId": "456",
          "name": "The transparent logo",
          "updatedDateTime": "2015-11-11T22:55:55+11:00",
          "state": "Active",
          "_links": {
            "view": {
              "href": "https://www.seek.com.au/logos/jobseeker/thumbnail/129301.jpg"
            }
          }
        },
        {
          "logoId": "129",
          "advertiserId": "3214",
          "name": "Old logo",
          "updatedDateTime": "2017-03-30T22:55:55+11:00",
          "state": "Inactive",
          "_links": {
            "view": {
              "href": "https://www.seek.com.au/logos/jobseeker/thumbnail/129.jpg"
            }
          }
        },
        {
          "logoId": "5818341",
          "advertiserId": "3214",
          "name": "New logo",
          "updatedDateTime": "2017-05-05T21:55:55+10:00",
          "state": "Active",
          "_links": {
            "view": {
              "href": "https://www.seek.com.au/logos/jobseeker/thumbnail/5818341.png"
            }
          }
        }
      ]
    },
    "_links": {
      "self": {
        "href": "/logo"
      }
    }
  }
}
```

<a name="a_GET_logos_request_to_retrieve_all_logos_for_an_advertiser_given_There_are_multiple_logos_for_multiple_advertisers_related_to_the_requestor"></a>
Given **there are multiple logos for multiple advertisers related to the requestor**, upon receiving **a GET logos request to retrieve all logos for an advertiser** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/logo",
  "query": "advertiserId=456",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.logo-list+json; version=1; charset=utf-8, application/vnd.seek.logo-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting Logo API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.logo-list+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "_embedded": {
      "logos": [
        {
          "logoId": "18475",
          "advertiserId": "456",
          "name": "The red logo",
          "updatedDateTime": "2018-01-03T22:55:55+11:00",
          "state": "Active",
          "_links": {
            "view": {
              "href": "https://www.seek.com.au/logos/jobseeker/thumbnail/18475.png"
            }
          }
        },
        {
          "logoId": "781312",
          "advertiserId": "456",
          "name": "The blue logo",
          "updatedDateTime": "2016-08-23T21:55:55+10:00",
          "state": "Active",
          "_links": {
            "view": {
              "href": "https://www.seek.com.au/logos/jobseeker/thumbnail/781312.png"
            }
          }
        },
        {
          "logoId": "129301",
          "advertiserId": "456",
          "name": "The transparent logo",
          "updatedDateTime": "2015-11-11T22:55:55+11:00",
          "state": "Active",
          "_links": {
            "view": {
              "href": "https://www.seek.com.au/logos/jobseeker/thumbnail/129301.jpg"
            }
          }
        }
      ]
    },
    "_links": {
      "self": {
        "href": "/logo?advertiserId=456"
      }
    }
  }
}
```

<a name="a_GET_logos_request_to_retrieve_all_logos_for_an_advertiser_not_related_to_requestor"></a>
Upon receiving **a GET logos request to retrieve all logos for an advertiser not related to requestor** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/logo",
  "query": "advertiserId=456",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Accept": "application/vnd.seek.logo-list+json; version=1; charset=utf-8, application/vnd.seek.logo-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting Logo API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.logo-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Forbidden",
    "errors": [
      {
        "code": "RelationshipError"
      }
    ]
  }
}
```

<a name="a_GET_logos_request_to_retrieve_all_logos_for_an_advertiser_that_doesn_t_exist"></a>
Upon receiving **a GET logos request to retrieve all logos for an advertiser that doesn't exist** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/logo",
  "query": "advertiserId=654321",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.logo-list+json; version=1; charset=utf-8, application/vnd.seek.logo-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting Logo API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.logo-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Forbidden",
    "errors": [
      {
        "code": "RelationshipError"
      }
    ]
  }
}
```

<a name="a_GET_logos_request_to_retrieve_all_logos_with_invalid_request_field_values_given_There_are_multiple_logos_for_multiple_advertisers_related_to_the_requestor"></a>
Given **there are multiple logos for multiple advertisers related to the requestor**, upon receiving **a GET logos request to retrieve all logos with invalid request field values** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/logo",
  "query": "advertiserId=asdf",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.logo-list+json; version=1; charset=utf-8, application/vnd.seek.logo-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting Logo API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.logo-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Forbidden",
    "errors": [
      {
        "message": "Invalid value 'asdf' in field 'AdvertiserPublicId'",
        "code": "InvalidValue"
      }
    ]
  }
}
```


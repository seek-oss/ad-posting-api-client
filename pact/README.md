### A pact between Ad Posting API Client and Ad Posting API

#### Requests from Ad Posting API Client to Ad Posting API

* [A GET advertisement request](#a_GET_advertisement_request_given_There_is_a_standout_advertisement_with_granular_location_data) given there is a standout advertisement with granular location data

* [A GET advertisement request](#a_GET_advertisement_request_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A GET advertisement request for a non-existent advertisement](#a_GET_advertisement_request_for_a_non-existent_advertisement)

* [A GET advertisement request for an advertisement of an advertiser not related to the requestor's account](#a_GET_advertisement_request_for_an_advertisement_of_an_advertiser_not_related_to_the_requestor&#39;s_account_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A GET advertisement request for an advertisement using a disabled requestor account](#a_GET_advertisement_request_for_an_advertisement_using_a_disabled_requestor_account_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A GET advertisements request for all advertisements](#a_GET_advertisements_request_for_all_advertisements_given_There_are_no_advertisements) given there are no advertisements

* [A GET advertisements request for first page of data](#a_GET_advertisements_request_for_first_page_of_data_given_A_page_size_of_3_with_more_than_1_page_of_data) given a page size of 3 with more than 1 page of data

* [A GET advertisements request for the first page of advertisements belonging to the advertiser](#a_GET_advertisements_request_for_the_first_page_of_advertisements_belonging_to_the_advertiser_given_A_page_size_of_3_with_more_than_1_page_of_data) given a page size of 3 with more than 1 page of data

* [A GET advertisements request for the last page of data](#a_GET_advertisements_request_for_the_last_page_of_data_given_A_page_size_of_3_with_more_than_1_page_of_data) given a page size of 3 with more than 1 page of data

* [A GET advertisements request for the second page of advertisements belonging to the advertiser](#a_GET_advertisements_request_for_the_second_page_of_advertisements_belonging_to_the_advertiser_given_A_page_size_of_3_with_more_than_1_page_of_data) given a page size of 3 with more than 1 page of data

* [A GET advertisements request to retrieve all advertisements for an advertiser that doesn't exist](#a_GET_advertisements_request_to_retrieve_all_advertisements_for_an_advertiser_that_doesn&#39;t_exist)

* [A GET advertisements request to retrieve all advertisements for the advertiser not related to requestor](#a_GET_advertisements_request_to_retrieve_all_advertisements_for_the_advertiser_not_related_to_requestor)

* [A GET index request that is unauthorised to retrieve API links](#a_GET_index_request_that_is_unauthorised_to_retrieve_API_links)

* [A GET index request to retrieve API links with an invalid access token](#a_GET_index_request_to_retrieve_API_links_with_an_invalid_access_token)

* [A GET index request to retrieve API links with Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6](#a_GET_index_request_to_retrieve_API_links_with_Bearer_a4f2aab5-5582-4ff0-b8f2-890d6146dbb6)

* [A GET index request to retrieve API links with Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e](#a_GET_index_request_to_retrieve_API_links_with_Bearer_b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e)

* [A HEAD advertisement request](#a_HEAD_advertisement_request_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A HEAD advertisement request for a non-existent advertisement](#a_HEAD_advertisement_request_for_a_non-existent_advertisement)

* [A HEAD advertisement request for an advertisement of an advertiser not related to the requestor's account](#a_HEAD_advertisement_request_for_an_advertisement_of_an_advertiser_not_related_to_the_requestor&#39;s_account_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A HEAD advertisement request for an advertisement using a disabled requestor account](#a_HEAD_advertisement_request_for_an_advertisement_using_a_disabled_requestor_account_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PATCH advertisement request to expire a job for an advertiser not related to the requestor's account](#a_PATCH_advertisement_request_to_expire_a_job_for_an_advertiser_not_related_to_the_requestor&#39;s_account_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PATCH advertisement request to expire a job using a disabled requestor account](#a_PATCH_advertisement_request_to_expire_a_job_using_a_disabled_requestor_account_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PATCH advertisement request to expire a job using invalid request content](#a_PATCH_advertisement_request_to_expire_a_job_using_invalid_request_content_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PATCH advertisement request to expire a non-existent advertisement](#a_PATCH_advertisement_request_to_expire_a_non-existent_advertisement)

* [A PATCH advertisement request to expire an advertisement](#a_PATCH_advertisement_request_to_expire_an_advertisement_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PATCH advertisement request to expire an advertisement](#a_PATCH_advertisement_request_to_expire_an_advertisement_given_There_is_an_expired_advertisement) given there is an expired advertisement

* [A POST advertisement request to create a job](#a_POST_advertisement_request_to_create_a_job_given_The_requestor&#39;s_account_is_disabled) given the requestor's account is disabled

* [A POST advertisement request to create a job ad with a questionnaire ID](#a_POST_advertisement_request_to_create_a_job_ad_with_a_questionnaire_ID)

* [A POST advertisement request to create a job ad with an invalid advertiser id](#a_POST_advertisement_request_to_create_a_job_ad_with_an_invalid_advertiser_id)

* [A POST advertisement request to create a job ad with duplicated names for template custom fields](#a_POST_advertisement_request_to_create_a_job_ad_with_duplicated_names_for_template_custom_fields)

* [A POST advertisement request to create a job ad with granular location](#a_POST_advertisement_request_to_create_a_job_ad_with_granular_location)

* [A POST advertisement request to create a job ad with invalid advertisement details](#a_POST_advertisement_request_to_create_a_job_ad_with_invalid_advertisement_details)

* [A POST advertisement request to create a job ad with invalid advertisement details and with 'CleanseJobAdDetails' processing option](#a_POST_advertisement_request_to_create_a_job_ad_with_invalid_advertisement_details_and_with_&#39;CleanseJobAdDetails&#39;_processing_option)

* [A POST advertisement request to create a job ad with invalid field values](#a_POST_advertisement_request_to_create_a_job_ad_with_invalid_field_values)

* [A POST advertisement request to create a job ad with invalid salary data](#a_POST_advertisement_request_to_create_a_job_ad_with_invalid_salary_data)

* [A POST advertisement request to create a job ad with required and optional field values](#a_POST_advertisement_request_to_create_a_job_ad_with_required_and_optional_field_values)

* [A POST advertisement request to create a job ad with required field values only](#a_POST_advertisement_request_to_create_a_job_ad_with_required_field_values_only)

* [A POST advertisement request to create a job ad with the same creation id 'CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184'](#a_POST_advertisement_request_to_create_a_job_ad_with_the_same_creation_id_&#39;CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184&#39;_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A POST advertisement request to create a job ad without a creation id](#a_POST_advertisement_request_to_create_a_job_ad_without_a_creation_id)

* [A POST advertisement request to create a job for an advertiser not related to the requestor's account](#a_POST_advertisement_request_to_create_a_job_for_an_advertiser_not_related_to_the_requestor&#39;s_account)

* [A POST advertisement request to expire an advertisement](#a_POST_advertisement_request_to_expire_an_advertisement_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PUT advertisement request](#a_PUT_advertisement_request_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PUT advertisement request for a non-existent advertisement](#a_PUT_advertisement_request_for_a_non-existent_advertisement)

* [A PUT advertisement request for advertisement with invalid advertisement details](#a_PUT_advertisement_request_for_advertisement_with_invalid_advertisement_details_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PUT advertisement request for advertisement with invalid advertisement details and with 'CleanseJobAdDetails' processing option](#a_PUT_advertisement_request_for_advertisement_with_invalid_advertisement_details_and_with_&#39;CleanseJobAdDetails&#39;_processing_option_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PUT advertisement request for advertisement with invalid field values](#a_PUT_advertisement_request_for_advertisement_with_invalid_field_values_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PUT advertisement request for advertisement with invalid salary data](#a_PUT_advertisement_request_for_advertisement_with_invalid_salary_data_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PUT advertisement request to update a job ad with a different advertiser from the one owning the job](#a_PUT_advertisement_request_to_update_a_job_ad_with_a_different_advertiser_from_the_one_owning_the_job_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PUT advertisement request to update a job ad with a questionnaire ID](#a_PUT_advertisement_request_to_update_a_job_ad_with_a_questionnaire_ID_given_There_is_a_standout_advertisement_with_maximum_data_and_a_questionnaire_ID) given there is a standout advertisement with maximum data and a questionnaire ID

* [A PUT advertisement request to update a job for an advertiser not related to the requestor's account](#a_PUT_advertisement_request_to_update_a_job_for_an_advertiser_not_related_to_the_requestor&#39;s_account_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PUT advertisement request to update a job using a disabled requestor account](#a_PUT_advertisement_request_to_update_a_job_using_a_disabled_requestor_account_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

* [A PUT advertisement request to update an expired advertisement](#a_PUT_advertisement_request_to_update_an_expired_advertisement_given_There_is_an_expired_advertisement) given there is an expired advertisement

* [A PUT advertisement request to update granular location](#a_PUT_advertisement_request_to_update_granular_location_given_There_is_a_standout_advertisement_with_maximum_data) given there is a standout advertisement with maximum data

#### Interactions

<a name="a_GET_advertisement_request_given_There_is_a_standout_advertisement_with_granular_location_data"></a>
Given **there is a standout advertisement with granular location data**, upon receiving **a GET advertisement request** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Processing-Status": "Completed",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "granularLocation": {
      "country": "Australia",
      "state": "Victoria",
      "city": "Melbourne",
      "postCode": "3000"
    },
    "screenId": 1,
    "id": "8e2fde50-bc5f-4a12-9cfb-812e50500184",
    "expiryDate": "2015-11-06T21:19:00Z",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184"
      },
      "view": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184/view"
      }
    }
  }
}
```
<a name="a_GET_advertisement_request_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a GET advertisement request** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Processing-Status": "Completed",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "screenId": 1,
    "id": "8e2fde50-bc5f-4a12-9cfb-812e50500184",
    "expiryDate": "2015-11-06T21:19:00Z",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184"
      },
      "view": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184/view"
      }
    }
  }
}
```
<a name="a_GET_advertisement_request_for_a_non-existent_advertisement"></a>
Upon receiving **a GET advertisement request for a non-existent advertisement** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement/9b650105-7434-473f-8293-4e23b7e0e064",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 404,
  "headers": {
    "X-Request-Id": "PactRequestId"
  }
}
```
<a name="a_GET_advertisement_request_for_an_advertisement_of_an_advertiser_not_related_to_the_requestor&#39;s_account_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a GET advertisement request for an advertisement of an advertiser not related to the requestor's account** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
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
<a name="a_GET_advertisement_request_for_an_advertisement_using_a_disabled_requestor_account_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a GET advertisement request for an advertisement using a disabled requestor account** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer ca11ab1e-c0de-b10b-f001-f00db0bb1e",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Forbidden",
    "errors": [
      {
        "code": "AccountError"
      }
    ]
  }
}
```
<a name="a_GET_advertisements_request_for_all_advertisements_given_There_are_no_advertisements"></a>
Given **there are no advertisements**, upon receiving **a GET advertisements request for all advertisements** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "_embedded": {
      "advertisements": [

      ]
    },
    "_links": {
      "self": {
        "href": "/advertisement"
      }
    }
  }
}
```
<a name="a_GET_advertisements_request_for_first_page_of_data_given_A_page_size_of_3_with_more_than_1_page_of_data"></a>
Given **a page size of 3 with more than 1 page of data**, upon receiving **a GET advertisements request for first page of data** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "_embedded": {
      "advertisements": [
        {
          "id": "9141cf19-b8d7-4380-9e3f-3b5c22783bdc",
          "advertiserId": "456",
          "jobTitle": "More Exciting Senior Developer role in a great CBD location. Great $$$",
          "jobReference": "JOB4444",
          "_links": {
            "self": {
              "href": "/advertisement/9141cf19-b8d7-4380-9e3f-3b5c22783bdc"
            },
            "view": {
              "href": "/advertisement/9141cf19-b8d7-4380-9e3f-3b5c22783bdc/view"
            }
          }
        },
        {
          "id": "7bbe4318-fd3b-4d26-8384-d41489ff1dd0",
          "advertiserId": "456",
          "jobTitle": "More Exciting Senior Tester role in a great CBD location. Great $$$",
          "jobReference": "JOB3333",
          "_links": {
            "self": {
              "href": "/advertisement/7bbe4318-fd3b-4d26-8384-d41489ff1dd0"
            },
            "view": {
              "href": "/advertisement/7bbe4318-fd3b-4d26-8384-d41489ff1dd0/view"
            }
          }
        },
        {
          "id": "e6e31b9c-3c2c-4b85-b17f-babbf7da972b",
          "advertiserId": "345",
          "jobTitle": "More Exciting Senior Developer role in a great CBD location. Great $$$",
          "jobReference": "JOB12345",
          "_links": {
            "self": {
              "href": "/advertisement/e6e31b9c-3c2c-4b85-b17f-babbf7da972b"
            },
            "view": {
              "href": "/advertisement/e6e31b9c-3c2c-4b85-b17f-babbf7da972b/view"
            }
          }
        }
      ]
    },
    "_links": {
      "self": {
        "href": "/advertisement"
      },
      "next": {
        "href": "/advertisement?beforeId=6"
      }
    }
  }
}
```
<a name="a_GET_advertisements_request_for_the_first_page_of_advertisements_belonging_to_the_advertiser_given_A_page_size_of_3_with_more_than_1_page_of_data"></a>
Given **a page size of 3 with more than 1 page of data**, upon receiving **a GET advertisements request for the first page of advertisements belonging to the advertiser** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "query": "advertiserId=456",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "_embedded": {
      "advertisements": [
        {
          "id": "9141cf19-b8d7-4380-9e3f-3b5c22783bdc",
          "advertiserId": "456",
          "jobTitle": "More Exciting Senior Developer role in a great CBD location. Great $$$",
          "jobReference": "JOB4444",
          "_links": {
            "self": {
              "href": "/advertisement/9141cf19-b8d7-4380-9e3f-3b5c22783bdc"
            },
            "view": {
              "href": "/advertisement/9141cf19-b8d7-4380-9e3f-3b5c22783bdc/view"
            }
          }
        },
        {
          "id": "7bbe4318-fd3b-4d26-8384-d41489ff1dd0",
          "advertiserId": "456",
          "jobTitle": "More Exciting Senior Tester role in a great CBD location. Great $$$",
          "jobReference": "JOB3333",
          "_links": {
            "self": {
              "href": "/advertisement/7bbe4318-fd3b-4d26-8384-d41489ff1dd0"
            },
            "view": {
              "href": "/advertisement/7bbe4318-fd3b-4d26-8384-d41489ff1dd0/view"
            }
          }
        },
        {
          "id": "3b138935-f65b-4ec7-91d8-fc250757b53d",
          "advertiserId": "456",
          "jobTitle": "Exciting tester role in a great CBD location. Great $$",
          "jobReference": "JOB2222",
          "_links": {
            "self": {
              "href": "/advertisement/3b138935-f65b-4ec7-91d8-fc250757b53d"
            },
            "view": {
              "href": "/advertisement/3b138935-f65b-4ec7-91d8-fc250757b53d/view"
            }
          }
        }
      ]
    },
    "_links": {
      "self": {
        "href": "/advertisement?advertiserId=456"
      },
      "next": {
        "href": "/advertisement?advertiserId=456&beforeId=5"
      }
    }
  }
}
```
<a name="a_GET_advertisements_request_for_the_last_page_of_data_given_A_page_size_of_3_with_more_than_1_page_of_data"></a>
Given **a page size of 3 with more than 1 page of data**, upon receiving **a GET advertisements request for the last page of data** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "query": "beforeId=6",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "_embedded": {
      "advertisements": [
        {
          "id": "3b138935-f65b-4ec7-91d8-fc250757b53d",
          "advertiserId": "456",
          "jobTitle": "Exciting tester role in a great CBD location. Great $$",
          "jobReference": "JOB2222",
          "_links": {
            "self": {
              "href": "/advertisement/3b138935-f65b-4ec7-91d8-fc250757b53d"
            },
            "view": {
              "href": "/advertisement/3b138935-f65b-4ec7-91d8-fc250757b53d/view"
            }
          }
        },
        {
          "id": "f7302df2-704b-407c-a42a-62ff822b5461",
          "advertiserId": "456",
          "jobTitle": "Exciting Developer role in a great CBD location. Great $$",
          "jobReference": "JOB1111",
          "_links": {
            "self": {
              "href": "/advertisement/f7302df2-704b-407c-a42a-62ff822b5461"
            },
            "view": {
              "href": "/advertisement/f7302df2-704b-407c-a42a-62ff822b5461/view"
            }
          }
        },
        {
          "id": "fa6939b5-c91f-4f6a-9600-1ea74963fbb2",
          "advertiserId": "123",
          "jobTitle": "Exciting Developer role in a great CBD location. Great $$",
          "jobReference": "JOB1234",
          "_links": {
            "self": {
              "href": "/advertisement/fa6939b5-c91f-4f6a-9600-1ea74963fbb2"
            },
            "view": {
              "href": "/advertisement/fa6939b5-c91f-4f6a-9600-1ea74963fbb2/view"
            }
          }
        }
      ]
    },
    "_links": {
      "self": {
        "href": "/advertisement?beforeId=6"
      }
    }
  }
}
```
<a name="a_GET_advertisements_request_for_the_second_page_of_advertisements_belonging_to_the_advertiser_given_A_page_size_of_3_with_more_than_1_page_of_data"></a>
Given **a page size of 3 with more than 1 page of data**, upon receiving **a GET advertisements request for the second page of advertisements belonging to the advertiser** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "query": "advertiserId=456&beforeId=5",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "_embedded": {
      "advertisements": [
        {
          "id": "f7302df2-704b-407c-a42a-62ff822b5461",
          "advertiserId": "456",
          "jobTitle": "Exciting Developer role in a great CBD location. Great $$",
          "jobReference": "JOB1111",
          "_links": {
            "self": {
              "href": "/advertisement/f7302df2-704b-407c-a42a-62ff822b5461"
            },
            "view": {
              "href": "/advertisement/f7302df2-704b-407c-a42a-62ff822b5461/view"
            }
          }
        }
      ]
    },
    "_links": {
      "self": {
        "href": "/advertisement?advertiserId=456&beforeId=5"
      }
    }
  }
}
```
<a name="a_GET_advertisements_request_to_retrieve_all_advertisements_for_an_advertiser_that_doesn&#39;t_exist"></a>
Upon receiving **a GET advertisements request to retrieve all advertisements for an advertiser that doesn't exist** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "query": "advertiserId=7d31d9b4-d922-43ef-9e88-f7b507ceea88",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Forbidden",
    "errors": [
      {
        "code": "InvalidValue"
      }
    ]
  }
}
```
<a name="a_GET_advertisements_request_to_retrieve_all_advertisements_for_the_advertiser_not_related_to_requestor"></a>
Upon receiving **a GET advertisements request to retrieve all advertisements for the advertiser not related to requestor** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "query": "advertiserId=874392",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Accept": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
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
<a name="a_GET_index_request_that_is_unauthorised_to_retrieve_API_links"></a>
Upon receiving **a GET index request that is unauthorised to retrieve API links** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "",
  "headers": {
    "Authorization": "Bearer baaa1b4f-0dfd-4d80-a871-64fb78716667",
    "Accept": "application/hal+json; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 401,
  "headers": {
    "X-Request-Id": "PactRequestId"
  }
}
```
<a name="a_GET_index_request_to_retrieve_API_links_with_an_invalid_access_token"></a>
Upon receiving **a GET index request to retrieve API links with an invalid access token** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "",
  "headers": {
    "Authorization": "Bearer ca11ab1e-c0de-b10b-feed-faceb0bb1e",
    "Accept": "application/hal+json; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 401,
  "headers": {
    "WWW-Authenticate": "Bearer error=\"Invalid request\""
  }
}
```
<a name="a_GET_index_request_to_retrieve_API_links_with_Bearer_a4f2aab5-5582-4ff0-b8f2-890d6146dbb6"></a>
Upon receiving **a GET index request to retrieve API links with Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "",
  "headers": {
    "Accept": "application/hal+json; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/hal+json; charset=utf-8"
  },
  "body": {
    "_links": {
      "advertisements": {
        "href": "/advertisement{?advertiserId}",
        "templated": true
      },
      "advertisement": {
        "href": "/advertisement/{advertisementId}",
        "templated": true
      }
    }
  }
}
```
<a name="a_GET_index_request_to_retrieve_API_links_with_Bearer_b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e"></a>
Upon receiving **a GET index request to retrieve API links with Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "",
  "headers": {
    "Accept": "application/hal+json; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/hal+json; charset=utf-8"
  },
  "body": {
    "_links": {
      "advertisements": {
        "href": "/advertisement{?advertiserId}",
        "templated": true
      },
      "advertisement": {
        "href": "/advertisement/{advertisementId}",
        "templated": true
      }
    }
  }
}
```
<a name="a_HEAD_advertisement_request_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a HEAD advertisement request** from Ad Posting API Client, with
```json
{
  "method": "head",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Processing-Status": "Completed"
  }
}
```
<a name="a_HEAD_advertisement_request_for_a_non-existent_advertisement"></a>
Upon receiving **a HEAD advertisement request for a non-existent advertisement** from Ad Posting API Client, with
```json
{
  "method": "head",
  "path": "/advertisement/9b650105-7434-473f-8293-4e23b7e0e064",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 404,
  "headers": {
    "X-Request-Id": "PactRequestId"
  }
}
```
<a name="a_HEAD_advertisement_request_for_an_advertisement_of_an_advertiser_not_related_to_the_requestor&#39;s_account_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a HEAD advertisement request for an advertisement of an advertiser not related to the requestor's account** from Ad Posting API Client, with
```json
{
  "method": "head",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "X-Request-Id": "PactRequestId"
  }
}
```
<a name="a_HEAD_advertisement_request_for_an_advertisement_using_a_disabled_requestor_account_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a HEAD advertisement request for an advertisement using a disabled requestor account** from Ad Posting API Client, with
```json
{
  "method": "head",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer ca11ab1e-c0de-b10b-f001-f00db0bb1e",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "X-Request-Id": "PactRequestId"
  }
}
```
<a name="a_PATCH_advertisement_request_to_expire_a_job_for_an_advertiser_not_related_to_the_requestor&#39;s_account_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PATCH advertisement request to expire a job for an advertiser not related to the requestor's account** from Ad Posting API Client, with
```json
{
  "method": "patch",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": [
    {
      "op": "replace",
      "path": "state",
      "value": "Expired"
    }
  ]
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
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
<a name="a_PATCH_advertisement_request_to_expire_a_job_using_a_disabled_requestor_account_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PATCH advertisement request to expire a job using a disabled requestor account** from Ad Posting API Client, with
```json
{
  "method": "patch",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer ca11ab1e-c0de-b10b-f001-f00db0bb1e",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": [
    {
      "op": "replace",
      "path": "state",
      "value": "Expired"
    }
  ]
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Forbidden",
    "errors": [
      {
        "code": "AccountError"
      }
    ]
  }
}
```
<a name="a_PATCH_advertisement_request_to_expire_a_job_using_invalid_request_content_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PATCH advertisement request to expire a job using invalid request content** from Ad Posting API Client, with
```json
{
  "method": "patch",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": [
    {
      "op": "add",
      "path": "state",
      "value": "open"
    }
  ]
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Validation Failure",
    "errors": [
      {
        "code": "InvalidRequestContent"
      }
    ]
  }
}
```
<a name="a_PATCH_advertisement_request_to_expire_a_non-existent_advertisement"></a>
Upon receiving **a PATCH advertisement request to expire a non-existent advertisement** from Ad Posting API Client, with
```json
{
  "method": "patch",
  "path": "/advertisement/9b650105-7434-473f-8293-4e23b7e0e064",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": [
    {
      "op": "replace",
      "path": "state",
      "value": "Expired"
    }
  ]
}
```
Ad Posting API will respond with:
```json
{
  "status": 404,
  "headers": {
    "X-Request-Id": "PactRequestId"
  }
}
```
<a name="a_PATCH_advertisement_request_to_expire_an_advertisement_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PATCH advertisement request to expire an advertisement** from Ad Posting API Client, with
```json
{
  "method": "patch",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": [
    {
      "op": "replace",
      "path": "state",
      "value": "Expired"
    }
  ]
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "screenId": 1,
    "id": "8e2fde50-bc5f-4a12-9cfb-812e50500184",
    "expiryDate": "2015-10-07T21:19:00Z",
    "state": "Expired",
    "_links": {
      "self": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184"
      },
      "view": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184/view"
      }
    }
  }
}
```
<a name="a_PATCH_advertisement_request_to_expire_an_advertisement_given_There_is_an_expired_advertisement"></a>
Given **there is an expired advertisement**, upon receiving **a PATCH advertisement request to expire an advertisement** from Ad Posting API Client, with
```json
{
  "method": "patch",
  "path": "/advertisement/c294088d-ff50-4374-bc38-7fa805790e3e",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": [
    {
      "op": "replace",
      "path": "state",
      "value": "Expired"
    }
  ]
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Forbidden",
    "errors": [
      {
        "code": "Expired"
      }
    ]
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_given_The_requestor&#39;s_account_is_disabled"></a>
Given **the requestor's account is disabled**, upon receiving **a POST advertisement request to create a job** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Forbidden",
    "errors": [
      {
        "code": "AccountError"
      }
    ]
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_with_a_questionnaire_ID"></a>
Upon receiving **a POST advertisement request to create a job ad with a questionnaire ID** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "questionnaireId": "QUESTIONNAIRE1234",
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Location": "http://localhost/advertisement/c6d541a4-e4c4-4357-a101-7762f8987581",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "questionnaireId": "QUESTIONNAIRE1234",
    "id": "c6d541a4-e4c4-4357-a101-7762f8987581",
    "expiryDate": "2015-11-06T21:19:00Z",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/c6d541a4-e4c4-4357-a101-7762f8987581"
      },
      "view": {
        "href": "/advertisement/c6d541a4-e4c4-4357-a101-7762f8987581/view"
      }
    }
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_with_an_invalid_advertiser_id"></a>
Upon receiving **a POST advertisement request to create a job ad with an invalid advertiser id** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1234ABC"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Forbidden",
    "errors": [
      {
        "code": "InvalidValue"
      }
    ]
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_with_duplicated_names_for_template_custom_fields"></a>
Upon receiving **a POST advertisement request to create a job ad with duplicated names for template custom fields** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "creationId": "20160120-162020-00000",
    "template": {
      "items": [
        {
          "name": "FieldNameA",
          "value": "Template Value 1"
        },
        {
          "name": "FieldNameB",
          "value": "Template Value 2"
        },
        {
          "name": "FieldNameA",
          "value": "Template Value 3"
        }
      ]
    }
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Validation Failure",
    "errors": [
      {
        "field": "template.items[0]",
        "code": "AlreadySpecified"
      },
      {
        "field": "template.items[2]",
        "code": "AlreadySpecified"
      }
    ]
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_with_granular_location"></a>
Upon receiving **a POST advertisement request to create a job ad with granular location** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "granularLocation": {
      "country": "Australia",
      "state": "Victoria",
      "city": "Melbourne",
      "postCode": "3000"
    },
    "screenId": 1,
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Location": "http://localhost/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "granularLocation": {
      "country": "Australia",
      "city": "Melbourne",
      "postCode": "3000"
    },
    "screenId": 1,
    "id": "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a",
    "expiryDate": "2015-11-06T21:19:00Z",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a"
      },
      "view": {
        "href": "/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a/view"
      }
    }
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_with_invalid_advertisement_details"></a>
Upon receiving **a POST advertisement request to create a job ad with invalid advertisement details** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Ad details with <a href='www.youtube.com'>a link</a> and incomplete <h2> element",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "creationId": "20150914-134527-00109"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Validation Failure",
    "errors": [
      {
        "field": "advertisementDetails",
        "code": "InvalidFormat"
      }
    ]
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_with_invalid_advertisement_details_and_with_&#39;CleanseJobAdDetails&#39;_processing_option"></a>
Upon receiving **a POST advertisement request to create a job ad with invalid advertisement details and with 'CleanseJobAdDetails' processing option** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "<p style=\"text-align:justify; color:#FF00AA\">Colourful</p>",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "creationId": "20150914-134527-00012",
    "processingOptions": [
      "CleanseAdvertisementDetails"
    ]
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Location": "http://localhost/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "<p style=\"text-align:justify\">Colourful</p>",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "id": "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a",
    "expiryDate": "2015-11-06T21:19:00Z",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a"
      },
      "view": {
        "href": "/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a/view"
      }
    }
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_with_invalid_field_values"></a>
Upon receiving **a POST advertisement request to create a job ad with invalid field values** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": -1.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "creationId": "20150914-134527-00109",
    "standout": {
      "bullets": [
        "new Uzi",
        "new Remington Model!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
        "new AK-47"
      ]
    },
    "applicationEmail": "someone(at)some.domain",
    "applicationFormUrl": "htp://somecompany.domain/apply",
    "template": {
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "",
          "value": "value2"
        }
      ]
    }
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Validation Failure",
    "errors": [
      {
        "field": "applicationEmail",
        "code": "InvalidEmailAddress"
      },
      {
        "field": "applicationFormUrl",
        "code": "InvalidUrl"
      },
      {
        "field": "salary.minimum",
        "code": "ValueOutOfRange"
      },
      {
        "field": "standout.bullets[1]",
        "code": "MaxLengthExceeded"
      },
      {
        "field": "template.items[1].name",
        "code": "Required"
      }
    ]
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_with_invalid_salary_data"></a>
Upon receiving **a POST advertisement request to create a job ad with invalid salary data** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 2.0,
      "maximum": 1.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Validation Failure",
    "errors": [
      {
        "field": "salary.maximum",
        "code": "InvalidValue"
      }
    ]
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_with_required_and_optional_field_values"></a>
Upon receiving **a POST advertisement request to create a job ad with required and optional field values** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "screenId": 1,
    "creationId": "20150914-134527-00097"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Location": "http://localhost/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "screenId": 1,
    "id": "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a",
    "expiryDate": "2015-11-06T21:19:00Z",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a"
      },
      "view": {
        "href": "/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a/view"
      }
    }
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_with_required_field_values_only"></a>
Upon receiving **a POST advertisement request to create a job ad with required field values only** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Location": "http://localhost/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "id": "75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a",
    "expiryDate": "2015-11-06T21:19:00Z",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a"
      },
      "view": {
        "href": "/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a/view"
      }
    }
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_with_the_same_creation_id_&#39;CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184&#39;_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a POST advertisement request to create a job ad with the same creation id 'CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184'** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "creationId": "CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 409,
  "headers": {
    "Location": "http://localhost/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Conflict",
    "errors": [
      {
        "field": "creationId",
        "code": "AlreadyExists"
      }
    ]
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_ad_without_a_creation_id"></a>
Upon receiving **a POST advertisement request to create a job ad without a creation id** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    }
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Validation Failure",
    "errors": [
      {
        "field": "creationId",
        "code": "Required"
      }
    ]
  }
}
```
<a name="a_POST_advertisement_request_to_create_a_job_for_an_advertiser_not_related_to_the_requestor&#39;s_account"></a>
Upon receiving **a POST advertisement request to create a job for an advertiser not related to the requestor's account** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "999888777"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
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
<a name="a_POST_advertisement_request_to_expire_an_advertisement_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a POST advertisement request to expire an advertisement** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": [
    {
      "op": "replace",
      "path": "state",
      "value": "Expired"
    }
  ]
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "screenId": 1,
    "id": "8e2fde50-bc5f-4a12-9cfb-812e50500184",
    "expiryDate": "2015-10-07T21:19:00Z",
    "state": "Expired",
    "_links": {
      "self": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184"
      },
      "view": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184/view"
      }
    }
  }
}
```
<a name="a_PUT_advertisement_request_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PUT advertisement request** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$ - updated",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://FakeATS.com.au",
    "endApplicationUrl": "http://endform.com/updated",
    "jobReference": "JOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "new Uzi",
        "new Remington Model",
        "new AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "screenId": 1
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$ - updated",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://FakeATS.com.au",
    "endApplicationUrl": "http://endform.com/updated",
    "jobReference": "JOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "new Uzi",
        "new Remington Model",
        "new AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "screenId": 1,
    "id": "8e2fde50-bc5f-4a12-9cfb-812e50500184",
    "expiryDate": "2015-11-06T21:19:00Z",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184"
      },
      "view": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184/view"
      }
    }
  }
}
```
<a name="a_PUT_advertisement_request_for_a_non-existent_advertisement"></a>
Upon receiving **a PUT advertisement request for a non-existent advertisement** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/9b650105-7434-473f-8293-4e23b7e0e064",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "This advertisement should not exist.",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    }
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 404,
  "headers": {
    "X-Request-Id": "PactRequestId"
  }
}
```
<a name="a_PUT_advertisement_request_for_advertisement_with_invalid_advertisement_details_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PUT advertisement request for advertisement with invalid advertisement details** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Ad details with <a href='www.youtube.com'>a link</a> and incomplete <h2> element",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    }
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Validation Failure",
    "errors": [
      {
        "field": "advertisementDetails",
        "code": "InvalidFormat"
      }
    ]
  }
}
```
<a name="a_PUT_advertisement_request_for_advertisement_with_invalid_advertisement_details_and_with_&#39;CleanseJobAdDetails&#39;_processing_option_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PUT advertisement request for advertisement with invalid advertisement details and with 'CleanseJobAdDetails' processing option** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "<p style=\"text-align:justify; font-family:'Comic Sans MS', cursive, sans-serif\">Whimsical</p>",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "additionalProperties": [
      "Graduate"
    ],
    "processingOptions": [
      "CleanseAdvertisementDetails"
    ]
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "<p style=\"text-align:justify\">Whimsical</p>",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "id": "8e2fde50-bc5f-4a12-9cfb-812e50500184",
    "expiryDate": "2015-11-06T21:19:00Z",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184"
      },
      "view": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184/view"
      }
    },
    "additionalProperties": [
      "Graduate"
    ]
  }
}
```
<a name="a_PUT_advertisement_request_for_advertisement_with_invalid_field_values_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PUT advertisement request for advertisement with invalid field values** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": -1.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    },
    "standout": {
      "bullets": [
        "new Uzi",
        "new Remington Model!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
        "new AK-47"
      ]
    },
    "applicationEmail": "someone(at)some.domain",
    "applicationFormUrl": "htp://somecompany.domain/apply",
    "template": {
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "",
          "value": "value2"
        }
      ]
    }
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Validation Failure",
    "errors": [
      {
        "field": "applicationEmail",
        "code": "InvalidEmailAddress"
      },
      {
        "field": "applicationFormUrl",
        "code": "InvalidUrl"
      },
      {
        "field": "salary.minimum",
        "code": "ValueOutOfRange"
      },
      {
        "field": "standout.bullets[1]",
        "code": "MaxLengthExceeded"
      },
      {
        "field": "template.items[1].name",
        "code": "Required"
      }
    ]
  }
}
```
<a name="a_PUT_advertisement_request_for_advertisement_with_invalid_salary_data_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PUT advertisement request for advertisement with invalid salary data** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 2.0,
      "maximum": 1.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    }
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Validation Failure",
    "errors": [
      {
        "field": "salary.maximum",
        "code": "InvalidValue"
      }
    ]
  }
}
```
<a name="a_PUT_advertisement_request_to_update_a_job_ad_with_a_different_advertiser_from_the_one_owning_the_job_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PUT advertisement request to update a job ad with a different advertiser from the one owning the job** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "99887766",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "screenId": 1
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
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
<a name="a_PUT_advertisement_request_to_update_a_job_ad_with_a_questionnaire_ID_given_There_is_a_standout_advertisement_with_maximum_data_and_a_questionnaire_ID"></a>
Given **there is a standout advertisement with maximum data and a questionnaire ID**, upon receiving **a PUT advertisement request to update a job ad with a questionnaire ID** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "questionnaireId": "DifferentQuestionnaireId"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "questionnaireId": "QUESTIONNAIRE1234",
    "id": "8e2fde50-bc5f-4a12-9cfb-812e50500184",
    "expiryDate": "2015-11-06T21:19:00Z",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184"
      },
      "view": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184/view"
      }
    }
  }
}
```
<a name="a_PUT_advertisement_request_to_update_a_job_for_an_advertiser_not_related_to_the_requestor&#39;s_account_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PUT advertisement request to update a job for an advertiser not related to the requestor's account** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "screenId": 1
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
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
<a name="a_PUT_advertisement_request_to_update_a_job_using_a_disabled_requestor_account_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PUT advertisement request to update a job using a disabled requestor account** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer ca11ab1e-c0de-b10b-f001-f00db0bb1e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "screenId": 1
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Forbidden",
    "errors": [
      {
        "code": "AccountError"
      }
    ]
  }
}
```
<a name="a_PUT_advertisement_request_to_update_an_expired_advertisement_given_There_is_an_expired_advertisement"></a>
Given **there is an expired advertisement**, upon receiving **a PUT advertisement request to update an expired advertisement** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/c294088d-ff50-4374-bc38-7fa805790e3e",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com"
    }
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "message": "Forbidden",
    "errors": [
      {
        "code": "Expired"
      }
    ]
  }
}
```
<a name="a_PUT_advertisement_request_to_update_granular_location_given_There_is_a_standout_advertisement_with_maximum_data"></a>
Given **there is a standout advertisement with maximum data**, upon receiving **a PUT advertisement request to update granular location** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Accept": "application/vnd.seek.advertisement+json; version=1; charset=utf-8, application/vnd.seek.advertisement-error+json; version=1; charset=utf-8",
    "User-Agent": "SEEK.AdPostingApi.Client/0.15.630.1108"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "granularLocation": {
      "country": "Australia",
      "state": "Victoria",
      "city": "Melbourne",
      "postCode": "3000"
    },
    "screenId": 1
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "X-Request-Id": "PactRequestId"
  },
  "body": {
    "advertisementDetails": "Exciting, do I need to say more?",
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobSummary": "Developer job",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "salary": {
      "minimum": 100000.0,
      "maximum": 119999.0,
      "type": "AnnualPackage",
      "details": "We will pay you"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "recruiter": {
      "fullName": "Recruiter Full Name",
      "email": "recruiter@email.com",
      "teamName": "Recruiter Team Name"
    },
    "searchJobTitle": "Senior Developer, .NET Core, Scala, Team Leader, Agile Methodologies",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/embed/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://apply.com/",
    "endApplicationUrl": "http://endform.com/",
    "jobReference": "JOB1234",
    "agentJobReference": "AGENTJOB1234",
    "template": {
      "id": 1,
      "items": [
        {
          "name": "Template Line 1",
          "value": "Template Value 1"
        },
        {
          "name": "Template Line 2",
          "value": "Template Value 2"
        }
      ]
    },
    "standout": {
      "logoId": 1,
      "bullets": [
        "Uzi",
        "Remington Model",
        "AK-47"
      ]
    },
    "additionalProperties": [
      "ResidentsOnly",
      "Graduate"
    ],
    "granularLocation": {
      "country": "Australia",
      "city": "Melbourne",
      "postCode": "3000"
    },
    "screenId": 1,
    "id": "8e2fde50-bc5f-4a12-9cfb-812e50500184",
    "expiryDate": "2015-11-06T21:19:00Z",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184"
      },
      "view": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184/view"
      }
    }
  }
}
```

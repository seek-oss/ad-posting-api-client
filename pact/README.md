### A pact between Ad Posting API Client and Ad Posting API

#### Requests from Ad Posting API Client to Ad Posting API

* [A request to create a job ad with a non integer advertiser name](#a_request_to_create_a_job_ad_with_a_non_integer_advertiser_name)

* [A request to create a job ad with bad data](#a_request_to_create_a_job_ad_with_bad_data)

* [A request to create a job ad with duplicated names for template custom fields](#a_request_to_create_a_job_ad_with_duplicated_names_for_template_custom_fields)

* [A request to create a job ad with invalid advertisement details](#a_request_to_create_a_job_ad_with_invalid_advertisement_details)

* [A request to create a job ad with invalid salary data](#a_request_to_create_a_job_ad_with_invalid_salary_data)

* [A request to create a job ad with maximum required data](#a_request_to_create_a_job_ad_with_maximum_required_data)

* [A request to create a job ad with minimum required data](#a_request_to_create_a_job_ad_with_minimum_required_data)

* [A request to create a job ad with the same creation id 'CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184'](#a_request_to_create_a_job_ad_with_the_same_creation_id_&#39;CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184&#39;_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [A request to create a job ad without a creation id](#a_request_to_create_a_job_ad_without_a_creation_id)

* [A request to create a job for an advertiser not related to the third party uploader](#a_request_to_create_a_job_for_an_advertiser_not_related_to_the_third_party_uploader)

* [A request to create a job with a disabled third party uploader](#a_request_to_create_a_job_with_a_disabled_third_party_uploader_given_The_third_party_uploader_account_is_disabled) given the third party uploader account is disabled

* [A request to expire a job for an advertiser not related to the third party uploader](#a_request_to_expire_a_job_for_an_advertiser_not_related_to_the_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [A request to expire a job with a disabled third party uploader](#a_request_to_expire_a_job_with_a_disabled_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [A request to retrieve API links with an invalid access token](#a_request_to_retrieve_API_links_with_an_invalid_access_token)

* [A request to retrieve API links with Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6](#a_request_to_retrieve_API_links_with_Bearer_a4f2aab5-5582-4ff0-b8f2-890d6146dbb6)

* [A request to retrieve API links with Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e](#a_request_to_retrieve_API_links_with_Bearer_b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e)

* [A request to update a job ad with a different advertiser from the one owning the job](#a_request_to_update_a_job_ad_with_a_different_advertiser_from_the_one_owning_the_job_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [A request to update a job for an advertiser not related to the third party uploader](#a_request_to_update_a_job_for_an_advertiser_not_related_to_the_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [A request to update a job with a disabled third party uploader](#a_request_to_update_a_job_with_a_disabled_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [A request to update a standout job to classic](#a_request_to_update_a_standout_job_to_classic_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [An expire request for a non-existent advertisement](#an_expire_request_for_a_non-existent_advertisement)

* [An expire request for advertisement](#an_expire_request_for_advertisement_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [An expire request for advertisement](#an_expire_request_for_advertisement_given_There_is_an_expired_advertisement) given there is an expired advertisement

* [GET request for a non-existent advertisement](#gET_request_for_a_non-existent_advertisement)

* [GET request for advertisement](#gET_request_for_advertisement_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [GET request for advertisement with errors](#gET_request_for_advertisement_with_errors_given_There_is_a_failed_classic_advertisement) given there is a failed classic advertisement

* [GET request for advertisement with warnings](#gET_request_for_advertisement_with_warnings_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [GET request for all advertisements](#gET_request_for_all_advertisements_given_There_are_no_advertisements) given there are no advertisements

* [GET request for an advertisement of an advertiser not related to the third party uploader](#gET_request_for_an_advertisement_of_an_advertiser_not_related_to_the_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [GET request for an advertisement with a disabled third party uploader](#gET_request_for_an_advertisement_with_a_disabled_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [GET request for first page of data](#gET_request_for_first_page_of_data_given_A_page_size_of_3_with_more_than_1_page_of_data) given a page size of 3 with more than 1 page of data

* [GET request for the first page of advertisements belong to the advertiser](#gET_request_for_the_first_page_of_advertisements_belong_to_the_advertiser_given_A_page_size_of_3_with_more_than_1_page_of_data) given a page size of 3 with more than 1 page of data

* [GET request for the last page of data](#gET_request_for_the_last_page_of_data_given_A_page_size_of_3_with_more_than_1_page_of_data) given a page size of 3 with more than 1 page of data

* [GET request for the second page of advertisements belong to the advertiser](#gET_request_for_the_second_page_of_advertisements_belong_to_the_advertiser_given_A_page_size_of_3_with_more_than_1_page_of_data) given a page size of 3 with more than 1 page of data

* [GET request to retrieve all advertisements for an advertiser not exists](#gET_request_to_retrieve_all_advertisements_for_an_advertiser_not_exists)

* [GET request to retrieve all advertisements for the advertiser not related to uploader](#gET_request_to_retrieve_all_advertisements_for_the_advertiser_not_related_to_uploader)

* [HEAD request for a non-existent advertisement](#hEAD_request_for_a_non-existent_advertisement)

* [HEAD request for advertisement](#hEAD_request_for_advertisement_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [HEAD request for an advertisement of an advertiser not related to the third party uploader](#hEAD_request_for_an_advertisement_of_an_advertiser_not_related_to_the_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [HEAD request for an advertisement with a disabled third party uploader](#hEAD_request_for_an_advertisement_with_a_disabled_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [Unauthorised request to retrieve API links](#unauthorised_request_to_retrieve_API_links)

* [Update request for a non-existent advertisement](#update_request_for_a_non-existent_advertisement)

* [Update request for advertisement](#update_request_for_advertisement_given_There_is_a_pending_standout_advertisement_with_maximum_data) given there is a pending standout advertisement with maximum data

* [Update request for advertisement with bad data](#update_request_for_advertisement_with_bad_data)

* [Update request for advertisement with invalid salary data](#update_request_for_advertisement_with_invalid_salary_data)

#### Interactions

<a name="a_request_to_create_a_job_ad_with_a_non_integer_advertiser_name"></a>
Upon receiving **a request to create a job ad with a non integer advertiser name** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1234ABC"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_create_a_job_ad_with_bad_data"></a>
Upon receiving **a request to create a job ad with bad data** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": -1.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
    "creationId": "20150914-134527-00109",
    "video": {
      "url": "htp://www.youtube.com/v/abc",
      "position": "Below"
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
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
      },
      {
        "field": "video.url",
        "code": "RegexPatternNotMatched"
      }
    ]
  }
}
```
<a name="a_request_to_create_a_job_ad_with_duplicated_names_for_template_custom_fields"></a>
Upon receiving **a request to create a job ad with duplicated names for template custom fields** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
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
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_create_a_job_ad_with_invalid_advertisement_details"></a>
Upon receiving **a request to create a job ad with invalid advertisement details** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Ad details with <a href='www.youtube.com'>a link</a> and incomplete <h2> element",
    "creationId": "20150914-134527-00109"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_create_a_job_ad_with_invalid_salary_data"></a>
Upon receiving **a request to create a job ad with invalid salary data** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 2.0,
      "maximum": 1.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_create_a_job_ad_with_maximum_required_data"></a>
Upon receiving **a request to create a job ad with maximum required data** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0,
      "details": "We will pay you"
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
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
    "screenId": 1,
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
    "creationId": "20150914-134527-00097"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 202,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Location": "http://localhost/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0,
      "details": "We will pay you"
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
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
    "screenId": 1,
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
<a name="a_request_to_create_a_job_ad_with_minimum_required_data"></a>
Upon receiving **a request to create a job ad with minimum required data** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 202,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Location": "http://localhost/advertisement/75b2b1fc-9050-4f45-a632-ec6b7ac2bb4a"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
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
<a name="a_request_to_create_a_job_ad_with_the_same_creation_id_&#39;CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184&#39;_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **a request to create a job ad with the same creation id 'CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184'** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
    "creationId": "CreationIdOf8e2fde50-bc5f-4a12-9cfb-812e50500184"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 409,
  "headers": {
    "Location": "http://localhost/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184"
  }
}
```
<a name="a_request_to_create_a_job_ad_without_a_creation_id"></a>
Upon receiving **a request to create a job ad without a creation id** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_create_a_job_for_an_advertiser_not_related_to_the_third_party_uploader"></a>
Upon receiving **a request to create a job for an advertiser not related to the third party uploader** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "999888777"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_create_a_job_with_a_disabled_third_party_uploader_given_The_third_party_uploader_account_is_disabled"></a>
Given **the third party uploader account is disabled**, upon receiving **a request to create a job with a disabled third party uploader** from Ad Posting API Client, with
```json
{
  "method": "post",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
    "creationId": "20150914-134527-00012"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_expire_a_job_for_an_advertiser_not_related_to_the_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **a request to expire a job for an advertiser not related to the third party uploader** from Ad Posting API Client, with
```json
{
  "method": "patch",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8"
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
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_expire_a_job_with_a_disabled_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **a request to expire a job with a disabled third party uploader** from Ad Posting API Client, with
```json
{
  "method": "patch",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer ca11ab1e-c0de-b10b-f001-f00db0bb1e",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8"
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
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_retrieve_API_links_with_an_invalid_access_token"></a>
Upon receiving **a request to retrieve API links with an invalid access token** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "",
  "headers": {
    "Authorization": "Bearer ca11ab1e-c0de-b10b-feed-faceb0bb1e",
    "Accept": "application/hal+json"
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
<a name="a_request_to_retrieve_API_links_with_Bearer_a4f2aab5-5582-4ff0-b8f2-890d6146dbb6"></a>
Upon receiving **a request to retrieve API links with Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "",
  "headers": {
    "Accept": "application/hal+json",
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6"
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
<a name="a_request_to_retrieve_API_links_with_Bearer_b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e"></a>
Upon receiving **a request to retrieve API links with Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "",
  "headers": {
    "Accept": "application/hal+json",
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e"
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
<a name="a_request_to_update_a_job_ad_with_a_different_advertiser_from_the_one_owning_the_job_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **a request to update a job ad with a different advertiser from the one owning the job** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "99887766",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0,
      "details": "We will pay you"
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
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
    "screenId": 1,
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
    ]
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_update_a_job_for_an_advertiser_not_related_to_the_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **a request to update a job for an advertiser not related to the third party uploader** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0,
      "details": "We will pay you"
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
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
    "screenId": 1,
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
    ]
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_update_a_job_with_a_disabled_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **a request to update a job with a disabled third party uploader** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer ca11ab1e-c0de-b10b-f001-f00db0bb1e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1",
      "agentId": "385"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0,
      "details": "We will pay you"
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
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
    "screenId": 1,
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
    ]
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="a_request_to_update_a_standout_job_to_classic_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **a request to update a standout job to classic** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
  },
  "body": {
    "message": "Validation Failure",
    "errors": [
      {
        "field": "advertisementType",
        "code": "ChangeNotAllowed"
      }
    ]
  }
}
```
<a name="an_expire_request_for_a_non-existent_advertisement"></a>
Upon receiving **an expire request for a non-existent advertisement** from Ad Posting API Client, with
```json
{
  "method": "patch",
  "path": "/advertisement/9b650105-7434-473f-8293-4e23b7e0e064",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8"
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
  "status": 404
}
```
<a name="an_expire_request_for_advertisement_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **an expire request for advertisement** from Ad Posting API Client, with
```json
{
  "method": "patch",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8"
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
  "status": 202,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0,
      "details": "We will pay you"
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
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
    "screenId": 1,
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
<a name="an_expire_request_for_advertisement_given_There_is_an_expired_advertisement"></a>
Given **there is an expired advertisement**, upon receiving **an expire request for advertisement** from Ad Posting API Client, with
```json
{
  "method": "patch",
  "path": "/advertisement/c294088d-ff50-4374-bc38-7fa805790e3e",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement-patch+json; version=1; charset=utf-8"
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
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
  },
  "body": {
    "message": "Validation Failure",
    "errors": [
      {
        "code": "InvalidState",
        "message": "Advertisement has already expired."
      }
    ]
  }
}
```
<a name="gET_request_for_a_non-existent_advertisement"></a>
Upon receiving **gET request for a non-existent advertisement** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement/9b650105-7434-473f-8293-4e23b7e0e064",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 404
}
```
<a name="gET_request_for_advertisement_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **gET request for advertisement** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Processing-Status": "Pending"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0,
      "details": "We will pay you"
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
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
    "screenId": 1,
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
<a name="gET_request_for_advertisement_with_errors_given_There_is_a_failed_classic_advertisement"></a>
Given **there is a failed classic advertisement**, upon receiving **gET request for advertisement with errors** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement/448b8474-6165-4eed-a5b5-d2bb52e471ef",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Processing-Status": "Failed"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/448b8474-6165-4eed-a5b5-d2bb52e471ef"
      },
      "view": {
        "href": "/advertisement/448b8474-6165-4eed-a5b5-d2bb52e471ef/view"
      }
    },
    "errors": [
      {
        "code": "Unauthorised",
        "message": "Unauthorised"
      }
    ]
  }
}
```
<a name="gET_request_for_advertisement_with_warnings_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **gET request for advertisement with warnings** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Processing-Status": "Pending"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0,
      "details": "We will pay you"
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
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
    "screenId": 1,
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
    "state": "Open",
    "_links": {
      "self": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184"
      },
      "view": {
        "href": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184/view"
      }
    },
    "warnings": [
      {
        "field": "standout.logoId",
        "code": "missing"
      },
      {
        "field": "standout.bullets",
        "code": "missing"
      }
    ]
  }
}
```
<a name="gET_request_for_all_advertisements_given_There_are_no_advertisements"></a>
Given **there are no advertisements**, upon receiving **gET request for all advertisements** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/hal+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8"
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
<a name="gET_request_for_an_advertisement_of_an_advertiser_not_related_to_the_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **gET request for an advertisement of an advertiser not related to the third party uploader** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Accept": "application/vnd.seek.advertisement+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="gET_request_for_an_advertisement_with_a_disabled_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **gET request for an advertisement with a disabled third party uploader** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer ca11ab1e-c0de-b10b-f001-f00db0bb1e",
    "Accept": "application/vnd.seek.advertisement+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="gET_request_for_first_page_of_data_given_A_page_size_of_3_with_more_than_1_page_of_data"></a>
Given **a page size of 3 with more than 1 page of data**, upon receiving **gET request for first page of data** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/hal+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8"
  },
  "body": {
    "_embedded": {
      "advertisements": [
        {
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
<a name="gET_request_for_the_first_page_of_advertisements_belong_to_the_advertiser_given_A_page_size_of_3_with_more_than_1_page_of_data"></a>
Given **a page size of 3 with more than 1 page of data**, upon receiving **gET request for the first page of advertisements belong to the advertiser** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "query": "advertiserId=456",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/hal+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8"
  },
  "body": {
    "_embedded": {
      "advertisements": [
        {
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
<a name="gET_request_for_the_last_page_of_data_given_A_page_size_of_3_with_more_than_1_page_of_data"></a>
Given **a page size of 3 with more than 1 page of data**, upon receiving **gET request for the last page of data** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "query": "beforeId=6",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/hal+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8"
  },
  "body": {
    "_embedded": {
      "advertisements": [
        {
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
<a name="gET_request_for_the_second_page_of_advertisements_belong_to_the_advertiser_given_A_page_size_of_3_with_more_than_1_page_of_data"></a>
Given **a page size of 3 with more than 1 page of data**, upon receiving **gET request for the second page of advertisements belong to the advertiser** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "query": "advertiserId=456&beforeId=5",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/hal+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-list+json; version=1; charset=utf-8"
  },
  "body": {
    "_embedded": {
      "advertisements": [
        {
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
<a name="gET_request_to_retrieve_all_advertisements_for_an_advertiser_not_exists"></a>
Upon receiving **gET request to retrieve all advertisements for an advertiser not exists** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "query": "advertiserId=7d31d9b4-d922-43ef-9e88-f7b507ceea88",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/hal+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="gET_request_to_retrieve_all_advertisements_for_the_advertiser_not_related_to_uploader"></a>
Upon receiving **gET request to retrieve all advertisements for the advertiser not related to uploader** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "/advertisement",
  "query": "advertiserId=874392",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Accept": "application/hal+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
<a name="hEAD_request_for_a_non-existent_advertisement"></a>
Upon receiving **hEAD request for a non-existent advertisement** from Ad Posting API Client, with
```json
{
  "method": "head",
  "path": "/advertisement/9b650105-7434-473f-8293-4e23b7e0e064",
  "headers": {
    "Authorization": "Bearer ca11ab1e-c0de-b10b-f001-f00db0bb1e",
    "Accept": "application/vnd.seek.advertisement+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 404
}
```
<a name="hEAD_request_for_advertisement_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **hEAD request for advertisement** from Ad Posting API Client, with
```json
{
  "method": "head",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Accept": "application/vnd.seek.advertisement+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 200,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8",
    "Processing-Status": "Pending"
  }
}
```
<a name="hEAD_request_for_an_advertisement_of_an_advertiser_not_related_to_the_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **hEAD request for an advertisement of an advertiser not related to the third party uploader** from Ad Posting API Client, with
```json
{
  "method": "head",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer a4f2aab5-5582-4ff0-b8f2-890d6146dbb6",
    "Accept": "application/vnd.seek.advertisement+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403
}
```
<a name="hEAD_request_for_an_advertisement_with_a_disabled_third_party_uploader_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **hEAD request for an advertisement with a disabled third party uploader** from Ad Posting API Client, with
```json
{
  "method": "head",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer ca11ab1e-c0de-b10b-f001-f00db0bb1e",
    "Accept": "application/vnd.seek.advertisement+json"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 403
}
```
<a name="unauthorised_request_to_retrieve_API_links"></a>
Upon receiving **unauthorised request to retrieve API links** from Ad Posting API Client, with
```json
{
  "method": "get",
  "path": "",
  "headers": {
    "Authorization": "Bearer baaa1b4f-0dfd-4d80-a871-64fb78716667"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 401
}
```
<a name="update_request_for_a_non-existent_advertisement"></a>
Upon receiving **update request for a non-existent advertisement** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/9b650105-7434-473f-8293-4e23b7e0e064",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "This advertisement should not exist."
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 404
}
```
<a name="update_request_for_advertisement_given_There_is_a_pending_standout_advertisement_with_maximum_data"></a>
Given **there is a pending standout advertisement with maximum data**, upon receiving **update request for advertisement** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/8e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$ - updated",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0,
      "details": "We will pay you"
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/v/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://FakeATS.com.au",
    "endApplicationUrl": "http://endform.com/updated",
    "screenId": 1,
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
    ]
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 202,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement+json; version=1; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$ - updated",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 100000.0,
      "maximum": 119999.0,
      "details": "We will pay you"
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
    "contact": {
      "name": "Contact name",
      "email": "qwert@asdf.com",
      "phone": "+1 (123) 456 7889"
    },
    "video": {
      "url": "https://www.youtube.com/v/dVDk7PXNXB8",
      "position": "Above"
    },
    "applicationEmail": "asdf@asdf.com",
    "applicationFormUrl": "http://FakeATS.com.au",
    "endApplicationUrl": "http://endform.com/updated",
    "screenId": 1,
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
<a name="update_request_for_advertisement_with_bad_data"></a>
Upon receiving **update request for advertisement with bad data** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/7e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "StandOut",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": -1.0,
      "maximum": 119999.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?",
    "video": {
      "url": "htp://www.youtube.com/v/abc",
      "position": "Below"
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
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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
      },
      {
        "field": "video.url",
        "code": "RegexPatternNotMatched"
      }
    ]
  }
}
```
<a name="update_request_for_advertisement_with_invalid_salary_data"></a>
Upon receiving **update request for advertisement with invalid salary data** from Ad Posting API Client, with
```json
{
  "method": "put",
  "path": "/advertisement/7e2fde50-bc5f-4a12-9cfb-812e50500184",
  "headers": {
    "Authorization": "Bearer b635a7ea-1361-4cd8-9a07-bc3c12b2cf9e",
    "Content-Type": "application/vnd.seek.advertisement+json; charset=utf-8"
  },
  "body": {
    "thirdParties": {
      "advertiserId": "1"
    },
    "advertisementType": "Classic",
    "jobTitle": "Exciting Senior Developer role in a great CBD location. Great $$$",
    "location": {
      "id": "EuropeRussia",
      "areaId": "RussiaEasternEurope"
    },
    "subclassificationId": "AerospaceEngineering",
    "workType": "FullTime",
    "salary": {
      "type": "AnnualPackage",
      "minimum": 2.0,
      "maximum": 1.0
    },
    "jobSummary": "Developer job",
    "advertisementDetails": "Exciting, do I need to say more?"
  }
}
```
Ad Posting API will respond with:
```json
{
  "status": 422,
  "headers": {
    "Content-Type": "application/vnd.seek.advertisement-error+json; version=1; charset=utf-8"
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

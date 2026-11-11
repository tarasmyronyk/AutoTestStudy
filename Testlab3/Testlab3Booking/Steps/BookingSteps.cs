using System;
using System.Text.Json; 
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators; 
using TechTalk.SpecFlow;

namespace Testlab3.Steps
{
    [Binding]
    public class BookingSteps
    {
        private RestClient _client;
        private RestRequest _request;
        private RestResponse _response;

        private string _currentBookingId;

        [Given(@"I set the base URL to ""(.*)""")]
        public void GivenISetTheBaseUrlTo(string baseUrl)
        {
            var options = new RestClientOptions(baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator("admin", "password123")
            };
            _client = new RestClient(options);
        }

        [When(@"I send a GET request to ""(.*)""")]
        public void WhenISendAGETRequestTo(string endpoint)
        {
            _request = new RestRequest(endpoint, Method.Get);
            _response = _client.Execute(_request);
        }

        [When(@"I send a POST request to ""(.*)"" with the following details:")]
        public void WhenISendAPOSTRequestToWithTheFollowingDetails(string endpoint, TechTalk.SpecFlow.Table table)
        {
            _request = new RestRequest(endpoint, Method.Post);

            _request.AddHeader("Content-Type", "application/json");
            _request.AddHeader("Accept", "application/json");

            var body = CreateBookingBody(table.Rows[0]);
            _request.AddJsonBody(body);

            _response = _client.Execute(_request);

            if ((int)_response.StatusCode == 200)
            {
                var jsonDocument = JsonDocument.Parse(_response.Content);
                _currentBookingId = jsonDocument.RootElement.GetProperty("bookingid").ToString();
            }
        }

        [When(@"I send a PUT request to the created booking with the updated details:")]
        public void WhenISendAPUTRequestToTheCreatedBookingWithTheUpdatedDetails(TechTalk.SpecFlow.Table table)
        {
            string endpoint = $"/booking/{_currentBookingId}";

            _request = new RestRequest(endpoint, Method.Put);
            _request.AddHeader("Content-Type", "application/json");
            _request.AddHeader("Accept", "application/json");


            var body = CreateBookingBody(table.Rows[0]);
            _request.AddJsonBody(body);

            _response = _client.Execute(_request);
        }

        [When(@"I send a DELETE request to the created booking")]
        public void WhenISendADELETERequestToTheCreatedBooking()
        {
            string endpoint = $"/booking/{_currentBookingId}";

            _request = new RestRequest(endpoint, Method.Delete);

            _response = _client.Execute(_request);
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            int actualStatusCode = (int)_response.StatusCode;
            Assert.That(actualStatusCode, Is.EqualTo(expectedStatusCode),
                $"Expected status {expectedStatusCode} but got {actualStatusCode}. Content: {_response.Content}");
        }

        private object CreateBookingBody(TableRow row)
        {
            return new
            {
                firstname = row["firstname"],
                lastname = row["lastname"],
                totalprice = int.Parse(row["totalprice"]),
                depositpaid = bool.Parse(row["depositpaid"]),
                bookingdates = new
                {
                    checkin = row["checkin"],
                    checkout = row["checkout"]
                },
                additionalneeds = row["additionalneeds"]
            };
        }
    }
}
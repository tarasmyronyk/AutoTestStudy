using System;
using NUnit.Framework;
using RestSharp;
using TechTalk.SpecFlow;

namespace Testlab3.Steps
{
    [Binding]
    public class PetstoreSteps
    {
        private RestClient _client;
        private RestRequest _request;
        private RestResponse _response;
        private string _currentPetId;

        [Given(@"I set the base URL to ""(.*)""")]
        public void GivenISetTheBaseUrlTo(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        [When(@"I send a POST request to ""(.*)"" with the following details:")]
        public void WhenISendAPOSTRequestToWithTheFollowingDetails(string endpoint, TechTalk.SpecFlow.Table table)
        {
            _request = new RestRequest(endpoint, Method.Post);
            _request.AddHeader("Content-Type", "application/json");
            _request.AddHeader("Accept", "application/json");

            var body = CreatePetBody(table.Rows[0]);
            _request.AddJsonBody(body);

            _response = _client.Execute(_request);

            if ((int)_response.StatusCode == 200)
            {
                _currentPetId = table.Rows[0]["id"];
            }
        }

        [When(@"I send a GET request for the created pet")]
        public void WhenISendAGETRequestForTheCreatedPet()
        {
            string endpoint = $"/pet/{_currentPetId}";
            _request = new RestRequest(endpoint, Method.Get);
            _response = _client.Execute(_request);
        }

        [When(@"I send a DELETE request for the created pet")]
        public void WhenISendADELETERequestForTheCreatedPet()
        {
            string endpoint = $"/pet/{_currentPetId}";
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

        private object CreatePetBody(TechTalk.SpecFlow.TableRow row)
        {
            return new
            {
                id = long.Parse(row["id"]),
                name = row["name"],
                category = new
                {
                    id = 1,
                    name = row["category"]
                },
                status = row["status"]
            };
        }
    }
}
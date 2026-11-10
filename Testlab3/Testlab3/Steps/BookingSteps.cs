using System;
using System.Text.Json; // Потрібно для парсингу JSON відповіді
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators; // Потрібно для базової авторизації
using TechTalk.SpecFlow;

namespace Testlab3.Steps
{
    [Binding]
    public class BookingSteps
    {
        private RestClient _client;
        private RestRequest _request;
        private RestResponse _response;

        // Змінна для зберігання ID поточного бронювання між кроками
        private string _currentBookingId;

        [Given(@"I set the base URL to ""(.*)""")]
        public void GivenISetTheBaseUrlTo(string baseUrl)
        {
            // Налаштовуємо клієнт та ОДРАЗУ додаємо базову авторизацію для всіх майбутніх запитів
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

            // ЗБЕРІГАЄМО ID БРОНЮВАННЯ ДЛЯ НАСТУПНИХ КРОКІВ
            if ((int)_response.StatusCode == 200)
            {
                // Парсимо JSON відповідь: {"bookingid": 123, "booking": {...}}
                var jsonDocument = JsonDocument.Parse(_response.Content);
                _currentBookingId = jsonDocument.RootElement.GetProperty("bookingid").ToString();
            }
        }

        // НОВИЙ КРОК ДЛЯ ДИНАМІЧНОГО PUT
        [When(@"I send a PUT request to the created booking with the updated details:")]
        public void WhenISendAPUTRequestToTheCreatedBookingWithTheUpdatedDetails(TechTalk.SpecFlow.Table table)
        {
            // Формуємо endpoint, використовуючи збережений ID (наприклад, "/booking/123")
            string endpoint = $"/booking/{_currentBookingId}";

            _request = new RestRequest(endpoint, Method.Put);
            _request.AddHeader("Content-Type", "application/json");
            _request.AddHeader("Accept", "application/json");

            // Нам більше не треба вручну додавати Authorization, бо ми додали Authenticator у клієнт!

            var body = CreateBookingBody(table.Rows[0]);
            _request.AddJsonBody(body);

            _response = _client.Execute(_request);
        }

        // НОВИЙ КРОК ДЛЯ ДИНАМІЧНОГО DELETE
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

        // --- ДОПОМІЖНИЙ МЕТОД ---
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
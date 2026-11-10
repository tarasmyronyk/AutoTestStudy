Feature: Booking API CRUD Operations

  Scenario: Get all bookings successfully
    Given I set the base URL to "https://restful-booker.herokuapp.com"
    When I send a GET request to "/booking"
    Then the response status code should be 200

  Scenario: Create a new booking
    Given I set the base URL to "https://restful-booker.herokuapp.com"
    When I send a POST request to "/booking" with the following details:
      | firstname | lastname | totalprice | depositpaid | checkin    | checkout   | additionalneeds |
      | Taras     | Myronyk  | 150        | true        | 2026-11-01 | 2026-11-10 | Breakfast       |
    Then the response status code should be 200

  Scenario: Update an existing booking dynamically
    Given I set the base URL to "https://restful-booker.herokuapp.com"

    When I send a POST request to "/booking" with the following details:
      | firstname | lastname | totalprice | depositpaid | checkin    | checkout   | additionalneeds |
      | Taras     | Myronyk  | 150        | true        | 2026-11-01 | 2026-11-10 | Breakfast       |
    When I send a PUT request to the created booking with the updated details:
      | firstname | lastname | totalprice | depositpaid | checkin    | checkout   | additionalneeds |
      | Taras     | Myronyk  | 250        | true        | 2026-11-01 | 2026-11-10 | Dinner          |
    Then the response status code should be 200

  Scenario: Delete a booking dynamically
    Given I set the base URL to "https://restful-booker.herokuapp.com"
    When I send a POST request to "/booking" with the following details:
      | firstname | lastname | totalprice | depositpaid | checkin    | checkout   | additionalneeds |
      | Delete    | Me       | 100        | false       | 2026-11-01 | 2026-11-10 | None            |
    When I send a DELETE request to the created booking
    Then the response status code should be 201
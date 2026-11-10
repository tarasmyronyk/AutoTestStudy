Feature: Petstore API CRUD Operations

  Scenario: Create a new pet successfully
    Given I set the base URL to "https://petstore.swagger.io/v2"
    When I send a POST request to "/pet" with the following details:
      | id | name | category | status    |
      | 1  | Dog  | Animal   | available |
    Then the response status code should be 200

  Scenario: Get the created pet by ID successfully
    Given I set the base URL to "https://petstore.swagger.io/v2"
    When I send a POST request to "/pet" with the following details:
      | id | name | category | status    |
      | 1  | Dog  | Animal   | available |
    When I send a GET request for the created pet
    Then the response status code should be 200

  Scenario: Delete the created pet by ID successfully
    Given I set the base URL to "https://petstore.swagger.io/v2"
    When I send a POST request to "/pet" with the following details:
      | id | name | category | status    |
      | 1  | Dog  | Animal   | available |
    When I send a DELETE request for the created pet
    Then the response status code should be 200
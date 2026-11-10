Feature: Search Customer
  In order to manage customers
  As a Bank Manager
  I want to be able to search for a specific customer

Scenario: Bank Manager searches for an existing customer
  Given I navigate to the banking project website
  When I click on the Bank Manager Login button
  And I go to the Customers tab
  And I search for customer "Harry"
  Then I should see customer "Harry" in the search results
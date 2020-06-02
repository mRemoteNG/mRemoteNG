Feature: CredentialRepositoryList
	As a user with several environments
	I would like to load multiple credential repositories
	In order to keep credentials separate

@credentials
Scenario: Add a new credential repository
	Given I have a credential repository list
	And It has 0 repositories set up
	When I press add and complete the creation wizard
	Then I will have 1 credential repository

Scenario: Remove a credential repository
	Given I have a credential repository list
	And It has 2 repositories set up
	When I remove the first repository
	Then I will have 1 credential repository
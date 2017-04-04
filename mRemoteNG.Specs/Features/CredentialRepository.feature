Feature: CredentialRepository
	As a user with several environments
	I would like to load multiple credential repositories
	In order to keep credentials separate

@credentials
Scenario: Load credential repository
	Given I have a credential repository
	And the credential repository is unloaded
	When I click load
	Then the credential repository is loaded

Scenario: Add credential record
	Given I have a credential repository
	And The credential repository is loaded
	And The repository has 0 credentials
	When I click add credential
	Then the repository has 1 credentials

Scenario: Unload credential repository
	Given I have a credential repository
	And The credential repository is loaded
	And The repository has 1 credentials
	When I click unload
	Then the credentials in the repository will no longer be available
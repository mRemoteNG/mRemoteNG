@ui
Feature: SmokeTest
    As a developer
    I want to verify that mRemoteNG launches correctly
    So that I know the application is not broken at startup

Scenario: Application launches and shows main window
    Given the application is running
    Then the main window title contains "mRemoteNG"

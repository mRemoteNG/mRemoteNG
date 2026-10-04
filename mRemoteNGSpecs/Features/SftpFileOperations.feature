@sftp
Feature: SFTP file operations
	As a user connecting to a host over SSH
	I want to browse and transfer files with the embedded SFTP client
	So that I can manage remote files without leaving mRemoteNG

Background:
	Given a running SFTP server
	And I connect the SFTP file service

Scenario: Connecting establishes a usable session
	Then the SFTP service reports it is connected
	And the home path is known

Scenario: Listing a directory shows uploaded files
	Given a remote file "list-me.txt" containing "listing works" exists in "/upload"
	When I list the directory "/upload"
	Then the listing contains a file named "list-me.txt"

Scenario: Uploading a local file
	Given a local file "to-upload.txt" containing "uploaded payload"
	When I upload it to "/upload/to-upload.txt"
	Then the listing of "/upload" contains a file named "to-upload.txt"

Scenario: Downloading a remote file
	Given a remote file "to-download.txt" containing "downloaded payload" exists in "/upload"
	When I download "/upload/to-download.txt"
	Then the downloaded file contains "downloaded payload"

Scenario: Creating a directory
	When I create the directory "/upload/new-folder"
	Then the listing of "/upload" contains a directory named "new-folder"

Scenario: Renaming a file
	Given a remote file "old-name.txt" containing "rename me" exists in "/upload"
	When I rename "/upload/old-name.txt" to "/upload/new-name.txt"
	Then the listing of "/upload" contains a file named "new-name.txt"
	And the listing of "/upload" does not contain a file named "old-name.txt"

Scenario: Deleting a file
	Given a remote file "delete-me.txt" containing "temporary" exists in "/upload"
	When I delete the file "/upload/delete-me.txt"
	Then the listing of "/upload" does not contain a file named "delete-me.txt"

Scenario: Deleting a directory
	Given a remote directory "/upload/dir-to-delete" exists
	When I delete the directory "/upload/dir-to-delete"
	Then the listing of "/upload" does not contain a directory named "dir-to-delete"

**************************
Credential Vault Connector
**************************

mRemote supports fetching credentials from external credential vaults. This allows providing credentials to the connection without storing sensitive information in the config file, which has numerous benefits (security, auditing, rotating passwords, etc).
Three password vaults are currently supported:

- Delinea Secret Server
- Clickstudios Passwordstate
- 1Password

The feature is implemented for RDP, RDP Gateway and SSH connections.

Before initiating a connection mRemote will access your Password Vault API and fetch the secret. For this to work the API endpoint URL and access credentials need to be specified. A popup will show up if this information has not yet been set.

.. figure:: /images/credvault02.png


Instead of setting username/password/domain directly in mRemote, leave these fields empty and specify the secret id instead: 

.. figure:: /images/credvault01.png

The secret id is the unique identifier of your secret.


Delinea Secret Server
---------------------

The secret ID can be found in the url of your secret: https://cred.domain.local/SecretServer/app/#/secret/3318/general  -> the secret id is 3318

Authentication works with WinAuth/SSO (OnPremise) and Username/Password (OnPremise, Cloud). MFA via OTP is supported.


Clickstudios PasswordState
--------------------------

The secred ID can be found in the UI after enabling "toggle visibility of web API IDs" in the "List Administrator Actions" dropdown

.. figure:: /images/credvault03.png

Authentication works with WinAuth/SSO and list-based API-Keys. MFA via OTP is supported.

- There is currently no support for token authentication, so if your API has MFA enabled, you need to specify a fresh OTP code quite frequently
- If you are using list-based API keys to access the vault, only one API key can currently be specified in the connector configuration


1Password
---------

The secret reference uses the 1Password URL format. Specify the secret in the mRemote ``UserViaAPI`` field using this format:

    ``op://vault-name/item-name``
    ``op:///item-name``

Or with an optional account parameter:

    ``op://vault-name/item-name?account=account-name``

Where:

- ``vault-name`` is the name or id of your 1Password vault
- ``item-name`` is the name or id of the item containing the credentials
- ``account-name`` (optional) specifies which 1Password account to use if you have multiple accounts

Field Mapping
~~~~~~~~~~~~~

The 1Password integration retrieves the following fields from your 1Password item:

- **Username**: Fields with purpose "USERNAME" or label "username"
- **Password**: Fields with purpose "PASSWORD" or label "password"
- **Domain**: String fields with label "domain"
- **SSH Private Key**: Fields of type "SSHKEY"

At least a password or SSH private key must be present in the item.

Prerequisites
~~~~~~~~~~~~~

The 1Password CLI (``op.exe``) must be installed and available in your system PATH. You can download it from https://1password.com/downloads/command-line/

The CLI requires the GUI application to run, because the GUI provides the authentication prompt. You can verify authentication by running:

    ``op signin``

Configuration Notes
~~~~~~~~~~~~~~~~~~~

- Server category items and some other item types may not have the ``purpose`` metadata set on their username/password fields. In these cases, the integration will fall back to matching by field label ("username" and "password").
- You can modify field labels in 1Password to match the expected conventions if needed.
- The domain field is optional and should be a string field with the label "domain".


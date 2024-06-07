**************************
Credential Vault Connector
**************************

mRemote supports fetching credentials from external credential vaults. This allows providing credentials to the connection without storing sensitive information in the config file, which has numerous benefits (security, auditing, rotating passwords, etc).
Two password vaults are currently supported: 

- Delinea Secret Server
- Clickstudios Passwordstate

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


<p align="center">
  <img width="500" src="https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/develop/Tools/img/logo.png">
</p>
  
<p align="center">
	This is a fork of <a href="https://github.com/mRemoteNG/mRemoteNG">mRemoteNG</a> (develop branch). It contains an interface to the Secret Server API so mRemote can connect to Thycotic Secret Server and fetch credentials. No credentials are therefore stored on the local host, which is a great improvement to security. This fork comes without support, but i will try to keep it updated once in a while.
</p>

## Features

The only change to the original project is a connector to the Secret Server REST API.
Set up your folder or connection like this:

<img src="https://raw.githubusercontent.com/tecxx/mRemoteNG/develop/usage1.png">

set username to SSAPI:xxxx (where xxxx is the secret ID you want to fetch)
leave password and domain empty

when initiating the connection, a prompt appears to ask for your secret server installation data. 

<img src="https://raw.githubusercontent.com/tecxx/mRemoteNG/develop/usage2.png">

you only need to fill out this form once, the data gets stored in the registry.

happy mRemoting!



### Download

If you can, please download the source and compile yourself. 
If you need a portable binary, download it from <a href="https://web.tresorit.com/l/UDpGl#cOGSIV0fhPEafZJNatn31A">here</a>.



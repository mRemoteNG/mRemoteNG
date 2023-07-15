openssl enc -base64 -aes-256-cbc -md sha512 -pbkdf2 -iter 100000 -d -in cert/CodeSigning_Cert_mRemoteNG_Certum.enc -out cert/CodeSigning_Cert_mRemoteNG_Certum.cer
pause